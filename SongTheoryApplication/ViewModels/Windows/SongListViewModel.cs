using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.ViewModels.Dialogs;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class SongListViewModel : BaseViewModel
{
    [ObservableProperty] private string? titleName;

    [ObservableProperty] private ObservableCollection<Song> _songs = new();

    [ObservableProperty] private bool _songsAreLoading;

    private readonly ILocalSongRepository _localSongRepository;
    private readonly ISongService _songService;
    private readonly IPresentationGeneratorService _presentationGeneratorService;
    private readonly ILogger<SongListViewModel> _logger;

    public SongListViewModel(
        ILocalSongRepository localSongRepository,
        ISongService songService,
        IPresentationGeneratorService presentationGeneratorService,
        ILogger<SongListViewModel> logger)
    {
        _localSongRepository = localSongRepository;
        _songService = songService;
        _presentationGeneratorService = presentationGeneratorService;
        _logger = logger;
    }

    [ICommand]
    private async Task OnLoadedAsync()
    {
        SongsAreLoading = true;
        var songs = await _localSongRepository.RetrieveAllSongsAsync();

        Songs = new ObservableCollection<Song>(songs);
        SongsAreLoading = false;
    }

    [ICommand]
    private async Task DeleteSong(Song song)
    {
        var result = await DialogHost.Show(
            new DialogQuestionViewModel(
                $"Chcete doopravdy smazat píseň '{song.Title}'?",
                $"Chcete doopravdy smazat píseň '{song.Title}'?"
            ),
            "SongListDialog"
        );

        if (result is true)
        {
            await _songService.DeleteSongAsync(new DeleteSongRequest(song.Title));
            Songs.Remove(song);
        }
    }

    [ICommand]
    private void ShareSong()
    {
        DialogHost.Show(
            new ErrorNotificationDialogViewModel("Sdílení písniček není zatím implementováno.", "Chyba"),
            "SongListDialog"
        );
    }

    [ICommand]
    private void EditSong()
    {
        DialogHost.Show(
            new ErrorNotificationDialogViewModel("Úprava písniček není zatím implementováno.", "Chyba"),
            "SongListDialog"
        );
    }

    [ICommand]
    private async Task GenerateSongPresentation(Song song)
    {
        var saveFileDialog = new SaveFileDialog();

        if (saveFileDialog.ShowDialog() == true)
        {
            var fileName = saveFileDialog.FileName;

            try
            {
                await Task.Run(() =>
                {
                    var slides = SongUtility.ParseSongTextIntoSlides(song.Text);

                    _presentationGeneratorService.GeneratePresentation(
                        new PresentationGenerationRequest(song.Title, slides),
                        fileName
                    );
                });

                var answer = await DialogHost.Show(new DialogQuestionViewModel(
                    "Úspěch",
                    "Prezentace písničky byla úspěšně vytvořena. Přejete si nyní zobrazit vygenerovanou prezentaci?"
                ), "SongListDialog");

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
                            "Prezentace nemůže být z neznámých důvodu spuštěna. Prosím spusťte ji manuálně.",
                            "Chyba"
                        ), "SongListDialog");
                    }
                }
            }

            catch (AggregateException ex)
            {
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
}