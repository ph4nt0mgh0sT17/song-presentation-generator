using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Providers;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Dialogs;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class GenerateSongPresentationViewModel : ObservableObject
{
    [ObservableProperty] private bool _songsAreLoading;
    [ObservableProperty] private ObservableCollection<Song> _allSongs = new();
    [ObservableProperty] private ObservableCollection<Song> _selectedSongs = new();
    [ObservableProperty] private bool _presentationIsGenerating;
    [ObservableProperty] private bool _isAddEmptySlideBetweenSongsChecked;

    [ObservableProperty]
    [AlsoNotifyCanExecuteFor(nameof(SelectSongCommand))]
    private Song? _selectedSong;

    [ObservableProperty]
    [AlsoNotifyCanExecuteFor(nameof(SelectSongCommand))]
    [AlsoNotifyCanExecuteFor(nameof(DeselectSongCommand))]
    [AlsoNotifyCanExecuteFor(nameof(MoveSongDownCommand), nameof(MoveSongUpCommand))]
    private Song? _selectedSongToDeselect;

    private readonly ILocalSongRepository _localSongRepository;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ISaveFileDialogProvider _saveFileDialogProvider;
    private readonly ILogger<SongListViewModel> _logger;

    public bool IsSongSelected => SelectedSong != null;
    public bool IsSongToDeselectSelected => SelectedSongToDeselect != null;
    public bool CanGeneratePresentation => SelectedSongs.Count > 0;

    public GenerateSongPresentationViewModel(
        ILocalSongRepository localSongRepository,
        IPresentationGeneratorService presentationGeneratorService,
        ISaveFileDialogProvider saveFileDialogProvider,
        ILogger<SongListViewModel> logger)
    {
        _localSongRepository = localSongRepository;
        _presentationGeneratorService = presentationGeneratorService;
        _logger = logger;
        _saveFileDialogProvider = saveFileDialogProvider;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(SelectedSong):
                SelectedSongToDeselect = null;
                break;

            case nameof(SelectedSongToDeselect):
                SelectedSong = null;
                break;
        }
    }

    [ICommand]
    public async Task OnLoadedAsync()
    {
        SongsAreLoading = true;

        var songs = await _localSongRepository.RetrieveAllSongsAsync();
        AllSongs = new ObservableCollection<Song>(songs);

        SongsAreLoading = false;
    }

    [ICommand(CanExecute = nameof(IsSongSelected))]
    public void SelectSong()
    {
        Guard.IsNotNull(SelectedSong);

        SelectedSongs.Add(SelectedSong);
        AllSongs.Remove(SelectedSong);
        SelectedSong = null;
        GeneratePresentationCommand.NotifyCanExecuteChanged();
    }

    [ICommand(CanExecute = nameof(IsSongToDeselectSelected))]
    public void DeselectSong()
    {
        Guard.IsNotNull(SelectedSongToDeselect);

        AllSongs.Add(SelectedSongToDeselect);
        SelectedSongs.Remove(SelectedSongToDeselect);
        SelectedSongToDeselect = null;
        GeneratePresentationCommand.NotifyCanExecuteChanged();
    }

    [ICommand(CanExecute = nameof(IsSongToDeselectSelected))]
    public void MoveSongUp()
    {
        var currentSongIndex = SelectedSongs.IndexOf(SelectedSongToDeselect);
        var newSongIndex = Math.Max(0, currentSongIndex - 1);

        SelectedSongs.Move(currentSongIndex, newSongIndex);
    }

    [ICommand(CanExecute = nameof(IsSongToDeselectSelected))]
    public void MoveSongDown()
    {
        var currentSongIndex = SelectedSongs.IndexOf(SelectedSongToDeselect);
        var newSongIndex = Math.Min(SelectedSongs.Count - 1, currentSongIndex + 1);

        SelectedSongs.Move(currentSongIndex, newSongIndex);
    }

    [ICommand(CanExecute = nameof(CanGeneratePresentation))]
    public async Task GeneratePresentation()
    {
        var saveFileDialog = _saveFileDialogProvider.ProvideSaveFileDialog();

        if (saveFileDialog.ShowDialog() == true)
        {
            PresentationIsGenerating = true;
            var fileName = saveFileDialog.FileName;

            var presentationGenerationRequests = SelectedSongs
                .Select(selectedSong => new PresentationGenerationRequest(
                    selectedSong.Title,
                    SongUtility.ParseSongTextIntoSlides(selectedSong.Text)
                )).ToList();

            try
            {
                await GeneratePowerPointPresentation(presentationGenerationRequests, fileName);

                PresentationIsGenerating = false;

                var answer = await DisplaySuccessGenerationDialog();

                if (answer is true)
                {
                    await OpenPresentation(fileName);
                }
            }

            catch (AggregateException ex)
            {
                PresentationIsGenerating = false;
                foreach (var innerException in ex.InnerExceptions)
                {
                    if (innerException is not Exception) continue;
                    await DialogHost.Show(new ErrorNotificationDialogViewModel(
                        "Prezentace písničky nemohla být úspěšně vygenerována. Kontaktujte administrátora.",
                        "Chyba"
                    ), "SongListDialog");

                    _logger.LogError(ex, "Cannot generate the presentation");
                }
            }
        }
    }

    private async Task OpenPresentation(string fileName)
    {
        try
        {
            ProcessExtensions.StartFileProcess($"{fileName}.pptx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"The file: '{fileName}.pptx' cannot be started.");
            await DialogHost.Show(new ErrorNotificationDialogViewModel(
                "Prezentace nemůže být z neznámých důvodu spuštěna. Prosím spusťte ji manuálně.",
                "Chyba"
            ), "SongListDialog");
        }
    }

    private static async Task<object?> DisplaySuccessGenerationDialog()
    {
        var answer = await DialogHost.Show(
            new DialogQuestionViewModel(
                "Úspěch",
                "Prezentace písniček byla úspěšně vytvořena. Přejete si nyní zobrazit vygenerovanou prezentaci?"
            ),
            "GenerateSongPresentationDialog"
        );
        return answer;
    }

    private async Task GeneratePowerPointPresentation(List<PresentationGenerationRequest> presentationGenerationRequests, string fileName)
    {
        await Task.Run(() =>
        {
            _presentationGeneratorService.GenerateMultipleSongsPresentation(
                presentationGenerationRequests,
                IsAddEmptySlideBetweenSongsChecked,
                fileName
            );
        });
    }
}