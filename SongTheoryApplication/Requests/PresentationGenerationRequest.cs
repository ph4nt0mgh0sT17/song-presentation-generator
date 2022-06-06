using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Requests;

public class PresentationGenerationRequest
{
    public PresentationGenerationRequest(List<PresentationSlideDetail>? slides)
    {
        Guard.IsNotNull(slides, nameof(slides));
        Slides = slides;
    }

    public List<PresentationSlideDetail> Slides { get; }
}