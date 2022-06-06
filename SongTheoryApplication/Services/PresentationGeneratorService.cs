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

    private Dictionary<string, int> COLORS = new()
    {
        { "Red", 16711680 },
        { "Blue", 255 },
        { "Green", 32768 }
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
    ///     Generates the whole PowerPoint presentation.
    /// </summary>
    /// <param name="songTitle">The title of the song.</param>
    /// <param name="songText">The text of the song.</param>
    /// <param name="powerpointApplication">The PowerPoint application running in the background.</param>
    /// <returns>The <see cref="Presentation" /> object that is used to save the presentation to specific file name location.</returns>
    private Presentation GeneratePresentation(string songTitle, string songText, Application powerpointApplication)
    {
        var presentation = powerpointApplication.Presentations.Add(MsoTriState.msoFalse);

        GenerateTitleSlide(songTitle, presentation);
        GenerateTextSlide(
            new PresentationSlideDetail(new PresentationFormatStyle("Center"), songText)
            {
                StyleName = "Default"
            },
            presentation
        );

        return presentation;
    }

    /// <summary>
    ///     Generates the slide with the text of the song.
    /// </summary>
    /// <param name="songText">The text of the song</param>
    /// <param name="presentation">The <see cref="Presentation" /> object that will contain the slide.</param>
    private void GenerateTextSlide(PresentationSlideDetail slideDetail, Presentation presentation, int slideIndex = 2)
    {
        var configuration = _configuration.Get<ApplicationConfiguration>();

        var defaultSettings = configuration.PresentationSettings.First(x => x.Name == slideDetail.StyleName);

        var textSlide = presentation.Slides.Add(slideIndex, PpSlideLayout.ppLayoutBlank);
        var songTextLabel =
            textSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 10, 10, 800, 500);
        songTextLabel.TextFrame.TextRange.Text = slideDetail.TextContent;
        songTextLabel.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter;
        songTextLabel.TextFrame.TextRange.Font.Name = defaultSettings.FontFamily;
        songTextLabel.TextFrame.TextRange.Font.Size = defaultSettings.FontSize;
        songTextLabel.TextFrame.TextRange.Font.Color.RGB = COLORS[defaultSettings.FontColor];
    }

    /// <summary>
    ///     Generates the slide with the title of the song.
    /// </summary>
    /// <param name="songTitle">The title of the song</param>
    /// <param name="presentation">The <see cref="Presentation" /> object that will contain the slide.</param>
    private void GenerateTitleSlide(string songTitle, Presentation presentation)
    {
        var titleSlide = presentation.Slides.Add(1, PpSlideLayout.ppLayoutTitleOnly);

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
}