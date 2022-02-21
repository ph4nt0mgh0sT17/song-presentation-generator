using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;

namespace SongTheoryApplication.Views.Windows;

public partial class DialogWindow : Window
{
    public DialogWindow(string? titleText, string? descriptionText, DialogButtons buttons, DialogIcons icons)
    {
        InitializeComponent();
        DataContext = this;
        TitleText = titleText;
        DescriptionText = descriptionText;
        Buttons = buttons;
        Icons = icons;
        
        var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
        var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

        var windowWidth = Width;
        var windowHeight = Height;

        Left = (screenWidth / 2) - (windowWidth / 2);
        Top = (screenHeight / 2) - (windowHeight / 2);
    }

    public string? TitleText { get; set; }
    public string? DescriptionText { get; set; }

    public DialogButtons? Buttons { get; set; }
    
    public DialogIcons? Icons { get; set; }

    public ICommand OkButtonCommand => new RelayCommand(Close);

    public Visibility OkButtonLayoutVisibility =>
        Buttons == DialogButtons.OK ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility AcceptCancelButtonsLayoutVisiblity =>
        Buttons == DialogButtons.ACCEPT_CANCEL ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility SuccessIconVisibility =>
        Icons == DialogIcons.SUCCESS ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility InformationIconVisibility =>
        Icons == DialogIcons.INFORMATION ? Visibility.Visible : Visibility.Hidden;
}