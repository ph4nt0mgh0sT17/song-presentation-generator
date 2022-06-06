using System;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Views;
using SongTheoryApplication.Views.Windows;

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
        return new ServiceCollection()
            .AddLogger()
            .AddConfiguration()
            .InstallAllServices()
            .BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}