using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Installers;

namespace SongTheoryApplication.Extensions;

public static class InstallerExtensions
{
    /// <summary>
    /// An extension that finds all IInstaller implementation classes and invokes their ConfigureServices method.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> that includes all injected services.</param>
    public static void InstallAllServices(this IServiceCollection services)
    {
        var installers = GetAllInstallers();
        installers.ForEach(installer => installer.ConfigureServices(services));
    }

    private static List<IInstaller> GetAllInstallers()
    {
        var installers = typeof(App)
            .Assembly
            .ExportedTypes
            .Where(IsInstaller)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();
        return installers;
    }

    private static bool IsInstaller(Type x)
    {
        return typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract;
    }
}