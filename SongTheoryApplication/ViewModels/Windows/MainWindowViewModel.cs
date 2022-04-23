﻿using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class MainWindowViewModel : BaseViewModel
{
    public string MainWindowTitleText =>
        "Aplikace pro evidenci písniček a jejich vygenerování do PowerPoint prezentace";

    public bool IsConfigurationNotFound => true;
    
    public SnackbarMessageQueue BoundMessageQueue { get; } = new();

    public ICommand OpenCreateSongWindowCommand => new RelayCommand(OpenCreateSongWindow);
    public ICommand OpenSongListWindowCommand => new RelayCommand(OpenSongListWindow);

    public MainWindowViewModel(IConfiguration configuration)
    {
        if (configuration is NullConfiguration)
        {
            BoundMessageQueue.Enqueue(
                "Konfigurace aplikace nenalezena. Prezentace budou vytvořeny v defaultním nastavení.",
                "Zavřít",
                _ => { },
                null,
                false,
                true,
                TimeSpan.FromSeconds(60)
            );
        }
    }

    private void OpenCreateSongWindow()
    {
        new CreateSongWindow().ShowDialog();
    }

    private void OpenSongListWindow()
    {
        new SongListWindow().ShowDialog();
    }
}