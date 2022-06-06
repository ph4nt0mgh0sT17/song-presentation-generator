using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using NetOffice.PowerPointApi;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Configuration;
using Fonts = System.Windows.Media.Fonts;

namespace SongTheoryApplication.ViewModels.Windows;

[ViewModel]
public partial class ApplicationSettingsViewModel : ObservableObject
{
    public List<string> FontFamilies { get; }

    public List<int> FontSizes { get; } = new()
    {
        10,
        15,
        20,
        25,
        30,
        35,
        40
    };

    public List<string> FontColors { get; }

    public ApplicationConfiguration ApplicationSettings { get; }

    [ObservableProperty] private PresentationSetting _currentSelectedPresentationSetting;
    [ObservableProperty] private string _selectedFont;
    [ObservableProperty] private int _selectedFontSize;
    [ObservableProperty] private string _selectedFontColor;

    public ApplicationSettingsViewModel(IConfiguration configuration)
    {
        FontColors = RetrieveAllColorNames();
        ApplicationSettings = configuration.Get<ApplicationConfiguration>();
        CurrentSelectedPresentationSetting = ApplicationSettings.PresentationSettings.First();
        SelectedFont = CurrentSelectedPresentationSetting.FontFamily;
        SelectedFontSize = CurrentSelectedPresentationSetting.FontSize;
        SelectedFontColor = CurrentSelectedPresentationSetting.FontColor;
        FontFamilies = Fonts.SystemFontFamilies
            .Select(fontFamily => fontFamily.Source)
            .ToList();
    }

    [ICommand]
    public void SelectPresentationSetting(string presentationSettingName)
    {
        CurrentSelectedPresentationSetting = ApplicationSettings.PresentationSettings
            .Where(presentationSetting => presentationSetting.Name == presentationSettingName)
            .First();

        SelectedFont = CurrentSelectedPresentationSetting.FontFamily;
        SelectedFontSize = CurrentSelectedPresentationSetting.FontSize;
        SelectedFontColor = CurrentSelectedPresentationSetting.FontColor;
    }

    private List<string> RetrieveAllColorNames()
    {
        return Enum.GetValues<KnownColor>()
            .Select(knownColor => Color.FromKnownColor(knownColor).Name)
            .ToList();
    }
}