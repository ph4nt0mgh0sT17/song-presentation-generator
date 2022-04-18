using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.DependencyInjection;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class CreateSongWindow : Window
{
    public CreateSongWindow()
    {
        InitializeComponent();
        var createSongWindowViewModel = GetViewModel();

        DataContext = createSongWindowViewModel;
    }

    private CreateSongWindowViewModel GetViewModel()
    {
        var createSongWindowViewModel = Ioc.Default.GetService<CreateSongWindowViewModel>();
        Guard.IsNotNull(createSongWindowViewModel, nameof(createSongWindowViewModel));
        createSongWindowViewModel.CreateSongWindow = this;
        return createSongWindowViewModel;
    }
}