using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Requests;

public class CreateSongRequest
{
    public CreateSongRequest(string? songTitle, string? songText, List<PresentationSlideDetail>? slides)
    {
        Guard.IsNotNullOrEmpty(songTitle, nameof(songTitle));
        Guard.IsNotNullOrEmpty(songText, nameof(songText));

        SongTitle = songTitle;
        SongText = songText;
        Slides = slides;
    }

    public string SongTitle { get; }
    public string SongText { get; }

    public List<PresentationSlideDetail> Slides { get; }
}