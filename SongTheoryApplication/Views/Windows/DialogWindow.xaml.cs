using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.Extensions;
using SongTheoryApplication.Models;
using SongTheoryApplication.ViewModels.Base;

namespace SongTheoryApplication.Views.Windows;

public partial class DialogWindow : Window
{
    public DialogWindow(
        string titleText, string descriptionText, 
        DialogButtons buttons = DialogButtons.OK, 
        DialogIcons icons = DialogIcons.INFORMATION)
    {
        InitializeComponent();
        DataContext = this;
        TitleText = titleText;
        DescriptionText = descriptionText;
        Buttons = buttons;
        Icons = icons;
        
        this.CenterScreen();
    }

    public string TitleText { get; set; } = "Test";
    public string DescriptionText { get; set; } = "Test";

    public DialogButtons Buttons { get; set; }
    
    public DialogIcons Icons { get; set; }
    
    public DialogResult? DialogResult { get; private set; }

    public ICommand OkButtonCommand => new RelayCommand(ExecuteOk);
    public ICommand AcceptButtonCommand => new RelayCommand(ExecuteAccept);
    public ICommand CancelButtonCommand => new RelayCommand(ExecuteCancel);

    public Visibility OkButtonLayoutVisibility =>
        Buttons == DialogButtons.OK ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility AcceptCancelButtonsLayoutVisiblity =>
        Buttons == DialogButtons.ACCEPT_CANCEL ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility SuccessIconVisibility =>
        Icons == DialogIcons.SUCCESS ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility InformationIconVisibility =>
        Icons == DialogIcons.INFORMATION ? Visibility.Visible : Visibility.Hidden;

    private void ExecuteOk()
    {
        DialogResult = new DialogResult(true, false, false);
        Close();
    }
    
    private void ExecuteAccept()
    {
        DialogResult = new DialogResult(false, true, false);
        Close();
    }
    
    private void ExecuteCancel()
    {
        DialogResult = new DialogResult(false, false, true);
        Close();
    }

    public static void ShowDialog(string titleText, string descriptionText,
        DialogButtons buttons = DialogButtons.OK,
        DialogIcons icons = DialogIcons.INFORMATION)
    {
        new DialogWindow(titleText, descriptionText, buttons, icons).ShowDialog();
    }
}