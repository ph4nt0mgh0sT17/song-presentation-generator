using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;
/// <summary>
/// Interaction logic for GenerateSongsPresentationWindow.xaml
/// </summary>
public partial class GenerateSongsPresentationWindow : Window
{
    public GenerateSongsPresentationWindow()
    {
        InitializeComponent();
        var viewModel = Ioc.Default.GetService<GenerateSongPresentationViewModel>();

        DataContext = viewModel;

        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var generateSongsPresentationWindowSettings = applicationConfiguration.WindowsSettings.GenerateSongsPresentationWindow;

        if (generateSongsPresentationWindowSettings.Width != 0 || generateSongsPresentationWindowSettings.Height != 0 || generateSongsPresentationWindowSettings.Top != 0 || generateSongsPresentationWindowSettings.Left != 0)
        {
            Width = generateSongsPresentationWindowSettings.Width;
            Height = generateSongsPresentationWindowSettings.Height;
            Left = generateSongsPresentationWindowSettings.Left;
            Top = generateSongsPresentationWindowSettings.Top;

            if (generateSongsPresentationWindowSettings.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }
    }

    public GenerateSongPresentationViewModel ViewModel
    {
        get
        {
            if (DataContext is GenerateSongPresentationViewModel generateSongPresentationViewModel)
                return generateSongPresentationViewModel;

            throw new InvalidOperationException("The DataContext of this window must be GenerateSongPresentationViewModel.");
        }
    }

    private void GenerateSongsPresentation_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var generateSongsPresentationWindowSettings = applicationConfiguration.WindowsSettings.GenerateSongsPresentationWindow;

        if (WindowState == WindowState.Maximized)
        {
            generateSongsPresentationWindowSettings.Width = (int)RestoreBounds.Width;
            generateSongsPresentationWindowSettings.Height = (int)RestoreBounds.Height;
            generateSongsPresentationWindowSettings.Top = (int)RestoreBounds.Top;
            generateSongsPresentationWindowSettings.Left = (int)RestoreBounds.Left;
            generateSongsPresentationWindowSettings.Maximized = true;
        }

        else
        {
            generateSongsPresentationWindowSettings.Width = (int)Width;
            generateSongsPresentationWindowSettings.Height = (int)Height;
            generateSongsPresentationWindowSettings.Top = (int)Top;
            generateSongsPresentationWindowSettings.Left = (int)Left;
            generateSongsPresentationWindowSettings.Maximized = false;
        }

        configuration.GetSection("ApplicationConfiguration").Bind(applicationConfiguration);

        var applicationConfigurationJson = JsonSerializer.Serialize(applicationConfiguration, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("ApplicationConfiguration.json", applicationConfigurationJson);
    }
}
