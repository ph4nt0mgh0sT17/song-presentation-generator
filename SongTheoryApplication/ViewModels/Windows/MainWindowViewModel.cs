using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using SongTheoryApplication.Services;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    public string _mainWindowTitleText = "Aplikace pro evidenci písniček a jejich vygenerování do PowerPoint prezentace";

    public bool IsConfigurationNotFound => true;

    public SnackbarMessageQueue BoundMessageQueue { get; } = new();

    public MainWindowViewModel(IConfiguration configuration, IApplicationService applicationService)
    {
        CheckConfigurationIsFound(configuration);
        CheckPowerPointIsInstalled(applicationService);
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
                content: "Aplikace PowerPoint nebyla nalezena ve vašem počítači. Nebudete moci vygenerovat písničky.",
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
    private void OpenCreateSongWindow()
    {
        new CreateSongWindow().ShowDialog();
    }

    [ICommand]
    private void OpenSongListWindow()
    {
        new SongListWindow().ShowDialog();
    }

    [ICommand]
    public void OpenGenerateSongsPresentationWindow()
    {
        new GenerateSongsPresentationWindow().ShowDialog();
    }
}