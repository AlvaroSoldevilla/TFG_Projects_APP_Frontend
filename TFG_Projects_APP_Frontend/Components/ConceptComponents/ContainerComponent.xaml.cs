using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

#if WINDOWS
using Microsoft.UI.Xaml;
#endif

namespace TFG_Projects_APP_Frontend.Components.ConceptComponents;

public partial class ContainerComponent : ContentView
{
    public static readonly BindableProperty ComponentProperty =
    BindableProperty.Create(nameof(Component), typeof(ConceptComponent), typeof(ConceptBoardComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty ChildDragEndCommandProperty =
    BindableProperty.Create(nameof(ChildDragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty HoverEnterCommandProperty =
    BindableProperty.Create(nameof(HoverEnterCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty HoverExitCommandProperty =
    BindableProperty.Create(nameof(HoverExitCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public ConceptComponent Component
    {
        get => (ConceptComponent)GetValue(ComponentProperty);
        set => SetValue(ComponentProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ICommand DragEndCommand
    {
        get => (ICommand)GetValue(DragEndCommandProperty);
        set => SetValue(DragEndCommandProperty, value);
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

    private Point _startOffset;
    private Point _position;
    private bool _suppressPan = false;


    public ContainerComponent()
	{
        InitializeComponent();
        BindingContext = this;
        AddGestures();
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ContainerComponent view && view.Component != null)
        {
            view.BindingContext = view;
            view.ChildrenContainer.Children.Clear();

            if (view.Component.Children == null || view.Component.Children.Count == 0)
            {
                var line = new BoxView
                {
                    Color = Colors.GhostWhite,
                    CornerRadius = 30,
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center
                };
                view.ChildrenContainer.Children.Add(line);
            } else
            {
                foreach (var child in view.Component.Children)
                {
                    ContentView renderedChild = null;
                    if (child.IdType == 2)
                    {
                        renderedChild = child.IdType switch
                        {
                            2 => new NoteComponent
                            {
                                Component = child,
                                TapCommand = new Command<ConceptComponent>(view.EditNote),
                                DragEndCommand = new Command<ConceptComponent>(view.DropComponent)
                            },
                        };

                        if (renderedChild is NoteComponent note)
                        {
                            note.DragStarted += (_, __) => view._suppressPan = true;
                            note.DragEnded += (_, __) => view._suppressPan = false;
                        }
                    }

                    if (renderedChild != null)
                    {
                        // Pass commands if needed
                        renderedChild.SetValue(TableComponent.DragEndCommandProperty, view.ChildDragEndCommand);
                        view.ChildrenContainer.Children.Add(renderedChild);
                    }
                }
            }
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
        if (_suppressPan)
            return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                var bounds = AbsoluteLayout.GetLayoutBounds(this);
                _startOffset = new Point(bounds.X, bounds.Y);
                _position = new Point(bounds.X, bounds.Y);
                break;

            case GestureStatus.Running:
                double runningX = _startOffset.X + e.TotalX;
                _position.X = runningX;
                double runningY = _startOffset.Y + e.TotalY;
                _position.Y = runningY;
                AbsoluteLayout.SetLayoutBounds(this, new Rect(runningX, runningY, -1, -1));
                break;

            case GestureStatus.Completed:
                double finalX = _position.X;
                double finalY = _position.Y;
                AbsoluteLayout.SetLayoutBounds(this, new Rect(finalX, finalY, -1, -1));
                OnDragEnded(finalX, finalY);
                break;
        }
    }

    private void OnDragEnded(double x, double y)
    {

        if (DragEndCommand?.CanExecute(this) == true)
        {
            Component.PosX = x;
            Component.PosY = y;
            DragEndCommand.Execute(Component);
        }
    }

    private void OnTapped()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(Component);
        }
    }

    private void EditNote(ConceptComponent component)
    {
        if (ChildTapCommand?.CanExecute(component) == true)
        {
            ChildTapCommand.Execute(component);
        }
    }

    private void DropComponent(ConceptComponent component)
    {
        if (ChildDragEndCommand?.CanExecute(component) == true)
        {
            ChildDragEndCommand.Execute(component);
        }
    }

    private void OnHoverEnter()
    {
        if (HoverEnterCommand?.CanExecute(this) == true)
        {
            HoverEnterCommand.Execute(Component);
        }
    }

    private void OnHoverExit()
    {
        if (HoverExitCommand?.CanExecute(this) == true)
        {
            HoverExitCommand.Execute(Component);
        }
    }

}