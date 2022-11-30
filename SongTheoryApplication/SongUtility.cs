using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Models;

namespace SongTheoryApplication;

public static class SongUtility
{
    /// <summary>
    /// Parses the song text into <see cref="List{T}"/> of <see cref="PresentationSlideDetail"/> objects.
    /// </summary>
    /// <param name="songText">The <see cref="string"/> text to be parsed.</param>
    /// <returns><see cref="List{T}"/> of <see cref="PresentationSlideDetail"/> objcets.</returns>
    /// <exception cref="SongTextParseException">When the text cannot be parsed.</exception>
    public static List<PresentationSlideDetail> ParseSongTextIntoSlides(string? songText)
    {
        Guard.IsNotNull(songText, nameof(songText));

        var songTextLines = SplitToLines(songText);

        var slides = new List<PresentationSlideDetail>();

        PresentationSlideDetail? currentPresentationSlide;
        PresentationTextStyle? currentPresentationTextStyle;

        var pattern = Regex.Match(songTextLines[0], @"\/style(\(([a-zA-Z0-9_\-,\.]+)\)|)");
        if (pattern.Success)
        {
            if (songTextLines[0].Trim() == "/style")
            {
                currentPresentationTextStyle = new PresentationTextStyle("", "Default");
            }
            else
            {
                currentPresentationTextStyle = new PresentationTextStyle("", pattern.Groups[2].Value);
            }
            currentPresentationSlide = new PresentationSlideDetail(new PresentationFormatStyle("Center"));
            currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);
        }

        else
        {
            currentPresentationTextStyle = new PresentationTextStyle("", "Default");
            currentPresentationSlide = new PresentationSlideDetail(new PresentationFormatStyle("Center"));
            currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);

            if (!songTextLines[0].StartsWith("/style"))
            {
                currentPresentationTextStyle.TextContent += songTextLines[0] + "\n";
            }
        }

        for (var index = 1; index < songTextLines.Count; index++)
        {
            var currentTextLine = songTextLines[index];

            if (currentTextLine.StartsWith("/"))
            {
                // TODO: Do special operations (/end-slide or /use-style)
                //       However, /use-style cannot be used twice in the same style
                if (currentTextLine.StartsWith("/style"))
                {
                    if (currentPresentationTextStyle.TextContent != "")
                    {
                        pattern = Regex.Match(currentTextLine, @"\/style(\(([a-zA-Z0-9_\-,\.]+)\)|)");
                        if (pattern.Success)
                        {
                            if (currentTextLine.Trim() == "/style")
                            {
                                currentPresentationTextStyle = new PresentationTextStyle("", "Default");
                            }
                            else
                            {
                                currentPresentationTextStyle = new PresentationTextStyle("", pattern.Groups[2].Value);
                            }
                            currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);
                        }
                        else
                        {
                            currentPresentationTextStyle = new PresentationTextStyle("", "Default");
                            currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);
                        }
                    }
                    else
                    {

                        pattern = Regex.Match(currentTextLine, @"\/style(\(([a-zA-Z0-9_\-,\.]+)\)|)");
                        if (pattern.Success)
                        {
                            if (currentTextLine.Trim() == "/style")
                            {
                                currentPresentationTextStyle.StyleName = "Default";
                            }
                            else
                            {
                                currentPresentationTextStyle.StyleName = pattern.Groups[2].Value;
                            }
                        }
                        else
                        {
                            currentPresentationTextStyle.StyleName = "Default";
                        }
                    }

                    continue;
                }

                if (currentTextLine.StartsWith("/slide"))
                {
                    slides.Add(currentPresentationSlide);
                    currentPresentationSlide =
                        new PresentationSlideDetail(currentPresentationSlide.PresentationFormatStyle);
                    currentPresentationTextStyle =
                        new PresentationTextStyle("", currentPresentationTextStyle.StyleName);

                    currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);

                    continue;
                }
            }

            currentPresentationTextStyle.TextContent += currentTextLine + "\n";
        }

        if (!currentPresentationSlide.PresentationTextStyles.Contains(currentPresentationTextStyle))
        {
            currentPresentationSlide.PresentationTextStyles.Add(currentPresentationTextStyle);
        }

        if (!slides.Contains(currentPresentationSlide))
        {
            slides.Add(currentPresentationSlide);
        }

        return slides;
    }

    private static List<string> SplitToLines(string songText)
    {
        var songTextLines = songText.Split('\n')
            .Select(line => line.Trim())
            .ToList();
        return songTextLines;
    }
}
