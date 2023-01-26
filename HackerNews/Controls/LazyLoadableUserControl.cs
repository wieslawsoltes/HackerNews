using System.Threading.Tasks;
using Avalonia.Controls;
using HackerNews.Model;

namespace HackerNews.Controls;

public class LazyLoadableUserControl : UserControl
{
    protected override async void OnLoaded()
    {
        base.OnLoaded();

        await Load(this);
    }

    private async Task Load(Control element)
    {
        if (DataContext is ILazyLoadable lazyLoadable)
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
