using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Styling;
using HackerNews.ViewModels;

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

        Load(element);
    }

    private void Load(Control element)
    {
        if (element is ListBoxItem listBoxItem)
        {
            if (listBoxItem.DataContext is ItemViewModel itemViewModel)
            {
                if (!itemViewModel.IsLoaded())
                {
                    //Console.WriteLine($"Load: {itemViewModel.Index}");
                    Task.Run(async () =>
                    {
                        await itemViewModel.Load();
                        itemViewModel.Update();
                        //Console.WriteLine($"Loaded: {itemViewModel.Index}");
                    });
                }
                else
                {
                    // TODO: Check if we need to update.
                    //Console.WriteLine($"Update: {itemViewModel.Index}");
                    itemViewModel.Update();
                }
            }
        }
    }
}

