using CommunityToolkit.Mvvm.ComponentModel;

namespace SongTheoryApplication.ViewModels.Dialogs;

public class ErrorNotificationDialogViewModel
{
    public string Message { get; }
    public string Title { get; }

    public ErrorNotificationDialogViewModel(string message, string title)
    {
        Message = message;
        Title = title;
    }
}