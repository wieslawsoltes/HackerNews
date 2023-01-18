﻿using System;
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
        Console.WriteLine("Refresh_OnRefreshRequested");
        var deferral = e.GetDeferral();

        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.Load();
        }

        deferral.Complete();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        if (this.VisualRoot is TopLevel topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
    }

    private async void TopLevelOnBackRequested(object? sender, RoutedEvent e)
    {
        if (DataContext is ILazyLoadable lazyLoadable)
        {
            await lazyLoadable.Back();
        }
    }
}

