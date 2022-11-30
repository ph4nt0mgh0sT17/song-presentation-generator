using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class CreateSongWindow : Window
{
    public CreateSongWindow()
    {
        InitializeComponent();
        var createSongWindowViewModel = GetViewModel();

        DataContext = createSongWindowViewModel;

        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var createSongWindowSettings = applicationConfiguration.WindowsSettings.CreateSongWindow;

        if (createSongWindowSettings.Width != 0 || createSongWindowSettings.Height != 0 || createSongWindowSettings.Top != 0 || createSongWindowSettings.Left != 0)
        {
            Width = createSongWindowSettings.Width;
            Height = createSongWindowSettings.Height;
            Left = createSongWindowSettings.Left;
            Top = createSongWindowSettings.Top;

            if (createSongWindowSettings.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }
        
    }

    private CreateSongWindowViewModel GetViewModel()
    {
        var createSongWindowViewModel = Ioc.Default.GetService<CreateSongWindowViewModel>();
        Guard.IsNotNull(createSongWindowViewModel, nameof(createSongWindowViewModel));
        createSongWindowViewModel.CreateSongWindow = this;
        return createSongWindowViewModel;
    }

    private void CreateSong_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var createSongWindowSettings = applicationConfiguration.WindowsSettings.CreateSongWindow;
        
        if (WindowState == WindowState.Maximized)
        {
            createSongWindowSettings.Width = (int)RestoreBounds.Width;
            createSongWindowSettings.Height = (int)RestoreBounds.Height;
            createSongWindowSettings.Top = (int)RestoreBounds.Top;
            createSongWindowSettings.Left = (int)RestoreBounds.Left;
            createSongWindowSettings.Maximized = true;
        } 
        
        else
        {
            createSongWindowSettings.Width = (int)Width;
            createSongWindowSettings.Height = (int)Height;
            createSongWindowSettings.Top = (int)Top;
            createSongWindowSettings.Left = (int)Left;
            createSongWindowSettings.Maximized = false;
        }

        configuration.GetSection("ApplicationConfiguration").Bind(applicationConfiguration);

        var applicationConfigurationJson = JsonSerializer.Serialize(applicationConfiguration, new JsonSerializerOptions { WriteIndented = true});

        File.WriteAllText("ApplicationConfiguration.json", applicationConfigurationJson);
    }
}