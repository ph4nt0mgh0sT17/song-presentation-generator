using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Models;

public class Song
{
    public Song()
    {
        Title = "Title";
        Text = "Text";
    }

    public Song(string title, string text)
    {
        Guard.IsNotNullOrEmpty(title, nameof(title));
        Guard.IsNotNullOrEmpty(text, nameof(text));

        Title = title;
        Text = text;
    }

    public string Title { get; set; }
    public string Text { get; set; }
}