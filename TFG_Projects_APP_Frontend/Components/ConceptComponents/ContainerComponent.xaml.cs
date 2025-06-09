using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.ConceptComponents;

public partial class ContainerComponent : ContentView
{
    public static readonly BindableProperty ComponentProperty =
    BindableProperty.Create(nameof(Component), typeof(ConceptComponent), typeof(ConceptBoardComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ContainerComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(ContainerComponent), default(ICommand));

    public static readonly BindableProperty ChildDragEndCommandProperty =
    BindableProperty.Create(nameof(ChildDragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty ChildDoubleTapCommandProperty =
    BindableProperty.Create(nameof(ChildDoubleTapCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

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

    public ICommand ChildDoubleTapCommand
    {
        get => (ICommand)GetValue(ChildDoubleTapCommandProperty);
        set => SetValue(ChildDoubleTapCommandProperty, value);
    }

    private Point _startOffset;
    private Point _position;


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
                                DragEndCommand = new Command<ConceptComponent>(view.DropComponent),
                                DoubleTapCommand = new Command<ConceptComponent>(view.RemoveContainer)
                            },
                        };
                    }

                    if (renderedChild != null)
                    {
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
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                var bounds = AbsoluteLayout.GetLayoutBounds(this);
                _startOffset = new Point(bounds.X, bounds.Y);
                _position = new Point(bounds.X, bounds.Y);
                this.ZIndex = 1;
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
                this.ZIndex = 0;
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
        return;
    }

    private void RemoveContainer(ConceptComponent component)
    {
        if (ChildDoubleTapCommand?.CanExecute(component) == true)
        {
            ChildDoubleTapCommand.Execute(component);
        }
    }

}