using Avalonia.Controls;
using Avalonia.Interactivity;
using HackerNews.Model;

namespace HackerNews.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
    }

    protected override void OnUnloaded()
    {
        base.OnUnloaded();

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
        }
    }

    private async void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        //Console.WriteLine("TopLevelOnBackRequested");
        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.BackAsync();
        }
    }
}

