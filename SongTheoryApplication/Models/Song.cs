using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Models;

public class Song
{
    public Song()
    {
        Id = Guid.NewGuid().ToString();
        Title = "Title";
        Text = "Text";
    }

    public Song(string? title, string? text, bool isSongShared = false, string? sharedSongId = null, 
        bool isSongDownloaded = false)
    {
        Guard.IsNotNullOrEmpty(title, nameof(title));
        Guard.IsNotNullOrEmpty(text, nameof(text));

        Id = Guid.NewGuid().ToString();
        Title = title;
        Text = text;
        IsSongShared = isSongShared;
        SharedSongId = sharedSongId;
        IsSongDownloaded = isSongDownloaded;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsSongShared { get; set; }
    public string? SharedSongId { get; set; }
    public bool IsSongDownloaded { get; set; }
}