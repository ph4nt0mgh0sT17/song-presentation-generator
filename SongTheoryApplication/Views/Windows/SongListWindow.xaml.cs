using System.Windows;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class SongListWindow : Window
{
    public SongListWindow()
    {
        InitializeComponent();
        DataContext = new SongListViewModel();
    }
}