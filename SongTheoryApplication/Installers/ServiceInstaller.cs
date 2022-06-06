using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Installers;

public class ServiceInstaller : IInstaller
{
    /// <summary>
    /// Configure the DI services to inject all application services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        var serviceImplementations = GetAllServiceImplementations();

        serviceImplementations.ForEach(serviceImplementation =>
        {
            var serviceInterface = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsInterface)
                .FirstOrDefault(p => p.IsAssignableFrom(serviceImplementation.Type));

            if (serviceInterface == null) return;

            if (serviceImplementation.IsSingleton)
            {
                services.AddSingleton(serviceInterface, serviceImplementation.Type);
            }

            else
            {
                services.AddScoped(serviceInterface, serviceImplementation.Type);
            }

        });
    }

    /// <summary>
    /// Gets all service implementations from the application.
    /// </summary>
    /// <returns>The <see cref="List{T}"/> of all service implementation types.</returns>
    private List<ServiceType> GetAllServiceImplementations()
    {
        var serviceImplementations = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => !p.IsInterface)
            .Where(p => p.GetCustomAttribute(typeof(ServiceAttribute)) != null)
            .Select(x => new ServiceType(x, x.GetCustomAttribute<ServiceAttribute>().IsSingleton))
            .ToList();

        return serviceImplementations;
    }

    private record ServiceType(Type Type, bool IsSingleton);

}