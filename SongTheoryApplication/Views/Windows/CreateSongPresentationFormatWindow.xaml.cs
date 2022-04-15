using System;
using System.Collections.Generic;
using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;

public partial class CreateSongPresentationFormatWindow : Window
{
    public CreateSongPresentationFormatWindow(
        string songTitle, string songText,
        Action<List<PresentationSlideDetail>?> onSaveFormat)
    {
        InitializeComponent();
        var createSongPresentationFormatViewModel = new CreateSongPresentationFormatViewModel(
            songTitle, songText, this, onSaveFormat
        );

        DataContext = createSongPresentationFormatViewModel;
    }
}