using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Requests;

public class PresentationGenerationRequest
{
    public string SongTitle { get; }
    public List<PresentationSlideDetail> Slides { get; }

    public PresentationGenerationRequest(string? songTitle, List<PresentationSlideDetail>? slides)
    {
        Guard.IsNotNull(slides, nameof(slides));
        Guard.IsNotNull(songTitle, nameof(songTitle));
        SongTitle = songTitle;
        Slides = slides;
    }

    
}