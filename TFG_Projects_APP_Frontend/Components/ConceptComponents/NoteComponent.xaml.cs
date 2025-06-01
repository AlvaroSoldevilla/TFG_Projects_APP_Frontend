using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.ConceptComponents;

public partial class NoteComponent : ContentView
{
    public static readonly BindableProperty ComponentProperty =
    BindableProperty.Create(nameof(Component),typeof(ConceptComponent),typeof(ConceptBoardComponent),propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

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

    private Point _startOffset;
    private Point _position;

    public NoteComponent()
	{
		InitializeComponent();
        BindingContext = this;
        AddGestures();
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NoteComponent view)
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
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                var bounds = AbsoluteLayout.GetLayoutBounds(this); // "this" = the NoteComponent
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
}