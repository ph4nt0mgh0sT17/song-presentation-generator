using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SongTheoryApplication.Models;
using SongTheoryApplication.Providers;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Windows;
using Xunit;

namespace SongTheoryApplication.Tests.ViewModels.Windows;

public class SongListViewModelTests
{
    private readonly Mock<ILocalSongRepository> _localSongRepository = new();
    private readonly Mock<ISongService> _songService = new();
    private readonly Mock<IPresentationGeneratorService> _presentationGeneratorService = new();
    private readonly Mock<ISaveFileDialogProvider> _saveFileDialogProvider = new();
    private readonly Mock<IDialogHostService> _dialogHostService = new();
    private readonly Mock<ILogger<SongListViewModel>> _logger = new();
    private readonly SongListViewModel _songListViewModel;

    public SongListViewModelTests()
    {
        _songListViewModel = new SongListViewModel(
            _localSongRepository.Object,
            _songService.Object,
            _presentationGeneratorService.Object,
            _saveFileDialogProvider.Object,
            _dialogHostService.Object,
            _logger.Object
        );
    }

    [Fact(DisplayName = "OnLoadedAsync() passes correctly")]
    public async Task OnLoadedAsync_PassesCorrectly()
    {
        // Arrange
        _localSongRepository.Setup(x => x.RetrieveAllSongsAsync())
            .Returns(Task.FromResult(new List<Song> { new("Song title", "Song text") }));

        // Act
        await _songListViewModel.OnLoadedCommand.ExecuteAsync(null);

        // Assert
        _localSongRepository.Verify(x => x.RetrieveAllSongsAsync(), Times.AtLeastOnce);

        _songListViewModel.SongsAreLoading.Should().BeFalse("The loading is completed.");
        _songListViewModel.Songs.Should().HaveCount(1, "Only 1 song is in the LocalSongRepository.");
    }

    [Fact(DisplayName = "DeleteSong() opens dialog")]
    public async Task DeleteSong_OpensDialog()
    {
        // Arrange
        _dialogHostService.Setup(x => x.OpenDialog(It.IsAny<object?>(), It.IsAny<string?>()))
            .Returns(Task.FromResult((object?) false));

        // Act
        await _songListViewModel.DeleteSongCommand.ExecuteAsync(new Song("Title", "Text"));

        // Assert
        _dialogHostService.Verify(
            x => x.OpenDialog(It.IsAny<object?>(), It.IsAny<string?>()),
            Times.Once
        );
    }

    [Fact(DisplayName = "DeleteSong() deletes a song when dialog is confirmed")]
    public async Task DeleteSong_DeletesSong_WhenDialogIsConfirmed()
    {
        // Arrange
        var song = new Song("Title", "Text");
        _songListViewModel.Songs = new ObservableCollection<Song>(new List<Song>
        {
            song
        });

        _dialogHostService.Setup(x => x.OpenDialog(It.IsAny<object?>(), It.IsAny<string?>()))
            .Returns(Task.FromResult((object?) true));

        // Act
        await _songListViewModel.DeleteSongCommand.ExecuteAsync(song);

        // Assert
        _dialogHostService.Verify(
            x => x.OpenDialog(It.IsAny<object?>(), It.IsAny<string?>()),
            Times.Once
        );

        _songService.Verify(x => x.DeleteSongAsync(It.IsAny<DeleteSongRequest>()), Times.Once);
        _songListViewModel.Songs.Should().BeEmpty("The only song that was present is now deleted.");
    }
}