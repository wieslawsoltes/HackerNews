using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace HackerNews.Controls;

public class CustomSelectableTextBlock : SelectableTextBlock, IStyleable
{
    Type IStyleable.StyleKey => typeof(SelectableTextBlock);

    private bool IsInputElement(PointerEventArgs e)
    {
        return this.GetVisualsAt(e.GetPosition(this)).FirstOrDefault() is IInputElement inputElement 
               && !Equals(inputElement, this);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (!IsInputElement(e))
        {
            base.OnPointerPressed(e);
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (!IsInputElement(e))
        {
            base.OnPointerMoved(e);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!IsInputElement(e))
        {
            base.OnPointerReleased(e);
        }
    }
}
