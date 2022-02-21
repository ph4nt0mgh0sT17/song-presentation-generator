using System.Windows;

namespace SongTheoryApplication.Extensions;

public static class WindowExtensions
{
    public static void CenterScreen(this Window window)
    {
        var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
        var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

        var windowWidth = window.Width;
        var windowHeight = window.Height;

        window.Left = (screenWidth / 2) - (windowWidth / 2);
        window.Top = (screenHeight / 2) - (windowHeight / 2);
    }
}