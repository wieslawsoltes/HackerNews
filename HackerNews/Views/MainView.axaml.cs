using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using HackerNews.Model;
using HackerNews.ViewModels;

namespace HackerNews.Views;

public partial class MainView : UserControl
{
    private Vector _delta;

    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        AddHandler(Gestures.PullGestureEvent, OnPullGesture);
        AddHandler(Gestures.PullGestureEndedEvent, PullGestureEnded);

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);

        RemoveHandler(Gestures.PullGestureEvent, OnPullGesture);
        RemoveHandler(Gestures.PullGestureEndedEvent, PullGestureEnded);

        if (VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
        }
    }

    private async void OnPullGesture(object? sender, PullGestureEventArgs e)
    {
        // Console.WriteLine($"PullGesture {e.Id} {e.PullDirection} {e.Delta}");

        switch (e.PullDirection)
        {
            case PullDirection.LeftToRight:
            {
                _delta = e.Delta;

                // TODO: Show hamburger menu.
                // SplitView.SetValue(SplitView.IsPaneOpenProperty, true);
                // e.Handled = true;

                /*
                if (DataContext is MainViewViewModel mainViewViewModel && mainViewViewModel.Navigation is { } navigation)
                {
                    if (navigation.CanGoBack)
                    {
                        if (DataContext is ILazyLoadable lazyLoadable)
                        {
                            var handled = await lazyLoadable.BackAsync();
                            e.Handled = handled;
                            return;
                        }
                    }
                }
                */

                break;
            }
            case PullDirection.RightToLeft:
            {
                _delta = e.Delta;

                // TODO: Hide hamburger menu.
                // SplitView.SetValue(SplitView.IsPaneOpenProperty, false);
                // e.Handled = true;
                
                /*
                if (DataContext is MainViewViewModel mainViewViewModel && mainViewViewModel.Navigation is { } navigation)
                {
                    if (navigation.CanGoBack)
                    {
                        // TODO:
                    }
                }
                */

                break;
            }

            case PullDirection.TopToBottom:
                _delta = new Vector();
                break;
            case PullDirection.BottomToTop:
                _delta = new Vector();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async void PullGestureEnded(object? sender, PullGestureEndedEventArgs e)
    {
        // Console.WriteLine($"PullGestureEnded {e.Id} {e.PullDirection}");
        // TODO: Handle hamburger menu show/hide state.

        switch (e.PullDirection)
        {
            case PullDirection.LeftToRight:
            {
                if (DataContext is MainViewViewModel mainViewViewModel && mainViewViewModel.Navigation is { } navigation)
                {
                    if (_delta.X < 30)
                    {
                        return;
                    }

                    if (navigation.CanGoBack)
                    {
                        if (DataContext is ILazyLoadable lazyLoadable)
                        {
                            var handled = await lazyLoadable.BackAsync();
                            e.Handled = handled;
                            return;
                        }
                    }

                    if (!navigation.CanGoBack)
                    {
                        if (mainViewViewModel.CurrentFeed is not null && mainViewViewModel.Feeds is not null)
                        {
                            var index = mainViewViewModel.Feeds.IndexOf(mainViewViewModel.CurrentFeed);
                            var totalFeeds = mainViewViewModel.Feeds.Count;

                            var nextIndex = index - 1;
                            if (nextIndex >= 0)
                            {
                                e.Handled = true;
                                _delta = new Vector();
                                await mainViewViewModel.OpenFeedAsync(mainViewViewModel.Feeds[nextIndex]);
                            }
                            else
                            {
                                e.Handled = true;
                                _delta = new Vector();
                                await mainViewViewModel.OpenFeedAsync(mainViewViewModel.Feeds[totalFeeds - 1]); 
                            }
                        }
                    }
                }

                break;
            }
            case PullDirection.RightToLeft:
            {
                if (DataContext is MainViewViewModel mainViewViewModel && mainViewViewModel.Navigation is { } navigation)
                {
                    if (!navigation.CanGoBack)
                    {
                        if (_delta.X < 30)
                        {
                            return;
                        }

                        if (mainViewViewModel.CurrentFeed is not null && mainViewViewModel.Feeds is not null)
                        {
                            var index = mainViewViewModel.Feeds.IndexOf(mainViewViewModel.CurrentFeed);
                            var totalFeeds = mainViewViewModel.Feeds.Count;

                            var nextIndex = index + 1;
                            if (nextIndex < totalFeeds)
                            {
                                e.Handled = true;
                                _delta = new Vector();
                                await mainViewViewModel.OpenFeedAsync(mainViewViewModel.Feeds[nextIndex]);
                                return;
                            }
                            else
                            {
                                e.Handled = true;
                                _delta = new Vector();
                                await mainViewViewModel.OpenFeedAsync(mainViewViewModel.Feeds[0]); 
                                return;
                            }
                        }
                    }
                }

                break;
            }
            case PullDirection.TopToBottom:
                break;
            case PullDirection.BottomToTop:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

