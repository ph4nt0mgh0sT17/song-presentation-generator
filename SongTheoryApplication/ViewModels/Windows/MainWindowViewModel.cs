using System.Windows.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class MainWindowViewModel : BaseViewModel
{
    public string MainWindowTitleText =>
        "Aplikace pro evidenci písniček a jejich vygenerování do PowerPoint prezentace";

    public ICommand OpenCreateSongWindowCommand => new RelayCommand(OpenCreateSongWindow);
    public ICommand OpenSongListWindowCommand => new RelayCommand(OpenSongListWindow);

    private void OpenCreateSongWindow()
    {
        new CreateSongWindow().Show();
    }
    
    private void OpenSongListWindow()
    {
        new DialogWindow(
                "Není implementovaná operace",
                "Tato operace není ještě implementována. Prosím kontaktuje administrátora.",
                DialogButtons.OK)
            .ShowDialog();
    }
}