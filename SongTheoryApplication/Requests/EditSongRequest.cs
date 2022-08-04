using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class EditSongRequest
{
    public EditSongRequest(string? id, string? songTitle, string? songText, string? songSource, bool isSongShared = false, string? sharedSongId = null, bool isSongDownloaded = false)
    {
        Guard.IsNotNullOrEmpty(id);
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);
        Guard.IsNotNullOrEmpty(songSource);

        Id = id;
        SongTitle = songTitle;
        SongText = songText;
        SongSource = songSource;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
        IsSongDownloaded = isSongDownloaded;
    }

    public string Id { get; }
    public string SongTitle { get; }
    public string SongText { get; }
    public string SongSource { get; }
    public bool IsSongShared { get; }
    public string? SharedSongId { get; }
    public bool IsSongDownloaded { get; }
}