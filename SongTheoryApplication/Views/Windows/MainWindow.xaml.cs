using System.Windows;
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
        DataContext = App.Current.Services.GetService(typeof(MainWindowViewModel));
    }
}