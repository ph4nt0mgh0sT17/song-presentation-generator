using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class CreateSongWindow : Window
{
    public CreateSongWindow()
    {
        InitializeComponent();
        var createSongWindowViewModel = App.Current.Services.GetService<CreateSongWindowViewModel>();
        //createSongWindowViewModel.CreateSongWindow = this;
        
        DataContext = createSongWindowViewModel;

    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
            MessageBox.Show(button.IsEnabled.ToString());
    }
}