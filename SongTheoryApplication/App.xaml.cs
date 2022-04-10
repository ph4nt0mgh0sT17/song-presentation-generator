using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Views;

namespace SongTheoryApplication;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public sealed partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
    }

    public new static App Current => (App) Application.Current;

    public IServiceProvider Services { get; }

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