﻿using System;
using System.Collections.Generic;
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
    private readonly SongService _songService;
    private readonly Mock<ILocalSongRepository> _localSongRepository = new();

    public SongServiceTests()
    {
        _songService = new SongService(_localSongRepository.Object);
    }


    [Fact(DisplayName = "CreateSong() should throw null argument when request is null")]
    public void CreateSong_ShouldThrowOperationException_WhenSongCannotBeCreated()
    {
        var songRequest = new CreateSongRequest("Song", "Song text");
        
        // Assert
        _localSongRepository.Setup(x => x.CreateSong(It.IsAny<Song>()))
            .Throws<InvalidOperationException>();

        // Act
        var act = () => _songService.CreateSong(songRequest);

        // Assert
        act.Should().Throw<SongCannotBeCreatedException>();
    }
}