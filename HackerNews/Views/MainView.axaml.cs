using System;
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

    private async void Refresh_OnRefreshRequested(object? sender, RefreshRequestedEventArgs e)
    {
        //Console.WriteLine("Refresh_OnRefreshRequested");
        var deferral = e.GetDeferral();

        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.LoadAsync();
        }

        deferral.Complete();
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

    private async void TopLevelOnBackRequested(object? sender, RoutedEvent e)
    {
        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.BackAsync();
        }
    }
}

