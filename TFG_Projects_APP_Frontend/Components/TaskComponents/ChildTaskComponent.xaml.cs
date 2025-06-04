using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class ChildTaskComponent : ContentView
{
    public static readonly BindableProperty ComponentTaskProperty =
    BindableProperty.Create(nameof(ComponentTaskProperty), typeof(ProjectTask), typeof(ChildTaskComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty EditCommandProperty =
    BindableProperty.Create(nameof(EditCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty HoverEnterCommandProperty =
    BindableProperty.Create(nameof(HoverEnterCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty HoverExitCommandProperty =
    BindableProperty.Create(nameof(HoverExitCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public ProjectTask ComponentTask
    {
        get => (ProjectTask)GetValue(ComponentTaskProperty);
        set => SetValue(ComponentTaskProperty, value);
    }

    public ICommand DragEndCommand
    {
        get => (ICommand)GetValue(DragEndCommandProperty);
        set => SetValue(DragEndCommandProperty, value);
    }

    public ICommand EditCommand
    {
        get => (ICommand)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
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

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }


    public ChildTaskComponent()
    {
        InitializeComponent();
        AddGestures();
        this.BindingContextChanged += (s, e) => OnBindingContextChanged();
    }

    private Point _startOffset;

    private void OnBindingContextChanged()
    {
        if (BindingContext is ProjectTask componentTask)
        {
            ComponentTask = componentTask;
        }
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TaskComponent view && newValue is ProjectTask)
        {
            view.BindingContext = view;
        }
    }

    private void AddGestures()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnTapped();

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        this.GestureRecognizers.Add(tapGesture);
        this.GestureRecognizers.Add(panGesture);

#if WINDOWS
        this.HandlerChanged += (s, e) =>
        {
            if (this.Handler?.PlatformView is Microsoft.UI.Xaml.FrameworkElement frameworkElement)
            {
                frameworkElement.PointerEntered += (sender, args) =>
                {
                    OnHoverEnter();
                };

                frameworkElement.PointerExited += (sender, args) =>
                {
                    OnHoverExit();
                };
            }
        };
#endif
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                var bounds = AbsoluteLayout.GetLayoutBounds(this);
                _startOffset = new Point(bounds.X, bounds.Y);
                break;

            case GestureStatus.Running:
                double runningX = _startOffset.X + e.TotalX;
                double runningY = _startOffset.Y + e.TotalY;
                AbsoluteLayout.SetLayoutBounds(this, new Rect(runningX, runningY, -1, -1));
                break;

            case GestureStatus.Completed:
                OnDragEnded();
                break;
        }
    }

    private void OnDragEnded()
    {

        if (DragEndCommand?.CanExecute(this) == true)
        {
            DragEndCommand.Execute(ComponentTask);
        }
    }

    private void OnTapped()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(ComponentTask);
        }
    }



    private void OnHoverExit()
    {
        if (HoverExitCommand?.CanExecute(ComponentTask) == true)
        {
            HoverExitCommand.Execute(ComponentTask);
        }
    }

    private void OnHoverEnter()
    {
        if (HoverEnterCommand?.CanExecute(ComponentTask) == true)
        {
            HoverEnterCommand.Execute(ComponentTask);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (DeleteCommand?.CanExecute(ComponentTask) == true)
        {
            DeleteCommand.Execute(ComponentTask);
        }
    }
}