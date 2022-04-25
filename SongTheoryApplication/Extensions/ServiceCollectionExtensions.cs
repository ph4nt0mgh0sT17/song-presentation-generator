using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all repositories into Dependency Injection Service Collection for use in the application.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> that contains all services, repositories and other
    ///     objects.
    /// </param>
    public static IServiceCollection AddAllRepositories(this IServiceCollection services)
    {
        services.AddTransient<ILocalSongRepository, LocalSongRepository>();

        return services;
    }


    /// <summary>
    /// Adds Serilog Logger to <see cref="IServiceCollection"/> to be used in services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> that contains all services, repositories and other objects.</param>
    /// <returns></returns>
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("SongTheoryApplication.log", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(configure => configure.AddSerilog());

        return services;
    }

    /// <summary>
    ///     Adds the configuration to the 
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> that contains all services, repositories and other
    ///     objects.
    /// </param>
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        var configuration = BuildConfigurationFromConfigurationFile();

        if (configuration is null)
        {
            services.AddSingleton<IConfiguration>(new NullConfiguration());
            return services;
        }

        services.AddSingleton<IConfiguration>(configuration);

        return services;
    }

    private static IConfigurationRoot? BuildConfigurationFromConfigurationFile()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("ApplicationConfiguration.json")
                .Build();
            
            Log.Logger.Information("The application configuration has been found successfully");

            return configuration;
        }
        catch (IOException ex)
        {
            Log.Logger.Error(ex, "Could not find the configuration file: 'ApplicationConfiguration.json'");
            return null;
        }
    }
}