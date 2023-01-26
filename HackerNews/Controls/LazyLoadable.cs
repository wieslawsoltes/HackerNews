using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HackerNews.Model;

namespace HackerNews.Controls;

public class LazyLoadable
{
    public static readonly AttachedProperty<bool> EnableProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>("Enable", typeof(LazyLoadable));

    public static bool GetEnable(Control element)
    {
        return element.GetValue(EnableProperty);
    }

    public static void SetEnable(Control element, bool value)
    {
        element.SetValue(EnableProperty, value);
    }

    static LazyLoadable()
    {
        EnableProperty.Changed.AddClassHandler<Control>(OnEnableChanged);
    }

    private static void OnEnableChanged(Control control, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is true)
        {
            control.DataContextChanged += ControlOnDataContextChanged;
        }
        else
        {
            control.DataContextChanged -= ControlOnDataContextChanged;
        }
    }

    private static async void ControlOnDataContextChanged(object? sender, EventArgs e)
    {
        if (sender is Control control)
        {
            //Console.WriteLine($"DataContextChanged: {control.DataContext}");
            await Load(control);
        }
    }

    private static async Task Load(Control control)
    {
        if (control.DataContext is ILazyLoadable lazyLoadable)
        {
            if (!lazyLoadable.IsLoaded())
            {
                //Console.WriteLine($"LoadAsync: {lazyLoadable}");
                await lazyLoadable.LoadAsync();
                await lazyLoadable.UpdateAsync();
                //Console.WriteLine($"Loaded: {lazyLoadable}");
            }
            else
            {
                // TODO: Check if we need to update.
                //Console.WriteLine($"UpdateAsync: {lazyLoadable}");
                await lazyLoadable.UpdateAsync();
                //Console.WriteLine($"Updated: {lazyLoadable}");
            }
        }
    }
}
