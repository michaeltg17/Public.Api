//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Serilog;
//using Serilog.Extensions.Hosting;
//using Serilog.Extensions.Logging;

//namespace IntegrationTests.Serilog;

//public static class SerilogServiceCollectionExtensions
//{
//    /// <summary>
//    /// Sets Serilog as the logging provider.
//    /// </summary>
//    /// <param name="collection">The service collection to use.</param>
//    /// <param name="logger">The Serilog logger; if not supplied, the static <see cref="Serilog.Log"/> will be used.</param>
//    /// <param name="dispose">When <c>true</c>, dispose <paramref name="logger"/> when the framework disposes the provider. If the
//    /// logger is not specified but <paramref name="dispose"/> is <c>true</c>, the <see cref="Serilog.Log.CloseAndFlush()"/> method will be
//    /// called on the static <see cref="Serilog.Log"/> class instead.</param>
//    /// <param name="providers">A <see cref="LoggerProviderCollection"/> registered in the Serilog pipeline using the
//    /// <c>WriteTo.Providers()</c> configuration method, enabling other <see cref="Microsoft.Extensions.Logging.ILoggerProvider"/>s to receive events. By
//    /// default, only Serilog sinks will receive events.</param>
//    /// <returns>The service collection.</returns>
//    public static IServiceCollection AddSerilog(
//        this IServiceCollection collection, 
//        ILogger logger = null, 
//        bool dispose = false,
//        LoggerProviderCollection providers = null)
//    {
//        if (collection == null) throw new ArgumentNullException(nameof(collection));

//        if (providers != null)
//        {
//            collection.AddSingleton<ILoggerFactory>(services =>
//            {
//                var factory = new SerilogLoggerFactory(logger, dispose, providers);

//                foreach (var provider in services.GetServices<ILoggerProvider>())
//                    factory.AddProvider(provider);

//                return factory;
//            });
//        }
//        else
//        {
//            collection.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(logger, dispose));
//        }

//        if (logger != null)
//        {
//            // This won't (and shouldn't) take ownership of the logger. 
//            collection.AddSingleton(logger);

//            // Still need to use RegisteredLogger as it is used by ConfigureDiagnosticContext.
//            collection.AddSingleton(new RegisteredLogger(logger));
//        }
//        bool useRegisteredLogger = logger != null;
//        ConfigureDiagnosticContext(collection, useRegisteredLogger);

//        return collection;
//    }

//    /// <summary>Sets Serilog as the logging provider.</summary>
//    /// <param name="collection">The service collection to use.</param>
//    /// <param name="configureLogger">The delegate for configuring the <see cref="Serilog.LoggerConfiguration" /> that will be used to construct a <see cref="Serilog.Core.Logger" />.</param>
//    /// <param name="preserveStaticLogger">Indicates whether to preserve the value of <see cref="Serilog.Log.Logger"/>.</param>
//    /// <param name="writeToProviders">By default, Serilog does not write events to <see cref="ILoggerProvider"/>s registered through
//    /// the Microsoft.Extensions.Logging API. Normally, equivalent Serilog sinks are used in place of providers. Specify
//    /// <c>true</c> to write events to all providers.</param>
//    /// <returns>The service collection.</returns>
//    public static IServiceCollection AddSerilog(
//        this IServiceCollection collection,
//        Action<LoggerConfiguration> configureLogger,
//        bool preserveStaticLogger = false,
//        bool writeToProviders = false)
//    {
//        if (collection == null) throw new ArgumentNullException(nameof(collection));
//        if (configureLogger == null) throw new ArgumentNullException(nameof(configureLogger));
//        return AddSerilog(
//            collection,
//            (services, loggerConfiguration) =>
//                configureLogger(loggerConfiguration),
//            preserveStaticLogger: preserveStaticLogger,
//            writeToProviders: writeToProviders);
//    }

//    /// <summary>Sets Serilog as the logging provider.</summary>
//    /// <param name="collection">The service collection to use.</param>
//    /// <param name="configureLogger">The delegate for configuring the <see cref="Serilog.LoggerConfiguration" /> that will be used to construct a <see cref="Serilog.Core.Logger" />.</param>
//    /// <param name="preserveStaticLogger">Indicates whether to preserve the value of <see cref="Serilog.Log.Logger"/>.</param>
//    /// <param name="writeToProviders">By default, Serilog does not write events to <see cref="ILoggerProvider"/>s registered through
//    /// the Microsoft.Extensions.Logging API. Normally, equivalent Serilog sinks are used in place of providers. Specify
//    /// <c>true</c> to write events to all providers.</param>
//    /// <remarks>If the static <see cref="Log.Logger"/> is a bootstrap logger (see
//    /// <c>LoggerConfigurationExtensions.CreateBootstrapLogger()</c>), and <paramref name="preserveStaticLogger"/> is
//    /// not specified, the the bootstrap logger will be reconfigured through the supplied delegate, rather than being
//    /// replaced entirely or ignored.</remarks>
//    /// <returns>The service collection.</returns>
//    public static IServiceCollection AddSerilog(
//        this IServiceCollection collection,
//        Action<IServiceProvider, LoggerConfiguration> configureLogger,
//        bool preserveStaticLogger = false,
//        bool writeToProviders = false)
//    {
//        if (collection == null) throw new ArgumentNullException(nameof(collection));
//        if (configureLogger == null) throw new ArgumentNullException(nameof(configureLogger));
        
//        // This check is eager; replacing the bootstrap logger after calling this method is not supported.
//#if !NO_RELOADABLE_LOGGER
//        var reloadable = Log.Logger as ReloadableLogger;
//        var useReload = reloadable != null && !preserveStaticLogger;
//#else
//        const bool useReload = false;
//#endif
        
//        LoggerProviderCollection loggerProviders = null;
//        if (writeToProviders)
//        {
//            loggerProviders = new LoggerProviderCollection();
//        }
            
//        collection.AddSingleton(services =>
//        {
//            ILogger logger;
//#if !NO_RELOADABLE_LOGGER
//            if (useReload)
//            {
//                reloadable!.Reload(cfg =>
//                {
//                    if (loggerProviders != null)
//                        cfg.WriteTo.Providers(loggerProviders);
                        
//                    configureLogger(services, cfg);
//                    return cfg;
//                });
                    
//                logger = reloadable.Freeze();
//            }
//            else
//#endif
//            {
//                var loggerConfiguration = new LoggerConfiguration();

//                if (loggerProviders != null)
//                    loggerConfiguration.WriteTo.Providers(loggerProviders);

//                configureLogger(services, loggerConfiguration);
//                logger = loggerConfiguration.CreateLogger();
//            }

//            return new RegisteredLogger(logger);
//        });

//        collection.AddSingleton(services =>
//        {
//            // How can we register the logger, here, but not have MEDI dispose it?
//            // Using the `NullEnricher` hack to prevent disposal.
//            var logger = services.GetRequiredService<RegisteredLogger>().Logger;
//            return logger.ForContext(new NullEnricher());
//        });
            
//        collection.AddSingleton<ILoggerFactory>(services =>
//        {
//            var logger = services.GetRequiredService<RegisteredLogger>().Logger;
                
//            ILogger registeredLogger = null;
//            if (preserveStaticLogger)
//            {
//                registeredLogger = logger;
//            }
//            else
//            {
//                // Passing a `null` logger to `SerilogLoggerFactory` results in disposal via
//                // `Log.CloseAndFlush()`, which additionally replaces the static logger with a no-op.
//                Log.Logger = logger;
//            }

//            var factory = new SerilogLoggerFactory(registeredLogger, !useReload, loggerProviders);

//            if (writeToProviders)
//            {
//                foreach (var provider in services.GetServices<ILoggerProvider>())
//                    factory.AddProvider(provider);
//            }

//            return factory;
//        });

//        collection.ConfigureDiagnosticContext(collection, preserveStaticLogger);
        
//        return collection;
//    }

//    static void ConfigureDiagnosticContext(IServiceCollection collection, bool useRegisteredLogger)
//    {
//        if (collection == null) throw new ArgumentNullException(nameof(collection));

//        // Registered to provide two services...            
//        // Consumed by e.g. middleware
//        collection.AddSingleton(services =>
//        {
//            ILogger logger = useRegisteredLogger ? services.GetRequiredService<RegisteredLogger>().Logger : null;
//            return new DiagnosticContext(logger);
//        });
//        // Consumed by user code
//        collection.AddSingleton<IDiagnosticContext>(services => services.GetRequiredService<DiagnosticContext>());
//    }
//}
