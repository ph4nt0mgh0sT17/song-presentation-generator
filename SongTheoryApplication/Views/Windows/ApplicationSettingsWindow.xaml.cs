using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.DependencyInjection;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows;
/// <summary>
/// Interaction logic for ApplicationSettingsWindow.xaml
/// </summary>
public partial class ApplicationSettingsWindow : Window
{
    public ApplicationSettingsWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService(typeof(ApplicationSettingsViewModel));
    }

    public ApplicationSettingsViewModel ViewModel
    {
        get
        {
            if (DataContext is ApplicationSettingsViewModel applicationSettingsViewModel)
                return applicationSettingsViewModel;

            throw new InvalidOperationException("The DataContext of this window must be ApplicationSettingsViewModel.");
        }
    }
}
