using System;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
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

    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;

    public string CreateSongWindowTitleText => "Formulář pro vytvoření písničky";
    public ICommand CreateSongCommand => new RelayCommand(CreateSong);
    
    public CreateSongWindowViewModel(
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService)
    {
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
    }

    public string? SongTitle
    {
        get => _songTitle;

        set
        {
            _songTitle = value;
            CanCreateSong = CheckCanCreateSong();
            RaisePropertyChanged(nameof(SongTitle));
        }
    }

    public string? SongText
    {
        get => _songText;

        set
        {
            _songText = value;
            CanCreateSong = CheckCanCreateSong();
            RaisePropertyChanged(nameof(SongText));
        }
    }

    public bool CanCreateSong
    {
        get => _canCreateSong;

        set
        {
            _canCreateSong = value;
            RaisePropertyChanged(nameof(CanCreateSong));
        }
    }
    
    public CreateSongWindow? CreateSongWindow
    {
        get;
        set;
    }

    private void CreateSong()
    {
        if (!CheckCanCreateSong()) return;

        var createSongRequest = new CreateSongRequest(SongTitle, SongText);

        try
        {
            _songService.CreateSong(createSongRequest);

            new DialogWindow(
                    "Písnička byla úspěšně vytvořena!",
                    "Písnička byla úspěšně vytvořena a uložena do systému.",
                    DialogButtons.OK,
                    DialogIcons.SUCCESS)
                .ShowDialog();

            _presentationGeneratorService.GenerateTestingPresentation(createSongRequest.SongTitle,
                createSongRequest.SongText);

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
        catch (InvalidOperationException)
        {
            DialogWindow.ShowDialog(
                "Písnička nemohla být z neznámých důvodů vytvořena.",
                "Písnička nemohla být z neznámých důvodů vytvořena.",
                DialogButtons.OK,
                DialogIcons.ERROR
            );
        }
    }

    private bool CheckCanCreateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongText);
    }
}