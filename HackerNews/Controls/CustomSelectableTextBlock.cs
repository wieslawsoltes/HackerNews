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
    public static readonly StyledProperty<bool> IsSelectionEnabledProperty = 
        AvaloniaProperty.Register<CustomSelectableTextBlock, bool>(nameof(IsSelectionEnabled), true);

    public bool IsSelectionEnabled
    {
        get => GetValue(IsSelectionEnabledProperty);
        set => SetValue(IsSelectionEnabledProperty, value);
    }

    Type IStyleable.StyleKey => typeof(SelectableTextBlock);

    private bool IsInputElement(PointerEventArgs e)
    {
        return this.GetVisualsAt(e.GetPosition(this)).FirstOrDefault() is IInputElement inputElement 
               && !Equals(inputElement, this);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (!IsInputElement(e) && IsSelectionEnabled)
        {
            base.OnPointerPressed(e);
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (!IsInputElement(e) && IsSelectionEnabled)
        {
            base.OnPointerMoved(e);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!IsInputElement(e) && IsSelectionEnabled)
        {
            base.OnPointerReleased(e);
        }
    }
}
