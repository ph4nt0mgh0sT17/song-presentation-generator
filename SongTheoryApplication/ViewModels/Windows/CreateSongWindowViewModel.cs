using System;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class CreateSongWindowViewModel : BaseViewModel
{
    private string? _songTitle;
    private string? _songText;
    private bool _canCreateSong;
    private bool _presentationIsBeingGenerated;
    private bool _windowIsIdle;

    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;

    public string CreateSongWindowTitleText => "Formulář pro vytvoření písničky";
    public IAsyncRelayCommand CreateSongCommand { get; }

    public CreateSongWindowViewModel(
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService)
    {
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        CreateSongCommand = new AsyncRelayCommand(CreateSong);
        WindowIsIdle = true;
    }

    public string? SongTitle
    {
        get => _songTitle;

        set
        {
            SetProperty(ref _songTitle, value);
            CanCreateSong = CheckCanCreateSong();
        }
    }

    public string? SongText
    {
        get => _songText;

        set
        {
            SetProperty(ref _songText, value);
            CanCreateSong = CheckCanCreateSong();
        }
    }

    public bool CanCreateSong
    {
        get => _canCreateSong;

        set => SetProperty(ref _canCreateSong, value);
    }

    public bool PresentationIsBeingGenerated
    {
        get => _presentationIsBeingGenerated;

        set
        {
            SetProperty(ref _presentationIsBeingGenerated, value);
            WindowIsIdle = !PresentationIsBeingGenerated;
        }
    }
    
    public bool WindowIsIdle
    {
        get => _windowIsIdle;

        set => SetProperty(ref _windowIsIdle, value);
    }

    public CreateSongWindow? CreateSongWindow { get; set; }

    private async Task CreateSong()
    {
        if (!CheckCanCreateSong()) return;

        var createSongRequest = new CreateSongRequest(SongTitle, SongText);

        await Task.Run(() =>
        {
            _songService.CreateSong(createSongRequest);
        });
        
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
                    _presentationGeneratorService.GenerateTestingPresentation(
                        createSongRequest.SongTitle,
                        createSongRequest.SongText,
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

    private bool CheckCanCreateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongText);
    }
}