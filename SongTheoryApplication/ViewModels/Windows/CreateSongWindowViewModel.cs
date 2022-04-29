using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class CreateSongWindowViewModel : BaseViewModel
{
    [ObservableProperty] [AlsoNotifyChangeFor(nameof(CanCreateSong))]
    private string? _songTitle;

    [ObservableProperty] [AlsoNotifyChangeFor(nameof(CanCreateSong))]
    private string? _songText;

    [ObservableProperty]
    private bool _presentationIsBeingGenerated;

    [ObservableProperty]
    private bool _presentationFormatIsCreated;

    private List<PresentationSlideDetail>? _slides;

    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;

    public string CreateSongWindowTitleText => "Formulář pro vytvoření písničky";
    public IAsyncRelayCommand CreateSongCommand { get; }
    public IRelayCommand CreateSongPresentationFormatCommand { get; }
    public IRelayCommand EditSongPresentationFormatCommand { get; }

    public bool CanCreateSong => CheckCanCreateSong();

    public CreateSongWindow? CreateSongWindow { get; set; }

    private List<PresentationSlideDetail>? Slides
    {
        get => _slides;
        set
        {
            _slides = value;
            PresentationFormatIsCreated = true;
        }
    }


    public CreateSongWindowViewModel(
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService)
    {
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        CreateSongCommand = new AsyncRelayCommand(CreateSong, () => CanCreateSong);
        CreateSongPresentationFormatCommand = new RelayCommand(CreateSongPresentationFormat, () => CanCreateSong);
        EditSongPresentationFormatCommand = new RelayCommand(
            EditSongPresentationFormat,
            () => PresentationFormatIsCreated
        );
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(SongTitle):
            case nameof(SongText):
                CreateSongCommand.NotifyCanExecuteChanged();
                CreateSongPresentationFormatCommand.NotifyCanExecuteChanged();
                break;
            case nameof(PresentationFormatIsCreated):
                EditSongPresentationFormatCommand.NotifyCanExecuteChanged();
                break;
        }
    }

    private async Task CreateSong()
    {
        if (!CheckCanCreateSong()) return;

        var createSongRequest = new CreateSongRequest(SongTitle, SongText, _slides);

        await Task.Run(() => { _songService.CreateSong(createSongRequest); });

        var dialog = new DialogWindow(
            "Písnička byla úspěšně vytvořena!",
            "Písnička byla úspěšně vytvořena a uložena do systému. " +
            "Přejete si taktéž vygenerovat i testovací prezentaci?",
            DialogButtons.ACCEPT_CANCEL,
            DialogIcons.SUCCESS
        );

        dialog.ShowDialog();

        if (dialog.DialogResult is { Accept: true })
        {
            var saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                PresentationIsBeingGenerated = true;
                var fileName = saveFileDialog.FileName;

                await Task.Run(() =>
                {
                    if (_slides == null)
                        _presentationGeneratorService.GenerateTestingPresentation(
                            createSongRequest.SongTitle,
                            createSongRequest.SongText,
                            fileName
                        );
                    else
                        _presentationGeneratorService.GeneratePresentation(
                            new Song(SongTitle, SongText, _slides),
                            fileName
                        );
                });
            }
        }


        if (CreateSongWindow == null)
        {
            DialogWindow.ShowDialog(
                "Toto dialogové okno nemohlo být uzavřeno.",
                "Toto dialogové okno nemohlo být uzavřeno.",
                DialogButtons.OK,
                DialogIcons.ERROR
            );
        }

        else
        {
            CreateSongWindow.Close();
        }
    }

    private void CreateSongPresentationFormat()
    {
        var createSongPresentationFormatWindow = new CreateSongPresentationFormatWindow(
            SongTitle, SongText,
            slides => Slides = slides
        );

        createSongPresentationFormatWindow.ShowDialog();
    }

    private void EditSongPresentationFormat()
    {
        Guard.IsNotNull(Slides, nameof(Slides));
        Guard.IsNotNull(SongTitle, nameof(SongTitle));
        Guard.IsNotNull(SongText, nameof(SongText));

        var editSongPresentationFormatWindow = new EditSongPresentationFormatWindow(
            SongTitle, SongText, new List<PresentationSlideDetail>(Slides),
            slides => Slides = slides
        );

        editSongPresentationFormatWindow.ShowDialog();
    }

    private bool CheckCanCreateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongText);
    }
}