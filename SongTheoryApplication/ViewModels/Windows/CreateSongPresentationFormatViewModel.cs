using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetOffice.PowerPointApi;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class CreateSongPresentationFormatViewModel : BaseViewModel
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CurrentPresentationSlideNumber))]
    private PresentationSlideDetail _currentPresentationSlide;

    [ObservableProperty]
    private bool _displayForLowerResolution;
    
    public CreateSongPresentationFormatWindow CreateSongPresentationFormatWindow { get; }
    
    public string Title { get; }

    public int CurrentPresentationSlideNumber => PresentationSlides.IndexOf(CurrentPresentationSlide) + 1;

    public ObservableCollection<PresentationSlideDetail> PresentationSlides { get; }

    public List<PresentationFormatStyle> PresentationFormatStyles { get; } = new()
    {
        new PresentationFormatStyle("Centered text")
    };
    
    public IRelayCommand OnAddNewSlideCommand { get; }
    public IRelayCommand OnGoToPreviousSlideCommand { get; }
    public IRelayCommand OnGoToNextSlideCommand { get; }
    public IRelayCommand<List<PresentationSlideDetail>> OnSavePresentationFormatCommand { get; set; }
    public IRelayCommand OnLocalSavePresentationFormatCommand { get; }

    public CreateSongPresentationFormatViewModel(
        string? songTitle, string? songText, 
        CreateSongPresentationFormatWindow? createSongWindow, 
        Action<List<PresentationSlideDetail>> onSaveFormat)
    {
        Guard.IsNotNull(songTitle, nameof(songTitle));
        Guard.IsNotNull(songText, nameof(songText));
        Guard.IsNotNull(createSongWindow, nameof(createSongWindow));
        Guard.IsNotNull(onSaveFormat, nameof(onSaveFormat));
        
        Title = $"{songTitle} - Vlastní šablona pro formát prezentace písničky";
        CreateSongPresentationFormatWindow = createSongWindow;
        
        PresentationSlides = new ObservableCollection<PresentationSlideDetail>
        {
            new(PresentationFormatStyles.First(), songText)
        };

        CurrentPresentationSlide = PresentationSlides.First();
        OnAddNewSlideCommand = new RelayCommand(AddNewSlide);
        OnGoToPreviousSlideCommand = new RelayCommand(GoToPreviousSlide, () => CurrentPresentationSlideNumber > 1);
        OnGoToNextSlideCommand = new RelayCommand(GoToNextSlide, () => CurrentPresentationSlideNumber < PresentationSlides.Count);
        OnSavePresentationFormatCommand = new RelayCommand<List<PresentationSlideDetail>>(onSaveFormat);
        OnLocalSavePresentationFormatCommand = new RelayCommand(SavePresentationFormat);
    }
    
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(CurrentPresentationSlide):
                OnGoToNextSlideCommand?.NotifyCanExecuteChanged();
                OnGoToPreviousSlideCommand?.NotifyCanExecuteChanged();
                break;
        }
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
        CurrentPresentationSlide = PresentationSlides.ElementAt(CurrentPresentationSlideNumber - 2);
    }
    
    private void GoToNextSlide()
    {
        CurrentPresentationSlide = PresentationSlides.ElementAt(CurrentPresentationSlideNumber);
    }

    private void SavePresentationFormat()
    {
        Guard.IsNotNull(OnSavePresentationFormatCommand, nameof(OnSavePresentationFormatCommand));
        OnSavePresentationFormatCommand.Execute(PresentationSlides.ToList());
        
        Guard.IsNotNull(CreateSongPresentationFormatWindow, nameof(CreateSongPresentationFormatWindow));
        CreateSongPresentationFormatWindow.Close();
    }
}