using System.Windows;
using System.Windows.Controls;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class CreateSongWindow : Window
{
    public CreateSongWindow()
    {
        InitializeComponent();
        DataContext = new CreateSongWindowViewModel(this);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
            MessageBox.Show(button.IsEnabled.ToString());
    }
}