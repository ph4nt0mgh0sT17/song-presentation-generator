using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Services;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.ViewModels.Dialogs;

[ViewModel]
public partial class ShareSongDialogViewModel : ObservableObject
{
    private readonly IShareService _shareService;
    private readonly ISongService _songService;
    private readonly Song _song;

    [ObservableProperty] private string _sharedSongId = "";
    [ObservableProperty] private bool _isSongShared = false;
    [ObservableProperty] private bool _sharedSongIdIsOccupied = false;

    public ShareSongDialogViewModel(
        IShareService shareService,
        ISongService songService,
        Song? song)
    {
        _shareService = shareService;
        _songService = songService;
        Guard.IsNotNull(song);
        _song = song;
    }

    [ICommand]
    public async Task ShareSong()
    {
        IsSongShared = false;
        SharedSongIdIsOccupied = false;

        try
        {
            await _shareService.ShareSong(new ShareSongRequest(_sharedSongId, _song.Title, _song.Text, _song.Source, _song.Tags));
        }
        catch (SharedSongAlreadyExistsException)
        {
            SharedSongIdIsOccupied = true;
            return;
        }

        try
        {
            await _songService.UpdateSongAsync(new EditSongRequest(_song.Id, _song.Title, _song.Text, _song.Source, _song.Tags,
                true, _sharedSongId, _song.IsSongDownloaded, _song.CopySongId));
        }
        catch (Exception ex)
        {

        }

        IsSongShared = true;
    }
}