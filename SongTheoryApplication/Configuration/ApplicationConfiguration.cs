using System.Collections.Generic;

namespace SongTheoryApplication.Configuration;

public class ApplicationConfiguration
{
    public List<PresentationSetting>? PresentationSettings { get; set; }
}

public class PresentationSetting
{
    public string? Name { get; set; }
    public int FontSize { get; set; }
    public string? FontFamily { get; set; }
    public string? FontColor { get; set; }
}