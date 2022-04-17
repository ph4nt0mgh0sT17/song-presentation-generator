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
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.ViewModels.Base;

namespace SongTheoryApplication.ViewModels.Windows;

public partial class SongListViewModel : BaseViewModel
{
    [ObservableProperty] private string? titleName;
    public RangeObservableCollection<Song> Songs { get; } = new();
    [ObservableProperty] private bool _songsAreLoading;

    [ICommand]
    private async Task OnLoadedAsync()
    {
        await Task.Run(async () =>
        {
            var songs = await new LocalSongRepository().RetrieveAllSongsAsync();
            songs = songs.Take(10).ToList();

            Songs.AddRange(songs);
        });
    }
}