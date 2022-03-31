using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class CreateSongRequest
{
    public string SongTitle { get; }
    public string SongText { get; }

    public CreateSongRequest(string? songTitle, string? songText)
    {
        Guard.IsNotNullOrEmpty(songTitle, nameof(songTitle));
        Guard.IsNotNullOrEmpty(songText, nameof(songText));
        
        SongTitle = songTitle;
        SongText = songText;
    }
}