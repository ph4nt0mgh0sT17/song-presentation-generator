using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SongTheoryApplication.Views.Controls;

public partial class FooterControl : UserControl
{
    public FooterControl()
    {
        InitializeComponent();

        if (DesignerProperties.GetIsInDesignMode(this))
            FooterWidth = 800;

        DataContext = this;
    }

    public string FooterVersion
    {
        get => (string)GetValue(FooterVersionProperty);
        set => SetValue(FooterVersionProperty, value);
    }

    public string ApplicationAuthor
    {
        get => (string)GetValue(ApplicationAuthorProperty);
        set => SetValue(ApplicationAuthorProperty, value);
    }

    public int FooterWidth
    {
        get => (int)GetValue(FooterWidthProperty);
        set => SetValue(FooterWidthProperty, value);
    }

    public static readonly DependencyProperty FooterVersionProperty =
        DependencyProperty.Register(nameof(FooterVersion), typeof(string), typeof(FooterControl), new PropertyMetadata("1.00"));
    
    public static readonly DependencyProperty ApplicationAuthorProperty =
        DependencyProperty.Register(nameof(ApplicationAuthor), typeof(string), typeof(FooterControl), new PropertyMetadata("Author name"));
    
    public static readonly DependencyProperty FooterWidthProperty =
        DependencyProperty.Register(nameof(FooterWidth), typeof(int), typeof(FooterControl), new PropertyMetadata(0));
}