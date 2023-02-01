using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace HackerNews.Converters;

public class IndentConverter : IValueConverter
{
    public static IndentConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int level)
        {
            return new Thickness(6 * level, 0, 0, 0);
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
