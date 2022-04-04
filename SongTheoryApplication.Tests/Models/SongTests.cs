using System;
using FluentAssertions;
using SongTheoryApplication.Models;
using Xunit;

namespace SongTheoryApplication.Tests.Models;

public class SongTests
{
    [Fact(DisplayName = "Call constructor throws ArgumentNullException when null song title is supplied")]
    public void Constructor_ThrowsArgumentNullException_WhenSongTitleIsNull()
    {
        // Arrange
        var act = () => new Song(null, null);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Call constructor throws ArgumentException when empty song title is supplied")]
    public void Constructor_ThrowsArgumentException_WhenSongTitleIsEmpty()
    {
        // Arrange
        var act = () => new Song("", null);

        // Act + Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Call constructor throws ArgumentNullException when null song text is supplied")]
    public void Constructor_ThrowsArgumentNullException_WhenSongTextIsNull()
    {
        // Arrange
        var act = () => new Song("Song title", null);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Call constructor throws ArgumentException when empty song text is supplied")]
    public void Constructor_ThrowsArgumentException_WhenSongTextIsEmpty()
    {
        // Arrange
        var act = () => new Song("Song title", "");

        // Act + Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Call constructor passes when all arguments are not empty and not null")]
    public void Constructor_Passes_WhenAllArgumentsAreValid()
    {
        // Arrange
        var act = () => new Song("Song title", "Song text");

        // Act + Assert
        act.Should().NotThrow();
    }
}