using CommunityToolkit.Diagnostics;
using System;

namespace SongTheoryApplication.Models;

public class ShareSong
{
    public ShareSong()
    {
    }

    public ShareSong(string? title, string? text)
    {
        Guard.IsNotNullOrEmpty(title, nameof(title));
        Guard.IsNotNullOrEmpty(text, nameof(text));

        Title = title;
        Text = text;
    }

    public string Title { get; set; }
    public string Text { get; set; }
}