using Aspire.Hosting.Dapr;
using Aspire.Hosting.MailDev;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

// Add a parameter
var pgUser = builder.AddParameter("PgUser");
var pgPassword = builder.AddParameter("PgPassword", secret: true);

var maildevUser = builder.AddParameter("MaildevUser");
var maildevPassword = builder.AddParameter("MaildevPassword", secret: true);

// Add a dapr statestore and pubsub
var stateStore = builder.AddDaprStateStore("eshopondapr-statestore");
var pubSub = builder.AddDaprPubSub("eshopondapr-pubsub");
//var secretStore = builder.AddDaprComponent("eshopondapr-secretstore", "secretstores.local.file");

var maildev = builder
      .AddMailDev("maildev", options => options
      .WithPorts(httpPort: 5500, smtpPort: 1025)
      .WithAuth(maildevUser.Resource.Value, maildevPassword.Resource.Value));

var rabbitmq = builder
      .AddRabbitMQ(name:"rabbitmq", port: 5672)
      .WithImageTag("3-management-alpine")
      .WithDataVolume("eshorp_rabbitmq_data");

var redis = builder
      .AddRedis("redis", port: 5379)
      .WithImageTag("7.4.0-alpine3.20")
      .WithDataVolume("eshorp_redis_data");

var seq = builder
      .AddSeq(name:"seq", port: 5340)
      .WithImageTag("latest")
      .WithEnvironment("ACCEPT_EULA", "Y")
      .WithDataVolume("eshorp_redis_data");

builder.Services.AddHttpClient("ZipkinExporter", 
    configureClient: (client) => client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value"));

var postgres = builder
      .AddPostgres("postgresql", pgUser, pgPassword, port: 15432)
      .WithImageTag("16.6-alpine3.20")
      .WithDataVolume("eshorp_postgres_data")
      .WithPgAdmin(c => c.WithHostPort(5050));

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
      .WithReference(redis)
      .WithReference(identityService)
      .WithReference(pubSub)
      .WithReference(rabbitmq)
      .WithReference(seq);

var catalogService = builder.AddProject<Projects.Catalog_API>("catalog-api")
      .WithDaprSidecar()
      .WithReference(maildev)
      .WithReference(catalogDb);

var orderingService = builder.AddProject<Projects.Ordering_API>("ordering-api")
      .WithDaprSidecar()
      .WithReference(orderingDb);

var paymentService = builder.AddProject<Projects.Payment_API>("payment-api")
      .WithDaprSidecar()
      .WithReference(pubSub)
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
