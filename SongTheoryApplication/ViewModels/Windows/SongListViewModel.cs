using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class SongListViewModel : BaseViewModel
{
    [ObservableProperty] private string? titleName;
    [ObservableProperty] private ObservableCollection<Song> _songs = new();
    [ObservableProperty] private bool _songsAreLoading;

    private readonly ILocalSongRepository _localSongRepository;

    public SongListViewModel(ILocalSongRepository localSongRepository)
    {
        _localSongRepository = localSongRepository;
    }

    [ICommand]
    private async Task OnLoadedAsync()
    {
        SongsAreLoading = true;
        var songs = await new LocalSongRepository().RetrieveAllSongsAsync();

        Songs = new ObservableCollection<Song>(songs);
        SongsAreLoading = false;
    }

    [ICommand]
    private async Task DeleteSong(Song song)
    {
        var dialogWindow = new DialogWindow(
            "Požadavek o smazání písničky",
            $"Doopravdy chcete smazat písničku '{song.Title}'?",
            DialogButtons.ACCEPT_CANCEL,
            DialogIcons.INFORMATION
        );

        dialogWindow.ShowDialog();

        if (dialogWindow.DialogResult.Accept)
        {
            await new LocalSongRepository().DeleteSongAsync(song.Title);
            Songs.Remove(song);
        }
    }
}