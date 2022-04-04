using System.ComponentModel;
using System.Windows;

namespace SongTheoryApplication.ViewModels.Base;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private int _height;

    private int _width;

    protected BaseViewModel()
    {
        ApplicationTitle = "Song Theory";
        Width = 1024;
        Height = 768;

        CenterScreen();
    }

    public int Width
    {
        get => _width;

        set
        {
            _width = value;
            RaisePropertyChanged(nameof(Width));
        }
    }

    public int Height
    {
        get => _height;

        set
        {
            _height = value;
            RaisePropertyChanged(nameof(Height));
        }
    }

    public int MinWidth { get; set; }

    public int MinHeight { get; set; }

    public int LeftWindow { get; set; }

    public int TopWindow { get; set; }

    public string ApplicationTitle { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void CenterScreen()
    {
        var screenWidth = (int) SystemParameters.PrimaryScreenWidth;
        var screenHeight = (int) SystemParameters.PrimaryScreenHeight;

        var windowWidth = Width;
        var windowHeight = Height;

        LeftWindow = screenWidth / 2 - windowWidth / 2;
        TopWindow = screenHeight / 2 - windowHeight / 2;
    }

    protected void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}