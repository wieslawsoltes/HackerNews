using Avalonia.Controls;
using HackerNews.Model;

namespace HackerNews.Views;

public partial class ItemsView : UserControl
{
    public ItemsView()
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
}

