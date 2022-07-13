using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class ShareSongRequest
{
    public ShareSongRequest(string? songTitle, string? songText)
    {
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);

        SongTitle = songTitle;
        SongText = songText;
    }

    public string SongTitle { get; }
    public string SongText { get; }
}