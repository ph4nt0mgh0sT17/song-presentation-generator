using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Models;

public class PresentationTextStyle
{
    public string TextContent { get; set; }
    public string? StyleName { get; set; }

    public PresentationTextStyle(string textContent, string? styleName)
    {
        Guard.IsNotNull(textContent);
        TextContent = textContent;
        StyleName = styleName;
    }
}