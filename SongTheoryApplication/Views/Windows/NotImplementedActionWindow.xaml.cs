using System.Windows;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class NotImplementedActionWindow : Window
{
    public NotImplementedActionWindow()
    {
        InitializeComponent();
        var notImplementedActionWindowViewModel = new NotImplementedActionWindowViewModel
        {
            Width = 300,
            Height = 200
        };
        
        notImplementedActionWindowViewModel.CenterScreen();

        DataContext = notImplementedActionWindowViewModel;
    }
}