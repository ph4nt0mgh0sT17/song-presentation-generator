using System;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class SongListWindow : Window
{
    public SongListWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService<SongListViewModel>();
    }

    public SongListViewModel ViewModel
    {
        get
        {
            if (DataContext is SongListViewModel songListViewModel)
                return songListViewModel;

            throw new InvalidOperationException("The DataContext of this window must be SongListViewModel.");
        }
    }
}