using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Extensions.Logging;
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
            this
        );

        DataContext = editSongWindowViewModel;
    }
}
