using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Styling;
using HackerNews.Model;

namespace HackerNews.Controls;

public class ItemsListBox : ListBox, IStyleable
{
    Type IStyleable.StyleKey => typeof(ListBox);

    protected override Control CreateContainerForItemOverride()
    {
        return new ListBoxItem();
    }

    protected override void PrepareContainerForItemOverride(Control element, object? item, int index)
    {
        base.PrepareContainerForItemOverride(element, item, index);

        Load(element);
    }

    protected override void ClearContainerForItemOverride(Control element)
    {
        base.ClearContainerForItemOverride(element);

        // TODO:
    }

    private void Load(Control element)
    {
        if (element is ListBoxItem { DataContext: ILazyLoadable lazyLoadable })
        {
            if (!lazyLoadable.IsLoaded())
            {
                //Console.WriteLine($"LoadAsync: {lazyLoadable}");
                Task.Run(async () =>
                {
                    await lazyLoadable.LoadAsync();
                    await lazyLoadable.UpdateAsync();
                    //Console.WriteLine($"Loaded: {lazyLoadable}");
                });
            }
            else
            {
                // TODO: Check if we need to update.
                //Console.WriteLine($"UpdateAsync: {lazyLoadable}");
                Task.Run(async () =>
                {
                    await lazyLoadable.UpdateAsync();
                });
            }
        }
    }
}

