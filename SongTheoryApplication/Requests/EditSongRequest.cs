using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class EditSongRequest
{
    public EditSongRequest(
        string? id, string? songTitle, string? songText, string? songSource, string? songTags,
        bool isSongShared = false, string? sharedSongId = null, bool isSongDownloaded = false,
        string? copySongId = null)
    {
        Guard.IsNotNullOrEmpty(id);
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);

        Id = id;
        SongTitle = songTitle;
        SongText = songText;
        SongSource = songSource;
        SongTags = songTags;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
        IsSongDownloaded = isSongDownloaded;
        CopySongId = copySongId;
    }

    public string Id { get; }
    public string SongTitle { get; }
    public string SongText { get; }
    public string? SongSource { get; }
    public string? SongTags { get; }
    public bool IsSongShared { get; }
    public string? SharedSongId { get; }
    public bool IsSongDownloaded { get; }
    public string? CopySongId { get; }
}