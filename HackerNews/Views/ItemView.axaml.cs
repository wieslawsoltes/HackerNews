using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HackerNews.Views;

public partial class ItemView : UserControl
{
    public ItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

