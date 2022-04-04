using System;
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


    [Fact(DisplayName = "CreateSong() should throw ThrowSongCannotBeCreatedException when song cannot be saved")]
    public void CreateSong_ShouldThrowSongCannotBeCreatedException_WhenSongCannotBeCreated()
    {
        var songRequest = new CreateSongRequest("Song", "Song text");

        // Assert
        _localSongRepository.Setup(x => x.SaveSong(It.IsAny<Song>()))
            .Throws<InvalidOperationException>();

        // Act
        var act = () => _songService.CreateSong(songRequest);

        // Assert
        act.Should().Throw<SongCannotBeCreatedException>();
    }
}