using System;
using FluentAssertions;
using SongTheoryApplication.Models;
using Xunit;

namespace SongTheoryApplication.Tests.Models;

public class SongTests
{
    [Theory(DisplayName = "Call constructor throws ArgumentNullException when parameters are null")]
    [InlineData(null, null)]
    [InlineData("Title", null)]
    public void Constructor_ThrowsArgumentNullException_WhenParametersAreNull(string? songTitle, string? songText)
    {
        // Arrange
        var act = () => new Song(songTitle, songText);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory(DisplayName = "Call constructor throws ArgumentException when parameters are illegal")]
    [InlineData("", null)]
    [InlineData("Title", "")]
    public void Constructor_ThrowsArgumentException_WhenParametersAreIllegal(string? songTitle, string? songText)
    {
        // Arrange
        var act = () => new Song(songTitle, songText);

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