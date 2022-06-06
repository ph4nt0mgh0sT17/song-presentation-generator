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

        var pattern = Regex.Match(songTextLines[0], @"\/use-style\(([a-zA-Z0-9_\-,\.]+)\)");
        if (pattern.Success)
        {
            currentPresentationSlide = new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
            {
                StyleName = pattern.Groups[1].Value
            };
        }

        else
        {
            currentPresentationSlide = new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
            {
                StyleName = "Default"
            };

            if (!songTextLines[0].StartsWith("/use-style"))
            {
                currentPresentationSlide.TextContent += songTextLines[0] + "\n";
            }
        }

        for (var index = 1; index < songTextLines.Count; index++)
        {
            var currentTextLine = songTextLines[index];

            if (currentTextLine.StartsWith("/"))
            {
                // TODO: Do special operations (/end-slide or /use-style)
                //       However, /use-style cannot be used twice in the same style
                if (currentTextLine.StartsWith("/use-style"))
                {
                    if (currentPresentationSlide.TextContent.Length > 0)
                    {
                        throw new SongTextParseException(
                            "Parse invalid.",
                            "Písnička nemůže být vytvořena/vygenerována, protože nemůžete použít 2 styly ve stejném slidu ve kterém už je nějaký text."
                        );
                    }

                    pattern = Regex.Match(currentTextLine, @"\/use-style\(([a-zA-Z0-9_\-,\.]+)\)");
                    if (pattern.Success)
                    {
                        currentPresentationSlide =
                            new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
                            {
                                StyleName = pattern.Groups[1].Value
                            };
                    }
                    else
                    {
                        currentPresentationSlide =
                            new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
                            {
                                StyleName = "Default"
                            };
                    }

                    continue;
                }

                if (currentTextLine.StartsWith("/new-slide"))
                {
                    slides.Add(currentPresentationSlide);
                    currentPresentationSlide =
                        new PresentationSlideDetail(currentPresentationSlide.PresentationFormatStyle, "")
                        {
                            StyleName = currentPresentationSlide.StyleName
                        };

                    continue;
                }
            }

            currentPresentationSlide.TextContent += currentTextLine + "\n";
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

    private static PresentationSlideDetail CreateNewPresentationSlideDetail(string songTextLine)
    {
        var pattern = Regex.Match(songTextLine, @"\/use-style\(([a-zA-Z0-9_\-,\.]+)\)");
        if (pattern.Success)
        {
            return new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
            {
                StyleName = pattern.Groups[1].Value
            };
        }

        var currentPresentationSlide = new PresentationSlideDetail(new PresentationFormatStyle("Center"), "")
        {
            StyleName = "Default"
        };

        if (!songTextLine.StartsWith("/use-style"))
        {
            currentPresentationSlide.TextContent += songTextLine + "\n";
        }

        return currentPresentationSlide;
    }
}