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
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.ViewModels.Pages;
using SongTheoryApplication.ViewModels.Windows;

namespace SongTheoryApplication.Views.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class LayoutWindow : Window
    {
        public LayoutWindow()
        {
            InitializeComponent();
            DataContext = new LayoutViewModel();
        }
    }
}
