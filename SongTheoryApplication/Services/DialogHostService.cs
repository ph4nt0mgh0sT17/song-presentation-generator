using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using MaterialDesignThemes.Wpf;
using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Services;

[Service]
public class DialogHostService : IDialogHostService
{
    public async Task<object?> OpenDialog(object? dialogViewModel, string? dialogIdentifier)
    {
        Guard.IsNotNull(dialogViewModel);
        Guard.IsNotNull(dialogIdentifier);

        return await DialogHost.Show(dialogViewModel, dialogIdentifier);
    }
}