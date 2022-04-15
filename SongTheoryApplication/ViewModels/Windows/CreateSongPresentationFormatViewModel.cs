﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using NetOffice.PowerPointApi;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class CreateSongPresentationFormatViewModel : BaseViewModel
{
    private int _currentPresentationSlideNumber;
    private PresentationSlideDetail _currentPresentationSlide;
    public CreateSongPresentationFormatWindow? CreateSongPresentationFormatWindow { get; set; }
    public string? SongTitle { get; set; }
    public string? SongText { get; set; }

    public int CurrentPresentationSlideNumber
    {
        get => _currentPresentationSlideNumber;
        private set
        {
            SetProperty(ref _currentPresentationSlideNumber, value);
            OnGoToNextSlideCommand?.NotifyCanExecuteChanged();
            OnGoToPreviousSlideCommand?.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<PresentationSlideDetail> PresentationSlides { get; }

    public PresentationSlideDetail CurrentPresentationSlide
    {
        get => _currentPresentationSlide;
        private set
        {
            SetProperty(ref _currentPresentationSlide, value);
            var slideNumber = PresentationSlides.IndexOf(value);
            CurrentPresentationSlideNumber = slideNumber + 1;
        }
    }

    public List<PresentationFormatStyle> PresentationFormatStyles { get; } = new()
    {
        new PresentationFormatStyle("Centered text")
    };
    
    public IRelayCommand OnAddNewSlideCommand { get; }
    public IRelayCommand OnGoToPreviousSlideCommand { get; }
    public IRelayCommand OnGoToNextSlideCommand { get; }
    public IRelayCommand<List<PresentationSlideDetail>>? OnSavePresentationFormatCommand { get; set; }
    public IRelayCommand OnLocalSavePresentationFormatCommand { get; }

    public CreateSongPresentationFormatViewModel()
    {
        PresentationSlides = new ObservableCollection<PresentationSlideDetail>
        {
            new(PresentationFormatStyles.First(), SongText ?? "Text písničky")
        };

        CurrentPresentationSlide = PresentationSlides.First();
        CurrentPresentationSlideNumber = 1;
        OnAddNewSlideCommand = new RelayCommand(AddNewSlide);
        OnGoToPreviousSlideCommand = new RelayCommand(GoToPreviousSlide, () => CurrentPresentationSlideNumber > 1);
        OnGoToNextSlideCommand = new RelayCommand(GoToNextSlide, () => CurrentPresentationSlideNumber < PresentationSlides.Count);
        OnLocalSavePresentationFormatCommand = new RelayCommand(SavePresentationFormat);
    }

    private void AddNewSlide()
    {
        var newPresentationSlide = new PresentationSlideDetail(PresentationFormatStyles.First(), "Text písničky zde");
        PresentationSlides.Add(newPresentationSlide);
        ChangePresentationSlide(newPresentationSlide);
    }
    
    private void ChangePresentationSlide(PresentationSlideDetail presentationSlide)
    {
        CurrentPresentationSlide = presentationSlide;
    }

    private void GoToPreviousSlide()
    {
        CurrentPresentationSlide = PresentationSlides.ElementAt(_currentPresentationSlideNumber - 2);
    }
    
    private void GoToNextSlide()
    {
        CurrentPresentationSlide = PresentationSlides.ElementAt(_currentPresentationSlideNumber);
    }

    private void SavePresentationFormat()
    {
        Guard.IsNotNull(OnSavePresentationFormatCommand, nameof(OnSavePresentationFormatCommand));
        OnSavePresentationFormatCommand.Execute(PresentationSlides.ToList());
        
        Guard.IsNotNull(CreateSongPresentationFormatWindow, nameof(CreateSongPresentationFormatWindow));
        CreateSongPresentationFormatWindow.Close();
    }
}