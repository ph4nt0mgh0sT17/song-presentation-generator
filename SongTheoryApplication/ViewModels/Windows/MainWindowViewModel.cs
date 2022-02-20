using System.Windows.Input;
using SongTheoryApplication.ViewModels.Base;
using SongTheoryApplication.Views.Windows;

namespace SongTheoryApplication.ViewModels.Windows;

public class MainWindowViewModel : BaseViewModel
{
    public string MainWindowTitleText =>
        "Aplikace pro evidenci písniček a jejich vygenerování do PowerPoint prezentace";

    public ICommand OpenCreateSongWindow => new RelayCommand(() => new CreateSongWindow().Show());
    public ICommand OpenSongListWindow => new RelayCommand(() => new NotImplementedActionWindow().Show());
}