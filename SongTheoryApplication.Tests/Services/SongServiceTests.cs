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

    [Fact(DisplayName = "CreateSongAsync() should throw ArgumentNullException when request is null")]
    public async Task CreateSongAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act
        var act = async () => await _songService.CreateSongAsync(null);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "CreateSongAsync() should throw SongAlreadyExistsException when song already exists")]
    public async Task CreateSongAsync_ShouldThrowSongAlreadyExistsException_WhenSongAlreadyExists()
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

    [Fact(DisplayName = "CreateSongAsync() should throw SongCannotBeCreatedException when song cannot be saved")]
    public async Task CreateSongAsync_ShouldThrowSongCannotBeCreatedException_WhenSongCannotBeCreated()
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

    [Fact(DisplayName = "CreateSongAsync() passes correctly")]
    public async Task CreateSongAsync_PassesCorrectly()
    {
        // Assert
        var createSongRequest = new CreateSongRequest("Song title", "Song text");

        _localSongRepository.Setup(x => x.RetrieveAllSongsAsync())
            .Returns(Task.FromResult(new List<Song> { new("Song 215125", "Song text") }));

        // Act
        await _songService.CreateSongAsync(createSongRequest);

        // Assert
        _localSongRepository.Verify(
            x => x.SaveSongAsync(It.IsAny<Song>()), 
            Times.AtLeastOnce
        );
    }

    [Fact(DisplayName = "DeleteSongAsync() should throw ArgumentNullException when request is null")]
    public async Task DeleteSongAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act
        var act = async () => await _songService.DeleteSongAsync(null);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "DeleteSongAsync() passes correctly")]
    public async Task DeleteSongAsync_PassesCorrectly()
    {
        // Assert
        var deleteSongRequest = new DeleteSongRequest("Song title");

        // Act
        await _songService.DeleteSongAsync(deleteSongRequest);

        // Assert
        _localSongRepository.Verify(
            x => x.DeleteSongAsync(It.Is("Song title", EqualityComparer<string>.Default)), 
            Times.AtLeastOnce
        );
    }
}