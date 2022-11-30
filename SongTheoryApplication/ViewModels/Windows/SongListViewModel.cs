using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Providers;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.ViewModels.Dialogs;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class SongListViewModel : ObservableValidator
{
    [ObservableProperty] private string? titleName;

    [ObservableProperty] private ObservableCollection<Song> _songs = new();

    [ObservableProperty] private bool _songsAreLoading;

    [ObservableProperty]
    private string? _searchSongQuery = "";

    private List<Song> _songDatabase = new List<Song>();

    private readonly ILocalSongRepository _localSongRepository;
    private readonly IShareService _shareService;
    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ISaveFileDialogProvider _saveFileDialogProvider;
    private readonly IDialogHostService _dialogHostService;
    private readonly ILogger<SongListViewModel> _logger;

    public SnackbarMessageQueue BoundMessageQueue { get; } = new();

    public SongListViewModel(
        ILocalSongRepository localSongRepository,
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService,
        ISaveFileDialogProvider saveFileDialogProvider,
        IDialogHostService dialogHostService,
        ILogger<SongListViewModel> logger, IShareService shareService,
        IConfiguration configuration, IApplicationService applicationService)
    {
        _localSongRepository = localSongRepository;
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        _logger = logger;
        _shareService = shareService;
        _saveFileDialogProvider = saveFileDialogProvider;
        _dialogHostService = dialogHostService;

        CheckConfigurationIsFound(configuration);
        CheckPowerPointIsInstalled(applicationService);
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

                        Songs = new ObservableCollection<Song>(changedSongs);
                    }
                    else
                    {
                        var changedSongs = new List<Song>(_songDatabase);
                        Songs = new ObservableCollection<Song>(changedSongs);
                    }
                });
                
                break;
        }
    }

    private void CheckConfigurationIsFound(IConfiguration configuration)
    {
        if (configuration is NullConfiguration)
        {
            BoundMessageQueue.Enqueue(
                content: "Konfigurace aplikace nenalezena. Prezentace budou vytvořeny v defaultním nastavení.",
                actionContent: "Zavřít",
                actionHandler: _ => { },
                actionArgument: null,
                promote: true,
                neverConsiderToBeDuplicate: true,
                durationOverride: TimeSpan.FromSeconds(60)
            );
        }
    }

    private void CheckPowerPointIsInstalled(IApplicationService applicationService)
    {
        if (!applicationService.IsPowerPointInstalled)
        {
            BoundMessageQueue.Enqueue(
                content: "Aplikace PowerPoint nebyla nalezena ve vašem počítači. Nebudete moci vygenerovat píseň.",
                actionContent: "Zavřít",
                actionHandler: _ => { },
                actionArgument: null,
                promote: true,
                neverConsiderToBeDuplicate: true,
                durationOverride: TimeSpan.FromSeconds(60)
            );
        }
    }

    [ICommand]
    private async Task OnLoadedAsync()
    {
        SongsAreLoading = true;
        var songs = await _localSongRepository.RetrieveAllSongsAsync();
        songs = songs.OrderBy(currentSong => currentSong.Title).ToList();

        _songDatabase = new List<Song>(songs);

        Songs = new ObservableCollection<Song>(songs);
        SongsAreLoading = false;
    }

    [ICommand]
    private async Task DeleteSong(Song song)
    {
        var result = await _dialogHostService.OpenDialog(
            new DialogQuestionViewModel(
                $"Chcete doopravdy smazat píseň '{song.Title}'?",
                $"Chcete doopravdy smazat píseň '{song.Title}'?"
            ),
            "SongListDialog"
        );

        if (result is true)
        {
            await _songService.DeleteSongAsync(new DeleteSongRequest(song.Id));
            Songs.Remove(song);
        }
    }

    [ICommand]
    private async Task ShareSong(Song song)
    {
        await _dialogHostService.OpenDialog(
            new ShareSongDialogViewModel(_shareService, _songService, song),
            "SongListDialog"
        );

        await RefreshSongs();
    }

    [ICommand]
    private async Task DeleteSharingSongAsync(Song song)
    {
        await _shareService.DeleteSongAsync(song.SharedSongId);

        await _songService.UpdateSongAsync(
            new EditSongRequest(song.Id, song.Title, song.Text, song.Source, song.Tags, false, null, song.IsSongDownloaded, song.CopySongId));
        song.IsSongShared = false;
        song.SharedSongId = null;

        var songIndex = Songs.IndexOf(song);
        Songs.RemoveAt(songIndex);
        Songs.Insert(songIndex, song);

        await _dialogHostService.OpenDialog(
            new SuccessNotificationDialogViewModel("Píseň je odstraněna ze sdílení.", "Úspěch"),
            "SongListDialog"
        );
    }

    [ICommand]
    private async Task ShowSharedSongIdAsync(Song song)
    {
        await _dialogHostService.OpenDialog(
            new DisplaySharedSongIdDialogViewModel("ID sdílené písně",
                $"ID sdílené písně je: {song.SharedSongId}", song.SharedSongId),
            "SongListDialog"
        );
    }

    [ICommand]
    private async Task AddSharedSong()
    {
        await _dialogHostService.OpenDialog(
            Ioc.Default.GetService<AddSharedSongDialogViewModel>(),
            "SongListDialog"
        );

        await RefreshSongs();
    }

    [ICommand]
    private async Task UpdateDownloadedSongAsync(Song song)
    {
        try
        {
            await _shareService.UpdateDownloadedSongAsync(song.SharedSongId);
        }
        catch (SharedSongDoesNotExist)
        {
            await _dialogHostService.OpenDialog(
                new ErrorNotificationDialogViewModel(
                    "Píseň nemůže být aktualizována, protože sdílená píseň již neexistuje.", "Chyba"),
                "SongListDialog"
            );
            return;
        }
        catch (SongDoesNotExist)
        {
            await _dialogHostService.OpenDialog(
                new ErrorNotificationDialogViewModel(
                    "Píseň nemůže být aktualizována, protože píseň neexistuje ve vašem počítači.", "Chyba"),
                "SongListDialog"
            );
            return;
        }

        await RefreshSongs();

        await _dialogHostService.OpenDialog(
            new SuccessNotificationDialogViewModel("Píseň je úspěšně aktualizována.", "Úspěch"),
            "SongListDialog"
        );
    }

    [ICommand]
    private async Task EditSong(Song song)
    {
        var newCopySong = song;
        if (song.IsSongDownloaded)
        {
            if (song.CopySongId != null)
            {
                newCopySong = await _songService.FindSongAsync(currentSong => currentSong.Id == song.CopySongId);
            }
            else
            {

                var result = await _dialogHostService.OpenDialog(
                    new DialogQuestionViewModel("Vytvoření kopie písně",
                        "Přejete si vytvořit kopii písně, abyste ji mohli editovat?"),
                    "SongListDialog"
                );

                if (result is true)
                {
                    newCopySong =
                        await _songService.CreateSongAsync(new CreateSongRequest(song.Title, song.Text, song.Source, song.Tags));
                    await _songService.UpdateSongAsync(
                        new EditSongRequest(
                            song.Id, song.Title, song.Text, song.Source, song.Tags, song.IsSongShared, song.SharedSongId,
                            song.IsSongDownloaded, newCopySong.Id
                        )
                    );
                }
                else
                {
                    return;
                }
            }
        }

        new EditSongWindow(newCopySong).ShowDialog();

        await RefreshSongs();
    }

    [ICommand]
    private async Task GenerateSongPresentation(Song song)
    {
        var saveFileDialog = _saveFileDialogProvider.ProvideSaveFileDialog();
        saveFileDialog.Filter = "PowerPoint files (*.pptx)|*.pptx";
        saveFileDialog.DefaultExt = "*.pptx";

        if (saveFileDialog.ShowDialog() == true)
        {
            var fileName = saveFileDialog.FileName.EndsWith(".pptx") ? saveFileDialog.FileName : $"{saveFileDialog.FileName}.pptx";

            try
            {
                await GeneratePowerpointPresentation(song, fileName);

                var answer = await DisplaySuccessGeneratedPresentationDialog();

                if (answer is true)
                {
                    await OpenPresentation(fileName);
                }
            }

            catch (AggregateException ex)
            {
                _logger.LogError(ex, "Cannot generate the presentation");
                foreach (var innerException in ex.InnerExceptions)
                {
                    if (innerException is not Exception) continue;
                    await _dialogHostService.OpenDialog(new ErrorNotificationDialogViewModel(
                        "Prezentace písně nemohla být úspěšně vygenerována. Kontaktujte administrátora.",
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
            ProcessExtensions.StartFileProcess(fileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"The file: '{fileName}' cannot be started.");
            await _dialogHostService.OpenDialog(new ErrorNotificationDialogViewModel(
                "Prezentace nemůže být z neznámých důvodu spuštěna. Prosím spusťte ji manuálně.",
                "Chyba"
            ), "SongListDialog");
        }
    }

    private async Task<object?> DisplaySuccessGeneratedPresentationDialog()
    {
        var answer = await _dialogHostService.OpenDialog(new DialogQuestionViewModel(
            "Úspěch",
            "Prezentace písně byla úspěšně vytvořena. Přejete si nyní zobrazit vygenerovanou prezentaci?"
        ), "SongListDialog");
        return answer;
    }

    private async Task GeneratePowerpointPresentation(Song song, string fileName)
    {
        await Task.Run(() =>
        {
            var slides = SongUtility.ParseSongTextIntoSlides(song.Text);

            _presentationGeneratorService.GeneratePresentation(
                new PresentationGenerationRequest(song.Title, slides),
                fileName
            );
        });
    }

    [ICommand]
    private async Task OpenCreateSongWindow()
    {
        new CreateSongWindow().ShowDialog();

        await RefreshSongs();
    }

    private async Task RefreshSongs()
    {
        await OnLoadedAsync();

        if (SearchSongQuery.Length != 0)
        {
            var changedSongs = new List<Song>(_songDatabase.Where(DoesSongMatchesSongQuery).ToList());

            Songs = new ObservableCollection<Song>(changedSongs);
        }
        else
        {
            var changedSongs = new List<Song>(_songDatabase);
            Songs = new ObservableCollection<Song>(changedSongs);
        }
    }

    [ICommand]
    public void OpenGenerateSongsPresentationWindow()
    {
        new GenerateSongsPresentationWindow().ShowDialog();
    }

    [ICommand]
    public async Task SearchSongs()
    {
        if (SearchSongQuery == null || SearchSongQuery.Length == 0)
            return;

        var songs = await _localSongRepository.RetrieveAllSongsAsync();

        var filteredSongs = songs.Where(DoesSongMatchesSongQuery).ToList();

        Songs = new ObservableCollection<Song>(filteredSongs);
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