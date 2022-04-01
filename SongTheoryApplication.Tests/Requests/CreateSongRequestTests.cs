using System;
using FluentAssertions;
using SongTheoryApplication.Requests;
using Xunit;

namespace SongTheoryApplication.Tests.Requests;

public class CreateSongRequestTests
{
    [Fact(DisplayName = "Call constructor throws ArgumentNullException when null song title is supplied")]
    public void Constructor_ThrowsArgumentNullException_WhenSongTitleIsNull()
    {
        // Arrange
        var act = () => new CreateSongRequest(null, null);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact(DisplayName = "Call constructor throws ArgumentException when empty song title is supplied")]
    public void Constructor_ThrowsArgumentException_WhenSongTitleIsEmpty()
    {
        // Arrange
        var act = () => new CreateSongRequest("", null);

        // Act + Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact(DisplayName = "Call constructor throws ArgumentNullException when null song text is supplied")]
    public void Constructor_ThrowsArgumentNullException_WhenSongTextIsNull()
    {
        // Arrange
        var act = () => new CreateSongRequest("Song title", null);

        // Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact(DisplayName = "Call constructor throws ArgumentException when empty song text is supplied")]
    public void Constructor_ThrowsArgumentException_WhenSongTextIsEmpty()
    {
        // Arrange
        var act = () => new CreateSongRequest("Song title", "");

        // Act + Assert
        act.Should().Throw<ArgumentException>();
    }
}