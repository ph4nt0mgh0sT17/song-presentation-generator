using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService(typeof(MainWindowViewModel));
    }
}