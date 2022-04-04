using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all services into Dependency Injection Service Collection for use in the application.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> that contains all services, repositories and other
    ///     objects.
    /// </param>
    public static void AddAllServices(this IServiceCollection services)
    {
        services.AddSingleton<ISongService, SongService>();
        services.AddSingleton<IPresentationGeneratorService, PresentationGeneratorService>();
    }

    /// <summary>
    ///     Adds all repositories into Dependency Injection Service Collection for use in the application.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> that contains all services, repositories and other
    ///     objects.
    /// </param>
    public static void AddAllRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ILocalSongRepository, LocalSongRepository>();
    }

    /// <summary>
    ///     Adds all View Models into Dependency Injection Service Collection for use in the application.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> that contains all services, repositories and other
    ///     objects.
    /// </param>
    public static void AddAllViewModels(this IServiceCollection services)
    {
        services.AddTransient<CreateSongWindowViewModel>();
        services.AddTransient<MainWindowViewModel>();
    }
}