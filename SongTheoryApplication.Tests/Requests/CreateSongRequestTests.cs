using System;
using FluentAssertions;
using SongTheoryApplication.Requests;
using Xunit;

namespace SongTheoryApplication.Tests.Requests;

public class CreateSongRequestTests
{
    [Theory(DisplayName = "Call constructor throws ArgumentNullException when parameters are null")]
    [InlineData(null, null)]
    [InlineData("Title", null)]
    public void Constructor_ThrowsArgumentNullException_WhenParametersAreNull(string? songTitle, string? songText)
    {
        // Arrange
        var act = () => new CreateSongRequest(songTitle, songText);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory(DisplayName = "Call constructor throws ArgumentException when parameters are illegal")]
    [InlineData("", null)]
    [InlineData("Title", "")]
    public void Constructor_ThrowsArgumentException_WhenParametersAreIllegal(string? songTitle, string? songText)
    {
        // Arrange
        var act = () => new CreateSongRequest(songTitle, songText);

        // Act + Assert
        act.Should().Throw<ArgumentException>();
    }
}