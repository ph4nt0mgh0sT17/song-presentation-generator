using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class DeleteSongRequest
{
    public string SongTitle { get; }

    public DeleteSongRequest(string? songTitle)
    {
        Guard.IsNotNull(songTitle);
        SongTitle = songTitle;
    }
}