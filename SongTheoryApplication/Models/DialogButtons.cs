namespace SongTheoryApplication.Models;

using SongTheoryApplication.Views.Windows;

/// <summary>
/// The enum representing all dialog buttons in the <see cref="DialogWindow"/>.
/// </summary>
public enum DialogButtons
{
    /// <summary>
    /// Only OK button is displayed.
    /// </summary>
    OK,
    
    /// <summary>
    /// Only ACCEPT and CANCEL buttons will be displayed.
    /// </summary>
    ACCEPT_CANCEL
}