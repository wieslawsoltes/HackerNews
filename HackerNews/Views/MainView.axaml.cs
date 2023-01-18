using System;
using Avalonia.Controls;
using HackerNews.ViewModels;

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

        if (DataContext is MainViewViewModel mainViewViewModel)
        {
            await mainViewViewModel.LoadItems();
        }

        deferral.Complete();
    }
}

