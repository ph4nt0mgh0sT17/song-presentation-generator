using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using Xunit;

namespace SongTheoryApplication.Tests.Services;

public class SongServiceTests
{
    private readonly Mock<ILocalSongRepository> _localSongRepository = new();
    private readonly SongService _songService;

    public SongServiceTests()
    {
        _songService = new SongService(_localSongRepository.Object);
    }

    [Fact(DisplayName = "CreateSong() should throw ArgumentNullException when request is null")]
    public async Task CreateSong_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act
        var act = async () => await _songService.CreateSongAsync(null);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "CreateSong() should throw SongAlreadyExistsException when song already exists")]
    public async Task CreateSong_ShouldThrowSongAlreadyExistsException_WhenSongAlreadyExists()
    {
        var songRequest = new CreateSongRequest("Song", "Song text");

        // Assert
        _localSongRepository.Setup(x => x.SaveSongAsync(It.IsAny<Song>()))
            .Throws<InvalidOperationException>();

        _localSongRepository.Setup(x => x.RetrieveAllSongsAsync())
            .Returns(Task.FromResult(new List<Song> { new("Song", "Song text") }));

        // Act
        var act = async () => await _songService.CreateSongAsync(songRequest);

        // Assert
        await act.Should().ThrowAsync<SongAlreadyExistsException>();
    }

    [Fact(DisplayName = "CreateSong() should throw SongCannotBeCreatedException when song cannot be saved")]
    public async Task CreateSong_ShouldThrowSongCannotBeCreatedException_WhenSongCannotBeCreated()
    {
        var songRequest = new CreateSongRequest("Song", "Song text");

        // Assert
        _localSongRepository.Setup(x => x.SaveSongAsync(It.IsAny<Song>()))
            .Throws<InvalidOperationException>();

        _localSongRepository.Setup(x => x.RetrieveAllSongsAsync())
            .Returns(Task.FromResult(new List<Song> { new("Song 215125", "Song text") }));

        // Act
        var act = async () => await _songService.CreateSongAsync(songRequest);

        // Assert
        await act.Should().ThrowAsync<SongCannotBeCreatedException>();
    }

}