using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class ShareSongRequest
{
    public ShareSongRequest(string? shareSongId, string? songTitle, string? songText, string? songSource)
    {
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);
        Guard.IsNotNull(shareSongId);
        Guard.IsNotNull(songSource);

        ShareSongId = shareSongId;
        SongTitle = songTitle;
        SongText = songText;
        SongSource = songSource;
    }

    public string ShareSongId { get; set; }
    public string SongTitle { get; }
    public string SongText { get; }
    public string SongSource { get; }
}