using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;

namespace SongTheoryApplication.Views.Windows;

public partial class DialogWindow : Window
{
    public DialogWindow(string? titleText, string? descriptionText, DialogButtons dialogButtons)
    {
        InitializeComponent();
        DataContext = this;
        TitleText = titleText;
        DescriptionText = descriptionText;
        DialogButtons = dialogButtons;
        
        var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
        var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

        var windowWidth = Width;
        var windowHeight = Height;

        Left = (screenWidth / 2) - (windowWidth / 2);
        Top = (screenHeight / 2) - (windowHeight / 2);
    }

    public string? TitleText { get; set; }
    public string? DescriptionText { get; set; }

    public DialogButtons? DialogButtons { get; set; }

    public ICommand OkButtonCommand => new RelayCommand(Close);

    public Visibility OkButtonLayoutVisibility =>
        DialogButtons == Models.DialogButtons.OK ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility AcceptCancelButtonsLayoutVisiblity =>
        DialogButtons == Models.DialogButtons.ACCEPT_CANCEL ? Visibility.Visible : Visibility.Hidden;
}