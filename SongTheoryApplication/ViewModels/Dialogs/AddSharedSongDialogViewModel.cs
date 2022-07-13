using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Services;

namespace SongTheoryApplication.ViewModels.Dialogs;

[ViewModel]
public partial class AddSharedSongDialogViewModel : ObservableObject
{
    private readonly IShareService _shareService;
    private readonly ISongService _songService;
    private readonly ILocalSongRepository _localSongRepository;

    [ObservableProperty]
    private string _sharedSongId = "";

    [ObservableProperty]
    private bool _sharedSongIdAlreadyInLocalRepository = false;

    public AddSharedSongDialogViewModel(IShareService shareService, ISongService songService, 
        ILocalSongRepository localSongRepository)
    {
        _shareService = shareService;
        _songService = songService;
        _localSongRepository = localSongRepository;
    }

    [ICommand]
    public async Task AddSharedSong()
    {
        var allSongs = await _localSongRepository.RetrieveAllSongsAsync();

        if (allSongs.Exists(song => song.SharedSongId != null && song.SharedSongId == SharedSongId))
        {
            SharedSongIdAlreadyInLocalRepository = true;
            return;
        }

        await _shareService.AddSharedSong(SharedSongId);

        DialogHost.Close(this);
    }
}