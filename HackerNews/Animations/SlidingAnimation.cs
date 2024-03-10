using System;
using System.Globalization;
using System.Numerics;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Reactive;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;

namespace HackerNews.Animations;

public static class SlidingAnimation
{
    public static void SetLeft(Control element, double milliseconds)
    {
        element.GetObservable(Control.TagProperty)
            .Subscribe(new AnonymousObserver<object?>(x =>
            {
                if (x is true)
                {
                    ApplyShow(element, -element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
                }
                else if (x is false)
                {
                    ApplyHide(element, -element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
                }
            }));

        element.Loaded += (_, _) =>
        {
            ApplyShow(element, -element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
        };

        element.Unloaded += (_, _) =>
        {
            
        };
    }

    public static void SetRight(Control element, double milliseconds)
    {
        element.GetObservable(Control.TagProperty)
            .Subscribe(new AnonymousObserver<object?>(x =>
            {
                if (x is true)
                {
                    ApplyShow(element, 2 * element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
                }
                else if (x is false)
                {
                    ApplyHide(element, element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
                }
            }));

        element.Loaded += (_, _) =>
        {
            ApplyShow(element, 2 * element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
        };

        element.Unloaded += (_, _) =>
        {
            
        };
    }

    public static void SetTop(Control element, double milliseconds)
    {
        element.GetObservable(Control.TagProperty)
            .Subscribe(new AnonymousObserver<object?>(x =>
            {
                if (x is true)
                {
                    ApplyShow(element, 0, -element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
                }
                else if (x is false)
                {
                    ApplyHide(element, 0, -element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
                }
            }));

        element.Loaded += (_, _) =>
        {
            ApplyShow(element, 0, -element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
        };

        element.Unloaded += (_, _) =>
        {
            
        };
    }

    public static void SetBottom(Control element, double milliseconds)
    {
        element.GetObservable(Control.TagProperty)
            .Subscribe(new AnonymousObserver<object?>(x =>
            {
                if (x is true)
                {
                    ApplyShow(element, 0, 2 * element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
                }
                else if (x is false)
                {
                    ApplyHide(element, 0, element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
                }
            }));

        element.Loaded += (_, _) =>
        {
            ApplyShow(element, 0, 2 * element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
        };

        element.Unloaded += (_, _) =>
        {
            
        };
    }

    private static void ApplyShow(Visual visual, double offsetX, double offsetY, TimeSpan duration)
    {
        var compositionVisual = ElementComposition.GetElementVisual(visual);
        if (compositionVisual is null)
        {
            return;
        }

        var compositor = compositionVisual.Compositor;

        var group = compositor.CreateAnimationGroup();
        
        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.InsertKeyFrame(0.0f, new Vector3((float)offsetX, (float)offsetY, 0));
        offsetAnimation.InsertKeyFrame(1.0f, new Vector3(0, 0, 0));
        offsetAnimation.Direction = PlaybackDirection.Normal;
        offsetAnimation.Duration = duration;
        offsetAnimation.IterationBehavior = AnimationIterationBehavior.Count;
        offsetAnimation.IterationCount = 1;
        
        offsetAnimation.Target = "Offset";

        // compositionVisual.StartAnimation("Offset", offsetAnimation);
        
        group.Add(offsetAnimation);
        
        var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
        opacityAnimation.InsertKeyFrame(0.0f, 0.0f);
        opacityAnimation.InsertKeyFrame(1.0f, 1.0f);
        opacityAnimation.Direction = PlaybackDirection.Normal;
        opacityAnimation.Duration = duration;
        opacityAnimation.IterationBehavior = AnimationIterationBehavior.Count;
        opacityAnimation.IterationCount = 1;

        opacityAnimation.Target = "Opacity";

        // compositionVisual.StartAnimation("Opacity", opacityAnimation);

        group.Add(opacityAnimation);
        
        compositionVisual.StartAnimationGroup(group);
    }

    private static void ApplyHide(Visual visual, double offsetX, double offsetY, TimeSpan duration)
    {
        var compositionVisual = ElementComposition.GetElementVisual(visual);
        if (compositionVisual is null)
        {
            return;
        }

        var compositor = compositionVisual.Compositor;

        var group = compositor.CreateAnimationGroup();

        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.InsertKeyFrame(0.0f, new Vector3(0, 0, 0));
        offsetAnimation.InsertKeyFrame(1.0f, new Vector3((float)offsetX, (float)offsetY, 0));
        offsetAnimation.Direction = PlaybackDirection.Normal;
        offsetAnimation.Duration = duration;
        offsetAnimation.IterationBehavior = AnimationIterationBehavior.Count;
        offsetAnimation.IterationCount = 1;

        offsetAnimation.Target = "Offset";

        group.Add(offsetAnimation);

        // compositionVisual.StartAnimation("Offset", offsetAnimation);

        var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
        opacityAnimation.InsertKeyFrame(0.0f, 1.0f);
        opacityAnimation.InsertKeyFrame(1.0f, 0.0f);
        opacityAnimation.Direction = PlaybackDirection.Normal;
        opacityAnimation.Duration = duration;
        opacityAnimation.IterationBehavior = AnimationIterationBehavior.Count;
        opacityAnimation.IterationCount = 1;

        opacityAnimation.Target = "Opacity";

        // compositionVisual.StartAnimation("Opacity", opacityAnimation);

        group.Add(opacityAnimation);

        compositionVisual.StartAnimationGroup(group);
    }
}
