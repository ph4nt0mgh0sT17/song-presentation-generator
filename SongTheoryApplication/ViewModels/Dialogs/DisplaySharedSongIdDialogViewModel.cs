using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SongTheoryApplication.ViewModels.Dialogs;

public partial class DisplaySharedSongIdDialogViewModel
{
    public string Message { get; }
    public string Title { get; }
    public string SharedSongId { get; }

    public DisplaySharedSongIdDialogViewModel(string? title, string? message, string? sharedSongId)
    {
        Guard.IsNotNull(title);
        Guard.IsNotNull(message);
        Guard.IsNotNull(sharedSongId);

        Message = message;
        Title = title;
        SharedSongId = sharedSongId;
    }

    [ICommand]
    private void CopySharedSongIdIntoClipboard()
    {
        Clipboard.SetText(SharedSongId);
    }
}