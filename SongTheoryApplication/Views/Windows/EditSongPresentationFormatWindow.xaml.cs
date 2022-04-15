using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace SongTheoryApplication.Views.Windows;

public partial class EditSongPresentationFormatWindow : Window
{
    public EditSongPresentationFormatWindow(
        string songTitle, string songText, List<PresentationSlideDetail> slides,
        Action<List<PresentationSlideDetail>?> onSaveFormat)
    {
        InitializeComponent();
        var editSongPresentationFormatViewModel = GetViewModel();
        
        editSongPresentationFormatViewModel.SongTitle = songTitle;
        editSongPresentationFormatViewModel.SongText = songText;
        editSongPresentationFormatViewModel.EditSongPresentationFormatWindow = this;
        editSongPresentationFormatViewModel.OnSavePresentationFormatCommand = new RelayCommand<List<PresentationSlideDetail>>(
            onSaveFormat
        );
        editSongPresentationFormatViewModel.PresentationSlides =
            new ObservableCollection<PresentationSlideDetail>(slides);
        
        editSongPresentationFormatViewModel.CurrentPresentationSlide =
            editSongPresentationFormatViewModel.PresentationSlides.First();
        
        DataContext = editSongPresentationFormatViewModel;
    }

    private EditSongPresentationFormatViewModel GetViewModel()
    {
        var currentViewModel = App.Current.Services.GetService<EditSongPresentationFormatViewModel>();

        Guard.IsNotNull(currentViewModel, nameof(currentViewModel));

        return currentViewModel;
    }
}