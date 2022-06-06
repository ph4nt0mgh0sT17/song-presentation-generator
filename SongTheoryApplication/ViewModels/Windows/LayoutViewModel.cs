using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.ViewModels.Pages;

namespace SongTheoryApplication.ViewModels.Windows;

public partial class LayoutViewModel : ObservableObject
{
    [ObservableProperty]
    private object? _currentView = new TestPage1ViewModel();

    [ICommand]
    private void DisplayPageOne() => CurrentView = new TestPage1ViewModel();

    [ICommand]
    private void DisplayPageTwo()
    {
        CurrentView = new TestPage2ViewModel();
    }
}