﻿using Microsoft.Extensions.DependencyInjection;

namespace AlfredGPT.Common;

public static class ServiceLocator
{
    private static IServiceProvider? _serviceProvider;

    public static void Build(Action<ServiceCollection> configureServices)
    {
        if (_serviceProvider != null) throw new InvalidOperationException($"{nameof(ServiceLocator)} is already built.");
        var serviceCollection = new ServiceCollection();
        configureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public static object Resolve(Type type, object? key = null)
    {
        if (_serviceProvider == null) throw new InvalidOperationException($"{nameof(ServiceLocator)} is not built.");
        if (key == null) return _serviceProvider.GetRequiredService(type);
        return _serviceProvider.GetRequiredKeyedService(type, key);
    }

    public static T Resolve<T>(object? key = null) where T : class
    {
        return (T)Resolve(typeof(T), key);
    }
}