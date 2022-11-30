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
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Models;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;
/// <summary>
/// Interaction logic for EditSongWindow.xaml
/// </summary>
public partial class EditSongWindow : Window
{
    public EditSongWindow(Song? song)
    {
        Guard.IsNotNull(song);
        InitializeComponent();

        var editSongWindowViewModel = new EditSongWindowViewModel(
            song,
            Ioc.Default.GetRequiredService<ISongService>(),
            Ioc.Default.GetRequiredService<IPresentationGeneratorService>(),
            Ioc.Default.GetRequiredService<IApplicationService>(),
            Ioc.Default.GetRequiredService<ILogger<EditSongWindowViewModel>>(),
            this, Ioc.Default.GetRequiredService<IShareService>()
        );

        DataContext = editSongWindowViewModel;

        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var editSongWindowSettings = applicationConfiguration.WindowsSettings.EditSongWindow;

        if (editSongWindowSettings.Width != 0 || editSongWindowSettings.Height != 0 || editSongWindowSettings.Top != 0 || editSongWindowSettings.Left != 0)
        {
            Width = editSongWindowSettings.Width;
            Height = editSongWindowSettings.Height;
            Left = editSongWindowSettings.Left;
            Top = editSongWindowSettings.Top;

            if (editSongWindowSettings.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var configuration = Ioc.Default.GetService<IConfiguration>();
        var applicationConfiguration = configuration.Get<ApplicationConfiguration>();

        var editSongWindowSettings = applicationConfiguration.WindowsSettings.EditSongWindow;

        if (WindowState == WindowState.Maximized)
        {
            editSongWindowSettings.Width = (int)RestoreBounds.Width;
            editSongWindowSettings.Height = (int)RestoreBounds.Height;
            editSongWindowSettings.Top = (int)RestoreBounds.Top;
            editSongWindowSettings.Left = (int)RestoreBounds.Left;
            editSongWindowSettings.Maximized = true;
        }

        else
        {
            editSongWindowSettings.Width = (int)Width;
            editSongWindowSettings.Height = (int)Height;
            editSongWindowSettings.Top = (int)Top;
            editSongWindowSettings.Left = (int)Left;
            editSongWindowSettings.Maximized = false;
        }

        configuration.GetSection("ApplicationConfiguration").Bind(applicationConfiguration);

        var applicationConfigurationJson = JsonSerializer.Serialize(applicationConfiguration, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("ApplicationConfiguration.json", applicationConfigurationJson);
    }
}
