using System;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Input;
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

        AddHandler(Gestures.PullGestureEvent, OnPullGesture);
        AddHandler(Gestures.PullGestureEndedEvent, PullGestureEnded);

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
    }

    protected override void OnUnloaded()
    {
        base.OnUnloaded();

        RemoveHandler(Gestures.PullGestureEvent, OnPullGesture);
        RemoveHandler(Gestures.PullGestureEndedEvent, PullGestureEnded);

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
        }
    }

    private void OnPullGesture(object? sender, PullGestureEventArgs e)
    {
        Console.WriteLine($"PullGesture {e.Id} {e.PullDirection} {e.Delta}");
    }

    private void PullGestureEnded(object? sender, PullGestureEndedEventArgs e)
    {
        Console.WriteLine($"PullGestureEnded {e.Id} {e.PullDirection}");
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

