using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SongTheoryApplication.ViewModels.Base;

public abstract class BaseViewModel : ObservableObject
{
    private int _width;
    private int _height;
    private int _minWidth;
    private int _minHeight;

    private int _leftWindow;
    private int _topWindow;

    private string _applicationTitle;

    protected BaseViewModel()
    {
        _applicationTitle = "Song Theory";
        Width = 1024;
        Height = 768;

        CenterScreen();
    }
    
    public void CenterScreen()
    {
        var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
        var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

        var windowWidth = Width;
        var windowHeight = Height;

        LeftWindow = (screenWidth / 2) - (windowWidth / 2);
        TopWindow = (screenHeight / 2) - (windowHeight / 2);
    }
    
    public int Width
    {
        get => _width;

        set
        {
            _width = value;
            SetProperty(ref _width, value);
        }
    }
    
    public int Height
    {
        get => _height;

        set
        {
            _height = value;
            SetProperty(ref _height, value);
        }
    }
    
    public int MinWidth
    {
        get => _minWidth;
        set => _minWidth = value;
    }
    
    public int MinHeight
    {
        get => _minHeight;
        set => _minHeight = value;
    }
    
    public int LeftWindow
    {
        get => _leftWindow;
        set => _leftWindow = value;
    }
    
    public int TopWindow
    {
        get => _topWindow;
        set => _topWindow = value;
    }
    
    public string ApplicationTitle
    {
        get => _applicationTitle;
        set => _applicationTitle = value;
    }
}