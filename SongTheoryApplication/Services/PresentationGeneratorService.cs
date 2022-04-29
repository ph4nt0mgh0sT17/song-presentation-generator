using CommunityToolkit.Diagnostics;
using NetOffice.OfficeApi.Enums;
using NetOffice.PowerPointApi;
using NetOffice.PowerPointApi.Enums;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Services;

[Service]
public class PresentationGeneratorService : IPresentationGeneratorService
{
    /// <inheritdoc cref="IPresentationGeneratorService.GenerateTestingPresentation" />
    public void GenerateTestingPresentation(string? songTitle, string? songText, string fileName)
    {
        Guard.IsNotNull(songTitle, nameof(songTitle));
        Guard.IsNotNull(songText, nameof(songText));

        var powerpointApplication = new Application();

        var presentation = GeneratePresentation(songTitle, songText, powerpointApplication);
        presentation.SaveAs($"{fileName}.pptx");

        ExitPowerpointApplication(powerpointApplication);
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
        GenerateTextSlide(songText, presentation);

        return presentation;
    }

    /// <summary>
    ///     Generates the slide with the text of the song.
    /// </summary>
    /// <param name="songText">The text of the song</param>
    /// <param name="presentation">The <see cref="Presentation" /> object that will contain the slide.</param>
    private void GenerateTextSlide(string songText, Presentation presentation, int slideIndex = 2)
    {
        var textSlide = presentation.Slides.Add(slideIndex, PpSlideLayout.ppLayoutBlank);
        var songTextLabel =
            textSlide.Shapes.AddLabel(MsoTextOrientation.msoTextOrientationHorizontal, 10, 10, 600, 20);
        songTextLabel.TextFrame.TextRange.Text = songText;
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

    public void GeneratePresentation(Song? song, string fileName)
    {
        Guard.IsNotNull(song, nameof(song));

        var powerpointApplication = new Application();

        var presentation = powerpointApplication.Presentations.Add(MsoTriState.msoFalse);

        int slideIndex = 1;
        song.Slides.ForEach(currentSlideFormat =>
        {
            GenerateTextSlide(currentSlideFormat.TextContent, presentation, slideIndex);
            slideIndex++;
        });

        presentation.SaveAs($"{fileName}.pptx");

        ExitPowerpointApplication(powerpointApplication);
    }
}