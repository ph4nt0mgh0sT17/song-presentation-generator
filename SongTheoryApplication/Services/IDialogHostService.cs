using System.Threading.Tasks;

namespace SongTheoryApplication.Services;

public interface IDialogHostService
{
    Task<object?> OpenDialog(object? dialogViewModel, string? dialogIdentifier);
}