using System.Windows;
using System.Windows.Controls;

namespace SongTheoryApplication.Views.Controls;

public partial class LoadingSpinnerControl : UserControl
{
    public static readonly DependencyProperty LoadingTitleProperty =
        DependencyProperty.Register(nameof(LoadingTitle), typeof(string), typeof(FooterControl),
            new PropertyMetadata("1.00"));

    public LoadingSpinnerControl()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     The loading title of the <see cref="LoadingSpinnerControl" />.
    /// </summary>
    public string LoadingTitle
    {
        get => (string)GetValue(LoadingTitleProperty);
        set => SetValue(LoadingTitleProperty, value);
    }
}