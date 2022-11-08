using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class ShareSongRequest
{
    public ShareSongRequest(string? shareSongId, string? songTitle, string? songText, string? songSource, string? songTags)
    {
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);
        Guard.IsNotNull(shareSongId);

        ShareSongId = shareSongId;
        SongTitle = songTitle;
        SongText = songText;
        SongSource = songSource;
        SongTags = songTags;
    }

    public string ShareSongId { get; set; }
    public string SongTitle { get; }
    public string SongText { get; }
    public string? SongSource { get; }
    public string? SongTags { get; }
}