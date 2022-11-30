using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class SongListWindow : Window
{
    public SongListWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService<SongListViewModel>();

        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var songListWindowSettings = applicationConfiguration.WindowsSettings.SongListWindow;

        if (songListWindowSettings.Width != 0 || songListWindowSettings.Height != 0 || songListWindowSettings.Top != 0 || songListWindowSettings.Left != 0)
        {
            Width = songListWindowSettings.Width;
            Height = songListWindowSettings.Height;
            Left = songListWindowSettings.Left;
            Top = songListWindowSettings.Top;

            if (songListWindowSettings.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }
    }

    public SongListViewModel ViewModel
    {
        get
        {
            if (DataContext is SongListViewModel songListViewModel)
                return songListViewModel;

            throw new InvalidOperationException("The DataContext of this window must be SongListViewModel.");
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var songListWindowSettings = applicationConfiguration.WindowsSettings.SongListWindow;

        if (WindowState == WindowState.Maximized)
        {
            songListWindowSettings.Width = (int)RestoreBounds.Width;
            songListWindowSettings.Height = (int)RestoreBounds.Height;
            songListWindowSettings.Top = (int)RestoreBounds.Top;
            songListWindowSettings.Left = (int)RestoreBounds.Left;
            songListWindowSettings.Maximized = true;
        }

        else
        {
            songListWindowSettings.Width = (int)Width;
            songListWindowSettings.Height = (int)Height;
            songListWindowSettings.Top = (int)Top;
            songListWindowSettings.Left = (int)Left;
            songListWindowSettings.Maximized = false;
        }

        configuration.GetSection("ApplicationConfiguration").Bind(applicationConfiguration);

        var applicationConfigurationJson = JsonSerializer.Serialize(applicationConfiguration, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("ApplicationConfiguration.json", applicationConfigurationJson);
    }
}