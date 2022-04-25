using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Installers;

public class ViewModelInstaller : IInstaller
{
    public void ConfigureServices(IServiceCollection services)
    {
        var viewModelImplementations = GetAllViewModelImplementations();
        
        viewModelImplementations.ForEach(viewModelImplementation =>
        {
            services.AddTransient(viewModelImplementation);
        });
    }

    /// <summary>
    /// Gets all view model implementations from the application.
    /// </summary>
    /// <returns>The <see cref="List{T}"/> of all service implementation types.</returns>
    private static List<Type> GetAllViewModelImplementations()
    {
        var serviceImplementations = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => !p.IsInterface)
            .Where(p => p.GetCustomAttribute(typeof(ViewModelAttribute)) != null)
            .ToList();

        return serviceImplementations;
    }
}