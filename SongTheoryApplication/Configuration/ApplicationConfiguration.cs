using System.Collections.Generic;

namespace SongTheoryApplication.Configuration;

public class ApplicationConfiguration
{
    public string? SlideBackgroundColor { get; set; }
    public List<PresentationSetting>? PresentationSettings { get; set; }
    public WindowsSettings? WindowsSettings { get; set; }

}

public class PresentationSetting
{
    public string? Name { get; set; }
    public int FontSize { get; set; }
    public string? FontFamily { get; set; }
    public string? FontColor { get; set; }
    public bool IsBold { get; set; }
}

public class WindowsSettings
{
    public WindowSettings? CreateSongWindow { get; set; }
    public WindowSettings? EditSongWindow { get; set; }
    public WindowSettings? SongListWindow { get; set; }
    public WindowSettings? GenerateSongsPresentationWindow { get; set; }

}

public class WindowSettings
{
    public int Width { get; set; }
    public int Height { get; set; }

    public int Top { get; set; }
    public int Left { get; set; }

    public bool Maximized { get; set; }
}