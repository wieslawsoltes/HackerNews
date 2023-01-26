using System;
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

    protected override async void OnLoaded()
    {
        base.OnLoaded();

        AddHandler(Gestures.PullGestureEvent, OnPullGesture);
        AddHandler(Gestures.PullGestureEndedEvent, PullGestureEnded);

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
        
        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.LoadAsync();
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
        //Console.WriteLine($"PullGesture {e.Id} {e.PullDirection} {e.Delta}");
        switch (e.PullDirection)
        {
            case PullDirection.LeftToRight:
                // TODO: Show hamburger menu.
                e.Handled = true;
                break;
        }
    }

    private void PullGestureEnded(object? sender, PullGestureEndedEventArgs e)
    {
        //Console.WriteLine($"PullGestureEnded {e.Id} {e.PullDirection}");
        // TODO: Handle hamburger menu show/hide state.
    }

    private async void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        //Console.WriteLine("TopLevelOnBackRequested");
        if (DataContext is ILazyLoadable lazyLoadable)
        {
            var handled = await lazyLoadable.BackAsync();

            e.Handled = handled;
        }
    }
}

