using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace SongTheoryApplication.Converters;

public class ReversedBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool val)
        {
            return !val;
        }

        throw new InvalidOperationException(
            "The ReversedBoolConverter only accepts bool values to convert it into reversed bool objects."
        );
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}