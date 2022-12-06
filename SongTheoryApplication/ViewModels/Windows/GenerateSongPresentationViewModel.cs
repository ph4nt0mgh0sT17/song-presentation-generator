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
    [AlsoNotifyCanExecuteFor(nameof(MoveSongUpCommand), nameof(MoveSongDownCommand))]
    private Song? _selectedSong;

    private List<Song> _songDatabase = new List<Song>();

    [ObservableProperty]
    private string? _searchSongQuery = "";

    private readonly ILocalSongRepository _localSongRepository;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ISaveFileDialogProvider _saveFileDialogProvider;
    private readonly ILogger<SongListViewModel> _logger;

    public bool IsSongSelected => SelectedSong != null;

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
            case nameof(SearchSongQuery):
                Task.Run(async () =>
                {
                    if (SearchSongQuery.Length != 0)
                    {
                        var changedSongs = new List<Song>(_songDatabase.Where(DoesSongMatchesSongQuery).ToList());

                        SelectedSongs.ToList().ForEach(selectedSong => changedSongs.Remove(selectedSong));

                        AllSongs = new ObservableCollection<Song>(changedSongs);
                    }
                    else
                    {
                        var changedSongs = new List<Song>(_songDatabase);
                        SelectedSongs.ToList().ForEach(selectedSong => changedSongs.Remove(selectedSong));
                        AllSongs = new ObservableCollection<Song>(changedSongs);
                    }
                });

                break;
        }
    }

    [ICommand]
    public async Task OnLoadedAsync()
    {
        SongsAreLoading = true;

        var songs = await _localSongRepository.RetrieveAllSongsAsync();
        songs = songs.OrderBy(currentSong => currentSong.Title).ToList();
        _songDatabase = new List<Song>(songs);
        AllSongs = new ObservableCollection<Song>(songs);

        SongsAreLoading = false;
    }

    [ICommand(CanExecute = nameof(CanGeneratePresentation))]
    public async Task GeneratePresentation()
    {
        var saveFileDialog = _saveFileDialogProvider.ProvideSaveFileDialog();
        saveFileDialog.Filter = "PowerPoint files (*.pptx)|*.pptx";
        saveFileDialog.DefaultExt = "*.pptx";

        if (saveFileDialog.ShowDialog() == true)
        {
            PresentationIsGenerating = true;
            var fileName = saveFileDialog.FileName.EndsWith(".pptx") ? saveFileDialog.FileName : $"{saveFileDialog.FileName}.pptx";

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
                        "Prezentace písní nemohla být úspěšně vygenerována. Kontaktujte administrátora.",
                        "Chyba"
                    ), "SongListDialog");

                    _logger.LogError(ex, "Cannot generate the presentation");
                }
            }
        }
    }

    [ICommand]
    public void SelectSong(Song song)
    {
        SelectedSongs.Add(song);
        AllSongs.Remove(song);
        GeneratePresentationCommand.NotifyCanExecuteChanged();
    }

    [ICommand]
    public void DeselectSong(Song song)
    {
        SelectedSongs.Remove(song);
        AllSongs.Add(song);
        GeneratePresentationCommand.NotifyCanExecuteChanged();
    }

    [ICommand]
    public void SelectSongForMove(Song song)
    {
        SelectedSong = song;
    }

    [ICommand]
    public void SelectSongForNothing(Song song)
    {
        SelectedSong = null;
    }

    [ICommand(CanExecute = nameof(IsSongSelected))]
    public void MoveSongUp()
    {
        var currentSongIndex = SelectedSongs.IndexOf(SelectedSong);
        var newSongIndex = Math.Max(0, currentSongIndex - 1);

        SelectedSongs.Move(currentSongIndex, newSongIndex);
    }

    [ICommand(CanExecute = nameof(IsSongSelected))]
    public void MoveSongDown()
    {
        var currentSongIndex = SelectedSongs.IndexOf(SelectedSong);
        var newSongIndex = Math.Min(SelectedSongs.Count - 1, currentSongIndex + 1);

        SelectedSongs.Move(currentSongIndex, newSongIndex);
    }

    private async Task OpenPresentation(string fileName)
    {
        try
        {
            ProcessExtensions.StartFileProcess(fileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"The file: '{fileName}' cannot be started.");
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
                "Prezentace písní byla úspěšně vytvořena. Přejete si nyní zobrazit vygenerovanou prezentaci?"
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

    private bool DoesSongMatchesSongQuery(Song song)
    {
        if (SearchSongQuery == null || SearchSongQuery.Length == 0)
            return true;

        return song.Title.Contains(SearchSongQuery.Trim(), StringComparison.CurrentCultureIgnoreCase) ||
            song.Text.Contains(SearchSongQuery.Trim(), StringComparison.CurrentCultureIgnoreCase) ||
            song.Tags != null && song.Tags.Contains(SearchSongQuery.Trim(), StringComparison.CurrentCultureIgnoreCase);
    }
}