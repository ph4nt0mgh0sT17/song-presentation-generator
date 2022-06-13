using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Constants;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Dialogs;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class CreateSongWindowViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Jméno písničky je požadováno")]
    [MinLength(2, ErrorMessage = "Jméno písničky musí být nejméně 2 znaky dlouhé")]
    private string _songTitle = "";

    [ObservableProperty]
    [Required(ErrorMessage = "Text písničky je požadováno")]
    [MinLength(2, ErrorMessage = "Text písničky musí být nejméně 2 znaky dlouhé")]
    private string _songText = "";

    [ObservableProperty] private bool _presentationIsBeingGenerated;

    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ILogger<CreateSongWindowViewModel> _logger;
    private readonly IApplicationService _applicationService;

    public string CreateSongWindowTitleText => "Formulář pro vytvoření písničky";
    public IAsyncRelayCommand CreateSongCommand { get; }

    public bool CanCreateSong => CheckCanCreateSong();

    private bool CheckCanCreateSong()
    {
        return !string.IsNullOrEmpty(SongTitle) &&
               !string.IsNullOrEmpty(SongText);
    }

    public bool CanGeneratePresentation => CanCreateSong && _applicationService.IsPowerPointInstalled;

    public CreateSongWindow? CreateSongWindow { get; set; }



    public CreateSongWindowViewModel(
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService,
        IApplicationService applicationService,
        ILogger<CreateSongWindowViewModel> logger)
    {
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        CreateSongCommand = new AsyncRelayCommand(CreateSong, () => CanCreateSong);
        _logger = logger;
        _applicationService = applicationService;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(SongTitle):
            case nameof(SongText):
                CreateSongCommand.NotifyCanExecuteChanged();
                GenerateSongPresentationCommand.NotifyCanExecuteChanged();
                break;
        }
    }

    private async Task CreateSong()
    {
        if (!CanCreateSong) return;

        var createSongRequest = new CreateSongRequest(SongTitle, SongText);

        await Task.Run(() => { _songService.CreateSong(createSongRequest); });

        if (CreateSongWindow == null)
        {
            await DialogHost.Show(new SuccessNotificationDialogViewModel(
                "Písnička úspěšně vytvořena!",
                "Písnička byla úspěšně vytvořena, ale okno nemůže být z neznámých důvodu zavřeno. Prosím zavřete ho manuálně."
            ));
        }

        else
        {
            await DialogHost.Show(new SuccessNotificationDialogViewModel(
                "Písnička úspěšně vytvořena!",
                "Písnička byla úspěšně vytvořena."
            ));
            CreateSongWindow.Close();
        }
    }

    [ICommand(CanExecute = nameof(CanGeneratePresentation), AllowConcurrentExecutions = false)]
    public async Task GenerateSongPresentation()
    {
        var slideTexts = new List<PresentationSlideDetail>();
        try
        {
            slideTexts = SongUtility.ParseSongTextIntoSlides(SongText);
        }
        catch (SongTextParseException ex)
        {
            _logger.LogError(ex, "The song text cannot be parsed into slides.");
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                ex.ApplicationErrorText,
                "Písnička nemůže být vygenerována"
            ));
            return;
        }

        var saveFileDialog = new SaveFileDialog();

        if (saveFileDialog.ShowDialog() == true)
        {
            PresentationIsBeingGenerated = true;
            var fileName = saveFileDialog.FileName;

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
                    try
                    {
                        ProcessExtensions.StartFileProcess($"{fileName}.pptx");
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
    }
}