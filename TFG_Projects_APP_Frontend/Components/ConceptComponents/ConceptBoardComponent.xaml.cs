using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.ConceptComponents;

public partial class ConceptBoardComponent : ContentView
{
    public static readonly BindableProperty ComponentProperty =
    BindableProperty.Create(nameof(ComponentProperty), typeof(ConceptComponent), typeof(NoteComponent), default(ConceptComponent));

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty DoubleTapCommandProperty =
    BindableProperty.Create(nameof(DoubleTapCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

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

    public ICommand DoubleTapCommand
    {
        get => (ICommand)GetValue(DoubleTapCommandProperty);
        set => SetValue(DoubleTapCommandProperty, value);
    }

    public ICommand DragEndCommand
    {
        get => (ICommand)GetValue(DragEndCommandProperty);
        set => SetValue(DragEndCommandProperty, value);
    }

    private Point _startOffset;
    private DateTime _lastTapTime;
    private Point _position;

    public ConceptBoardComponent()
	{
		InitializeComponent();
        BindingContext = this;
        AddGestures();
    }

    private void AddGestures()
    {
        var tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
        tapGesture.Tapped += OnTapped;

        var doubleTapgesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
        doubleTapgesture.Tapped += OnDoubleTapped;

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        this.GestureRecognizers.Add(tapGesture);
        this.GestureRecognizers.Add(doubleTapgesture);
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

    private void OnTapped(object sender, EventArgs e)
    {
        var now = DateTime.Now;
        if ((now - _lastTapTime).TotalMilliseconds < 300)
        {
            OnDoubleTap(); // Fallback in case OS misses a double-tap
        }
        else
        {
            _lastTapTime = now;
            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                if ((DateTime.Now - _lastTapTime).TotalMilliseconds >= 300)
                {
                    OnSingleTap();
                }
                return false;
            });
        }
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        OnDoubleTap();
    }

    private void OnSingleTap()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(Component);
        }
    }

    private void OnDoubleTap()
    {
        if (DoubleTapCommand?.CanExecute(this) == true)
        {
            DoubleTapCommand.Execute(Component);
        }
    }

}