using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Models;

public class Song
{
    public Song(string title, string text, List<PresentationSlideDetail>? slides)
    {
        Guard.IsNotNullOrEmpty(title, nameof(title));
        Guard.IsNotNullOrEmpty(text, nameof(text));
        
        Title = title;
        Text = text;
        Slides = slides;
    }

    public string Title { get; }
    public string Text { get; }

    public List<PresentationSlideDetail>? Slides { get; }
}