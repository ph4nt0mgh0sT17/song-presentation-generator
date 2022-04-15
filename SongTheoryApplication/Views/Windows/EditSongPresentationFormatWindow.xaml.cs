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
        var editSongPresentationFormatViewModel = new EditSongPresentationFormatViewModel(
            songTitle, this, onSaveFormat, slides
        );

        DataContext = editSongPresentationFormatViewModel;
    }
}