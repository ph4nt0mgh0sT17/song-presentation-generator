using Microsoft.Win32;
using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Providers;

[Service]
public class SaveFileDialogProvider : ISaveFileDialogProvider
{
    public SaveFileDialog ProvideSaveFileDialog()
    {
        return new SaveFileDialog();
    }
}