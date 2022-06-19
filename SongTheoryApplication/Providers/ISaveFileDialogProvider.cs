using Microsoft.Win32;

namespace SongTheoryApplication.Providers;

public interface ISaveFileDialogProvider
{
    /// <summary>
    /// Provides <see cref="SaveFileDialog"/> for saving files.
    /// </summary>
    /// <returns>The <see cref="SaveFileDialog"/>.</returns>
    SaveFileDialog ProvideSaveFileDialog();
}