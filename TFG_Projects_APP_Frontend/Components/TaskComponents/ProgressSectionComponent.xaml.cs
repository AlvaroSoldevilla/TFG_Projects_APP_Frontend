using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class ProgressSectionComponent : ContentView
{
    public static readonly BindableProperty ProgressSectionProperty =
    BindableProperty.Create(nameof(TaskProgress), typeof(TaskProgress), typeof(ProgressSectionComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty ChildDragEndCommandProperty =
    BindableProperty.Create(nameof(ChildDragEndCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty HoverEnterCommandProperty =
    BindableProperty.Create(nameof(HoverEnterCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty HoverExitCommandProperty =
    BindableProperty.Create(nameof(HoverExitCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public TaskProgress ProgressSection
    {
        get => (TaskProgress)GetValue(ProgressSectionProperty);
        set => SetValue(ProgressSectionProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ICommand ChildDragEndCommand
    {
        get => (ICommand)GetValue(ChildDragEndCommandProperty);
        set => SetValue(ChildDragEndCommandProperty, value);
    }

    public ICommand ChildTapCommand
    {
        get => (ICommand)GetValue(ChildTapCommandProperty);
        set => SetValue(ChildTapCommandProperty, value);
    }

    public ICommand HoverEnterCommand
    {
        get => (ICommand)GetValue(HoverEnterCommandProperty);
        set => SetValue(HoverEnterCommandProperty, value);
    }

    public ICommand HoverExitCommand
    {
        get => (ICommand)GetValue(HoverExitCommandProperty);
        set => SetValue(HoverExitCommandProperty, value);
    }

    public ProgressSectionComponent()
	{
        InitializeComponent();
        BindingContext = this;
        AddGestures();
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProgressSectionComponent view && view.ProgressSection != null)
        {
            view.BindingContext = view;
        }
    }

    private void AddGestures()
    {
        throw new NotImplementedException();
    }
}