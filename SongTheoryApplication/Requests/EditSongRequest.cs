using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class EditSongRequest
{
    public EditSongRequest(string? id, string? songTitle, string? songText, bool isSongShared = false, string? sharedSongId = null)
    {
        Guard.IsNotNullOrEmpty(id);
        Guard.IsNotNullOrEmpty(songTitle);
        Guard.IsNotNullOrEmpty(songText);

        Id = id;
        SongTitle = songTitle;
        SongText = songText;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
    }

    public string Id { get; }
    public string SongTitle { get; }
    public string SongText { get; }
    public bool IsSongShared { get; }
    public string? SharedSongId { get; }
}