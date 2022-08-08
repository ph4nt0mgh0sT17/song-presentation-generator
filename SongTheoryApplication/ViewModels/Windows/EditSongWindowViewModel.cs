using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Constants;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Dialogs;
using SongTheoryApplication.Views.Windows;
using static SongTheoryApplication.Extensions.ProcessExtensions;

namespace SongTheoryApplication.ViewModels.Windows;

public partial class EditSongWindowViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Jméno písničky je požadováno")]
    [MinLength(2, ErrorMessage = "Jméno písničky musí být nejméně 2 znaky dlouhé")]
    private string _songTitle = "";

    [ObservableProperty]
    [Required(ErrorMessage = "Text písničky je požadováno")]
    [MinLength(2, ErrorMessage = "Text písničky musí být nejméně 2 znaky dlouhé")]
    private string _songText = "";

    [ObservableProperty]
    [Required(ErrorMessage = "Zdroj písničky je požadován")]
    [MinLength(2, ErrorMessage = "Zdroj písničky musí být nejméně 2 znaky dlouhý")]
    private string _songSource = "";

    private string SongId { get; }

    [ObservableProperty] private bool _presentationIsBeingGenerated;

    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ILogger<EditSongWindowViewModel> _logger;
    private readonly IApplicationService _applicationService;
    private readonly IShareService _shareService;

    public string EditSongWindowTitleText => "Formulář pro úpravu písničky";
    public IAsyncRelayCommand UpdateSongCommand { get; }

    public EditSongWindow EditSongWindow { get; }

    private readonly Song _song;

    public bool CanUpdateSong => CheckCanUpdateSong();

    private bool CheckCanUpdateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongText) &&
               !string.IsNullOrEmpty(SongSource)&&
               !HasErrors;
    }

    public bool CanGeneratePresentation => CanUpdateSong && _applicationService.IsPowerPointInstalled;

    private bool _isSongShared = false;


    public EditSongWindowViewModel(
        Song? song,
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService,
        IApplicationService applicationService,
        ILogger<EditSongWindowViewModel> logger,
        EditSongWindow? editSongWindow, IShareService shareService)
    {
        Guard.IsNotNull(song);
        Guard.IsNotNull(editSongWindow);
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        UpdateSongCommand = new AsyncRelayCommand(UpdateSong, () => CanUpdateSong);
        _logger = logger;
        _applicationService = applicationService;

        _song = song;

        SongTitle = song.Title;
        SongText = song.Text;
        SongId = song.Id;
        SongSource = song.Source;
        _isSongShared = song.IsSongShared;

        EditSongWindow = editSongWindow;
        _shareService = shareService;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(SongTitle):
            case nameof(SongText):
            case nameof(SongSource):
                UpdateSongCommand.NotifyCanExecuteChanged();
                GenerateSongPresentationCommand.NotifyCanExecuteChanged();
                break;
        }
    }

    private async Task UpdateSong()
    {
        if (!CanUpdateSong) return;

        var editSongRequest =
            new EditSongRequest(SongId, SongTitle, SongText, SongSource, _isSongShared, _song.SharedSongId);

        try
        {
            await _songService.UpdateSongAsync(editSongRequest);
            if (_song.IsSongShared)
            {
                await _shareService.UpdateSongAsync(_song.SharedSongId,
                    new ShareSongRequest(_song.SharedSongId, SongTitle, SongText, SongSource));
            }

            await DisplaySuccessDialog();
        }

        catch (SongCannotBeCreatedException)
        {
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                "Písnička nemohla být upravena.",
                "Písnička nemohla být upravena"
            ));
        }
    }

    private async Task DisplaySuccessDialog()
    {
        await DialogHost.Show(new SuccessNotificationDialogViewModel(
            "Písnička úspěšně upravena!",
            "Písnička byla úspěšně upravena."
        ));

        EditSongWindow.Close();
    }

    [ICommand(CanExecute = nameof(CanGeneratePresentation), AllowConcurrentExecutions = false)]
    public async Task GenerateSongPresentation()
    {
        var slideTexts = await ParseSongIntoSlides();

        if (slideTexts == null)
            return;

        var saveFileDialog = new SaveFileDialog();

        if (saveFileDialog.ShowDialog() == true)
        {
            await GeneratePresentation(saveFileDialog.FileName, slideTexts);
        }
    }

    private async Task GeneratePresentation(string fileName, List<PresentationSlideDetail> slideTexts)
    {
        PresentationIsBeingGenerated = true;

        try
        {
            await Task.Run(() =>
            {
                _presentationGeneratorService.GeneratePresentation(
                    new PresentationGenerationRequest(SongTitle, slideTexts),
                    fileName
                );
            });

            var answer = await DialogHost.Show(new DialogQuestionViewModel(
                "Úspěch",
                "Prezentace písničky byla úspěšně vytvořena. Přejete si nyní zobrazit vygenerovanou prezentaci?"
            ));

            if (answer is true)
            {
                await OpenPresentationFile(fileName);
            }
        }

        catch (AggregateException ex)
        {
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                ApplicationConstants.PresentationCannotBeGeneratedDialog.DESCRIPTION,
                ApplicationConstants.PresentationCannotBeGeneratedDialog.TITLE
            ));

            _logger.LogError(ex.InnerException, ApplicationConstants.Logs.CANNOT_GENERATE_PRESENTATION);
        }

        finally
        {
            PresentationIsBeingGenerated = false;
        }
    }

    private async Task OpenPresentationFile(string fileName)
    {
        try
        {
            StartFileProcess($"{fileName}.pptx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"The file: '{fileName}.pptx' cannot be started.");
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                ApplicationConstants.PresentationCannotBeStartedDialog.DESCRIPTION,
                ApplicationConstants.PresentationCannotBeStartedDialog.TITLE
            ));
        }
    }

    private async Task<List<PresentationSlideDetail>?> ParseSongIntoSlides()
    {
        try
        {
            var slideTexts = SongUtility.ParseSongTextIntoSlides(SongText);
            return slideTexts;
        }
        catch (SongTextParseException ex)
        {
            _logger.LogError(ex, "The song text cannot be parsed into slides.");
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                ex.ApplicationErrorText,
                "Písnička nemůže být vygenerována"
            ));

            return null;
        }
    }
}