using CommunityToolkit.Diagnostics;
using System;

namespace SongTheoryApplication.Models;

public class ShareSong
{
    public ShareSong()
    {
    }

    public ShareSong(string? title, string? text, string? source, string? tags)
    {
        Guard.IsNotNullOrEmpty(title, nameof(title));
        Guard.IsNotNullOrEmpty(text, nameof(text));

        Title = title;
        Text = text;
        Source = source;
        Tags = tags;
    }

    public string Title { get; set; }
    public string Text { get; set; }
    public string? Source { get; set; }
    public string? Tags { get; set; }
}