using System;
using System.Printing;
using System.Windows;
using System.Windows.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class CreateSongWindowViewModel : BaseViewModel
{
    private string? _songTitle;
    private string? _songInterpret;
    private string? _songText;
    private bool _canCreateSong;

    private readonly ISongService _songService;
    private readonly CreateSongWindow? _createSongWindow;

    public string CreateSongWindowTitleText => "Formulář pro vytvoření písničky";

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

    public string? SongInterpret
    {
        get => _songInterpret;

        set
        {
            _songInterpret = value;
            CanCreateSong = CheckCanCreateSong();
            RaisePropertyChanged(nameof(SongInterpret));
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


    public ICommand CreateSongCommand => new RelayCommand(CreateSong);

    public bool CanCreateSong
    {
        get => _canCreateSong;

        set
        {
            _canCreateSong = value;
            RaisePropertyChanged(nameof(CanCreateSong));
        }
    }

    public CreateSongWindowViewModel()
        : this(new SongService(), null)
    {
    }

    public CreateSongWindowViewModel(CreateSongWindow? createSongWindow)
        : this(new SongService(), createSongWindow)
    {
    }

    public CreateSongWindowViewModel(ISongService songService, CreateSongWindow? createSongWindow)
    {
        _songService = songService;
        _createSongWindow = createSongWindow;
    }


    private void CreateSong()
    {
        if (!CheckCanCreateSong()) return;

        var createSongRequest = new CreateSongRequest
        {
            SongTitle = SongTitle,
            SongInterpret = SongInterpret,
            SongText = SongText
        };

        try
        {
            _songService.CreateSong(createSongRequest);
            
            new DialogWindow(
                    "Písnička byla úspěšně vytvořena!",
                    "Písnička byla úspěšně vytvořena a uložena do systému.",
                    DialogButtons.OK,
                    DialogIcons.SUCCESS)
                .ShowDialog();
            
            if (_createSongWindow == null)
                throw new InvalidOperationException("Cannot close the window because its reference is null.");

            _createSongWindow.Close();
        }
        catch (InvalidOperationException)
        {
            // TODO: Replace MessageBox.Show by some dialog window
            MessageBox.Show("The song could not be created due to various reasons.");
        }
    }

    private bool CheckCanCreateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongInterpret) &&
               !string.IsNullOrEmpty(SongText);
    }
}