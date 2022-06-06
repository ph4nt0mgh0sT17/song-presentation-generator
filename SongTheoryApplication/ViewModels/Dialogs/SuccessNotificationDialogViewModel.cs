using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongTheoryApplication.ViewModels.Dialogs;

public class SuccessNotificationDialogViewModel
{
    public string Message { get; }
    public string Title { get; }

    public SuccessNotificationDialogViewModel(string message, string title)
    {
        Message = message;
        Title = title;
    }
}