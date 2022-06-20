using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;
using NetOffice.OfficeApi.Enums;
using NetOffice.PowerPointApi;
using NetOffice.PowerPointApi.Enums;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

[Service]
public class PresentationGeneratorService : IPresentationGeneratorService
{
    private readonly IConfiguration _configuration;

    private readonly Dictionary<string, int> COLORS = new()
    {
        // TODO: Add more colors (only basic ones) -> It is not expected to be some fancy colors
        { "Red", 16711680 },
        { "Blue", 255 },
        { "Green", 32768 },
        { "Black", 0 }
    };


    public PresentationGeneratorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///     Exits the PowerPoint application that is running in the background.
    /// </summary>
    /// <param name="powerpointApplication">The PowerPoint application running in the background.</param>
    private void ExitPowerpointApplication(Application powerpointApplication)
    {
        powerpointApplication.Quit();
        powerpointApplication.Dispose();
    }

    /// <summary>
    ///     Generates the slide with the text of the song.
    /// </summary>
    /// <param name="songText">The text of the song</param>
    /// <param name="presentation">The <see cref="Presentation" /> object that will contain the slide.</param>
    private void GenerateTextSlide(PresentationSlideDetail slideDetail, Presentation presentation, int slideIndex = 2)
    {
        var configuration = _configuration.Get<ApplicationConfiguration>();

        var defaultSettings =
            configuration.PresentationSettings?.FirstOrDefault(x => x.Name == slideDetail.StyleName) ??
            configuration.PresentationSettings?.FirstOrDefault(x => x.Name == "Default") ??
            new PresentationSetting
            {
                Name = "Default",
                FontColor = "Black",
                FontFamily = "Times New Roman",
                FontSize = 15
            };

        var textSlide = presentation.Slides.Add(slideIndex, PpSlideLayout.ppLayoutBlank);
        var songTextLabel =
            textSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 10, 800, 500);
        songTextLabel.TextFrame.TextRange.Text = slideDetail.TextContent;
        songTextLabel.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter;
        songTextLabel.TextFrame.TextRange.Font.Name = defaultSettings.FontFamily;
        songTextLabel.TextFrame.TextRange.Font.Size = defaultSettings.FontSize;

        if (defaultSettings.FontColor == null)
            throw new InvalidOperationException("The application configuration needs to have 'FontColor' parameter.");

        if (!COLORS.ContainsKey(defaultSettings.FontColor))
        {
            songTextLabel.TextFrame.TextRange.Font.Color.RGB = COLORS["Black"];
            // TODO: Log there is no color like this.
        }

        else
        {
            songTextLabel.TextFrame.TextRange.Font.Color.RGB = COLORS[defaultSettings.FontColor];
        }

        songTextLabel.TextFrame.TextRange.Font.Color.RGB = COLORS[defaultSettings.FontColor ?? "Black"];
        songTextLabel.TextFrame.TextRange.Font.Bold =
            defaultSettings.IsBold ? MsoTriState.msoTrue : MsoTriState.msoFalse;
    }

    /// <summary>
    ///     Generates the slide with the title of the song.
    /// </summary>
    /// <param name="songTitle">The title of the song</param>
    /// <param name="presentation">The <see cref="Presentation" /> object that will contain the slide.</param>
    private void GenerateTitleSlide(string songTitle, Presentation presentation, int slideIndex = 1)
    {
        var titleSlide = presentation.Slides.Add(slideIndex, PpSlideLayout.ppLayoutTitleOnly);

        var titleLabel = titleSlide.Shapes.Title;
        titleLabel.TextFrame.TextRange.Text = songTitle;
    }

    public void GeneratePresentation(PresentationGenerationRequest? presentationGenerationRequest, string fileName)
    {
        Guard.IsNotNull(presentationGenerationRequest, nameof(presentationGenerationRequest));

        var powerpointApplication = new Application();

        var presentation = powerpointApplication.Presentations.Add(MsoTriState.msoFalse);

        int slideIndex = 1;
        presentationGenerationRequest.Slides.ForEach(currentSlide =>
        {
            GenerateTextSlide(currentSlide, presentation, slideIndex);
            slideIndex++;
        });

        presentation.SaveAs($"{fileName}.pptx");

        ExitPowerpointApplication(powerpointApplication);
    }

    public void GenerateMultipleSongsPresentation(
        List<PresentationGenerationRequest>? presentationGenerationRequests,
        bool addEmptySlidesBetweenSongs,
        string fileName)
    {
        Guard.IsNotNull(presentationGenerationRequests, nameof(presentationGenerationRequests));

        var powerpointApplication = new Application();

        var presentation = powerpointApplication.Presentations.Add(MsoTriState.msoFalse);

        var slideIndex = 1;
        for (var index = 0; index < presentationGenerationRequests.Count; index++)
        {
            if (CanAddEmptySlidesBetweenSongs(addEmptySlidesBetweenSongs, index))
            {
                CreateEmptySlide(presentation, ref slideIndex);
            }

            var presentationGenerationRequest = presentationGenerationRequests[index];

            GenerateSongSlides(presentationGenerationRequest, presentation, ref slideIndex);
        }

        presentation.SaveAs($"{fileName}.pptx");

        ExitPowerpointApplication(powerpointApplication);
    }

    private void GenerateSongSlides(
        PresentationGenerationRequest presentationGenerationRequest,
        Presentation presentation,
        ref int slideIndex)
    {
        GenerateTitleSlide(presentationGenerationRequest.SongTitle, presentation, slideIndex);
        slideIndex++;

        foreach (var currentSlide in presentationGenerationRequest.Slides)
        {
            GenerateTextSlide(currentSlide, presentation, slideIndex);
            slideIndex++;
        }
    }

    private static bool CanAddEmptySlidesBetweenSongs(bool addEmptySlidesBetweenSongs, int index)
    {
        return addEmptySlidesBetweenSongs && index != 0;
    }

    private void CreateEmptySlide(Presentation presentation, ref int slideIndex)
    {
        GenerateTextSlide(
            new PresentationSlideDetail(new PresentationFormatStyle("Unknown"), ""),
            presentation,
            slideIndex
        );

        slideIndex++;
    }
}