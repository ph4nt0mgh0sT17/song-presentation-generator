namespace SongTheoryApplication.Models;

public class DialogResult
{
    public bool Ok { get; }
    public bool Accept { get; }
    public bool Cancel { get; }

    public DialogResult(bool ok, bool accept, bool cancel)
    {
        Ok = ok;
        Accept = accept;
        Cancel = cancel;
    }
}