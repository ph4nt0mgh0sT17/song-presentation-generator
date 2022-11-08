﻿using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Requests;

public class CreateSongRequest
{
    public CreateSongRequest(
        string? songTitle, string? songText, 
        string? source,
        bool isSongShared = false, 
        string? sharedSongId = null,
        bool isSongDownloaded = false)
    {
        Guard.IsNotNullOrEmpty(songTitle, nameof(songTitle));
        Guard.IsNotNullOrEmpty(songText, nameof(songText));

        SongTitle = songTitle;
        SongText = songText;
        SongSource = source;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
        IsSongDownloaded = isSongDownloaded;
    }

    public string SongTitle { get; }
    public string SongText { get; }
    public string? SongSource { get; }
    public bool IsSongShared { get; }
    public string? SharedSongId { get; }
    public bool IsSongDownloaded { get; }
}