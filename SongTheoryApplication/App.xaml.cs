using System;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Views;

namespace SongTheoryApplication;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public sealed partial class App : Application
{
    public App()
    {
        Ioc.Default.ConfigureServices(ConfigureServices());
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddAllServices();
        services.AddAllRepositories();
        services.AddAllViewModels();


        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}