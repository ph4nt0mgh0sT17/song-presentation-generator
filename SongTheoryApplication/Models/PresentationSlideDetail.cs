namespace SongTheoryApplication.Models;

public class PresentationSlideDetail
{
    public PresentationFormatStyle PresentationFormatStyle { get; set; }
    public string TextContent { get; set; }
    
    public PresentationSlideDetail(PresentationFormatStyle presentationFormatStyle, string textContent)
    {
        PresentationFormatStyle = presentationFormatStyle;
        TextContent = textContent;
    }
}