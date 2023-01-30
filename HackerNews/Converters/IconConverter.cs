using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace HackerNews.Converters;

public class IconConverter : IValueConverter
{
    public static IconConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string icon)
        {
            if (Application.Current?.Resources.TryGetResource(icon, out var resource) == true)
            {
                return resource;
            }
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
