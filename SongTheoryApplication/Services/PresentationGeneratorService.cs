﻿using System;
using NetOffice.OfficeApi.Enums;
using NetOffice.OfficeApi.Tools.Contribution;
using NetOffice.PowerPointApi;
using NetOffice.PowerPointApi.Enums;

namespace SongTheoryApplication.Services;

public class PresentationGeneratorService : IPresentationGeneratorService
{
    public void GenerateTestingPresentation(string? songTitle, string? songText)
    {
        if (songTitle == null)
            throw new ArgumentNullException(nameof(songTitle));
        
        if (songText == null)
            throw new ArgumentNullException(nameof(songText));
        
        var powerpointApplication = new Application();

        var presentation = GeneratePresentation(songTitle, songText, powerpointApplication);
        presentation.SaveAs("D:\\testinxdg.pptx");

        ExitPowerpointApplication(powerpointApplication);
    }

    private void ExitPowerpointApplication(Application powerpointApplication)
    {
        powerpointApplication.Quit();
        powerpointApplication.Dispose();
    }

    private Presentation GeneratePresentation(string songTitle, string songText, Application powerpointApplication)
    {
        Presentation presentation = powerpointApplication.Presentations.Add(MsoTriState.msoFalse);

        GenerateTitleSlide(songTitle, presentation);
        GenerateTextSlide(songText, presentation);

        return presentation;
    }

    private void GenerateTextSlide(string songText, Presentation presentation)
    {
        Slide textSlide = presentation.Slides.Add(2, PpSlideLayout.ppLayoutBlank);
        Shape songTextLabel =
            textSlide.Shapes.AddLabel(MsoTextOrientation.msoTextOrientationHorizontal, 10, 10, 600, 20);
        songTextLabel.TextFrame.TextRange.Text = songText;
    }

    private void GenerateTitleSlide(string s, Presentation presentation1)
    {
        Slide titleSlide = presentation1.Slides.Add(1, PpSlideLayout.ppLayoutTitleOnly);

        Shape titleLabel = titleSlide.Shapes.Title;
        titleLabel.TextFrame.TextRange.Text = s;
    }
}