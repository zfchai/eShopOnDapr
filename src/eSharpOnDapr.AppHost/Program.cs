using Aspire.Hosting.MailDev;
using CommunityToolkit.Aspire.Hosting.Dapr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

// Add a parameter
var pgUser = builder.AddParameter("PgUser");
var pgPassword = builder.AddParameter("PgPassword", secret: true);

var maildevUser = builder.AddParameter("MaildevUser");
var maildevPassword = builder.AddParameter("MaildevPassword", secret: true);

var garnetPassword = builder.AddParameter("GarnetPassword");

// Add a dapr statestore and pubsub
//var stateStore = builder.AddDaprStateStore("eshopondapr-statestore");
//var pubsub = builder.AddDaprPubSub("eshopondapr-pubsub");
//var secretStore = builder.AddDaprComponent("eshopondapr-secretstore", "secretstores.local.file");

var maildev = builder
      .AddMailDev("maildev", async options => 
      {
          var user = await maildevUser.Resource.GetValueAsync(default);
          var pwd = await maildevPassword.Resource.GetValueAsync(default);

          ArgumentNullException.ThrowIfNullOrWhiteSpace(user, "MaildevUser");
          ArgumentNullException.ThrowIfNullOrWhiteSpace(pwd, "MaildevPassword");

          options
              .WithPorts(httpPort: 5500, smtpPort: 1025)
              .WithAuth(user, pwd);
      });

var rabbitmq = builder
      .AddRabbitMQ(name:"rabbitmq", port: 5672)
      .WithImageTag("3-management-alpine")
      .WithDataVolume("eshorp_rabbitmq_data");

//var redis = builder
//      .AddRedis("redis", port: 5379)
//      .WithImageTag("7.4.0-alpine3.20")
//      .WithDataVolume("eshorp_redis_data");

var garnet = builder
      .AddGarnet("cache", port: 6379, password: garnetPassword)
      .WithDataVolume("eshorp_garnet_data");

// 手动添加 RedisInsight（用于管理 Garnet）
var redisInsight = builder
    //.AddContainer("redisinsight", "redis/redisinsight", "latest")
    .AddContainer("redisinsight", "docker.io/redislabs/redisinsight", "2.70") // https://docker.aityp.com/r/docker.io/redislabs/redisinsight                                                                        
    .WithEndpoint(port: 8001, targetPort: 5540, scheme: "http", name: "http") // RedisInsight 默认容器端口是 5540
    .WithReference(garnet)
    .WithVolume("eshop_redisinsight_data", "/data")
    .WithEnvironment("REDISINSIGHT_HOST", "0.0.0.0")
    .WithEnvironment("REDISINSIGHT_PORT", "5540");

var seq = builder
      .AddSeq(name:"seq", port: 5340)
      .WithImageTag("latest")
      .WithEnvironment("ACCEPT_EULA", "Y")
      .WithDataVolume("eshorp_seq_data");

builder.Services.AddHttpClient("ZipkinExporter", 
    configureClient: (client) => client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value"));

var postgres = builder
      .AddPostgres("postgresql", pgUser, pgPassword, port: 15432)
      .WithImageTag("16.6-alpine3.20")
      .WithDataVolume("eshorp_postgres_data")
      .WithPgAdmin(
         c => c.WithImage("dpage/pgadmin4:9.2")
               .WithHostPort(5050)
      );

var catalogDb = postgres.AddDatabase("CatalogDb");
var identityDb = postgres.AddDatabase("IdentityDb");
var orderingDb = postgres.AddDatabase("OrderingDb");

var identityService = builder.AddProject<Projects.Identity_API>("identity-api")
      .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
      .WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:80")
      .WithEnvironment("IdentityUrl", "http://identity-api")
      .WithEnvironment("IdentityUrlExternal", "http://${ESHOP_EXTERNAL_DNS_NAME_OR_IP}:5105")
      .WithEnvironment("SeqServerUrl", "http://seq")
      .WithDaprSidecar()
      .WithReference(identityDb);

var shoppingService = builder.AddProject<Projects.Web_Shopping_HttpAggregator>("web-shopping-httpaggregator")
      .WithDaprSidecar()
      .WithReference(identityDb);

var blazorClientHost = builder.AddProject<Projects.BlazorClient_Host>("blazor-client-host")
      .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
      .WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:80")
      .WithEnvironment("ApiGatewayUrlExternal", "http://${ESHOP_EXTERNAL_DNS_NAME_OR_IP}:5202")
      .WithEnvironment("IdentityUrlExternal", "http://${ESHOP_EXTERNAL_DNS_NAME_OR_IP}:5105")
      .WithEnvironment("SeqServerUrl", "http://seq");

var basketService = builder.AddProject<Projects.Basket_API>("basket-api")
      .WithDaprSidecar()
      //.WithReference(redis) 
      .WithReference(garnet) 
      //.WithReference(stateStore)
      .WithReference(identityService)
      //.WithReference(pubsub)
      .WithReference(rabbitmq)
      .WithReference(seq);

var catalogService = builder.AddProject<Projects.Catalog_API>("catalog-api")
      .WithDaprSidecar()
      .WithReference(maildev)
      //.WithReference(secretStore)
      .WithReference(catalogDb);

var orderingService = builder.AddProject<Projects.Ordering_API>("ordering-api")
      .WithDaprSidecar()
      .WithReference(orderingDb);

var paymentService = builder.AddProject<Projects.Payment_API>("payment-api")
      .WithDaprSidecar()
      //.WithReference(pubsub)
      .WithReference(rabbitmq)
      .WithReference(seq);

// Workaround for https://github.com/dotnet/aspire/issues/2219
if (builder.Configuration.GetValue<string>("DAPR_CLI_PATH") is { } daprCliPath)
{
    builder.Services.Configure<DaprOptions>(options =>
    {
        options.DaprPath = daprCliPath;
    });
}

var app = builder.Build();

await app.RunAsync();
