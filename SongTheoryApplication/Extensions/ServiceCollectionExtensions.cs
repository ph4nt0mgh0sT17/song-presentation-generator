using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services)
    {
        services.AddSingleton<ISongService, SongService>();
        services.AddSingleton<IPresentationGeneratorService, PresentationGeneratorService>();
    }
    
    public static void AddAllViewModels(this IServiceCollection services)
    {
        services.AddTransient<CreateSongWindowViewModel>();
        services.AddTransient<MainWindowViewModel>();
    }
}