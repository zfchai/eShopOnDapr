using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Utilities;

public static class ConfigurationExtensions
{
    // 跟踪已添加的密钥存储
    private static readonly ConcurrentDictionary<string, bool> _addedStores = new();

    // 构建Dapr客户端
    private static readonly DaprClient _daprClient = new DaprClientBuilder().Build();

    /// <summary>
    /// 检查配置中是否存在指定的键，用于判断密钥存储是否已添加
    /// </summary>
    /// <param name="configuration">配置实例</param>
    /// <param name="key">要检查的键名</param>
    /// <returns>如果键存在则返回true，否则返回false</returns>
    public static bool IsSecretStoreAdded(this IConfigurationManager configuration, string key)
    {
        try
        {
            var value = configuration[key];
            return value != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 安全地添加 Dapr 密钥存储配置源，避免重复添加
    /// </summary>
    /// <param name="builder">配置构建器</param>
    /// <param name="storeName">密钥存储名称</param>
    /// <returns>配置构建器实例</returns>
    public static IConfigurationBuilder TryAddDaprSecretStore(this IConfigurationBuilder builder, string storeName)
    {
        // 如果尚未添加此存储，则添加它
        if (_addedStores.TryAdd(storeName, true))
        {
            builder.AddDaprSecretStore(storeName, _daprClient);
        }
        return builder;
    }

    /// <summary>
    /// 检查指定的密钥存储是否已添加
    /// </summary>
    /// <param name="builder">配置构建器</param>
    /// <param name="storeName">密钥存储名称</param>
    /// <returns></returns>
    public static bool IsDaprSecretStoreAdded(this IConfigurationBuilder builder, string storeName)
    {
        return _addedStores.ContainsKey(storeName);
    }
}
