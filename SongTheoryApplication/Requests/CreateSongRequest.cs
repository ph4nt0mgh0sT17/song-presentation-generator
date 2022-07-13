using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Requests;

public class CreateSongRequest
{
    public CreateSongRequest(string songTitle, string songText,bool isSongShared = false, string? sharedSongId = null)
    {
        Guard.IsNotNullOrEmpty(songTitle, nameof(songTitle));
        Guard.IsNotNullOrEmpty(songText, nameof(songText));

        SongTitle = songTitle;
        SongText = songText;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
    }

    public string SongTitle { get; }
    public string SongText { get; }
    public bool IsSongShared { get; }
    public string? SharedSongId { get; }
}