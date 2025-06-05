using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class ChildTaskComponent : ContentView
{
    public static readonly BindableProperty ComponentTaskProperty =
    BindableProperty.Create(nameof(ComponentTaskProperty), typeof(ProjectTask), typeof(ChildTaskComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand)); //

    public static readonly BindableProperty DroppedOnTaskCommandProperty =
    BindableProperty.Create(nameof(DroppedOnTaskCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand));

    public static readonly BindableProperty TaskGrabbedCommandProperty =
    BindableProperty.Create(nameof(TaskGrabbedCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand)); //

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(ChildTaskComponent), default(ICommand)); //

    public ProjectTask ComponentTask
    {
        get => (ProjectTask)GetValue(ComponentTaskProperty);
        set => SetValue(ComponentTaskProperty, value);
    }

    public ICommand DroppedOnTaskCommand
    {
        get => (ICommand)GetValue(DroppedOnTaskCommandProperty);
        set => SetValue(DroppedOnTaskCommandProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ICommand TaskGrabbedCommand
    {
        get => (ICommand)GetValue(TaskGrabbedCommandProperty);
        set => SetValue(TaskGrabbedCommandProperty, value);
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
        this.GestureRecognizers.Add(tapGesture);
    }

    private void OnTapped()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(ComponentTask);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (DeleteCommand?.CanExecute(ComponentTask) == true)
        {
            DeleteCommand.Execute(ComponentTask);
        }
    }

    private void OnTaskGrabbed(object sender, DragStartingEventArgs e)
    {
        if (TaskGrabbedCommand?.CanExecute(ComponentTask) == true)
        {
            TaskGrabbedCommand.Execute(ComponentTask);
        }
    }

    private void OnDroppedOnTask(object sender, DragStartingEventArgs e)
    {
        if (DroppedOnTaskCommand?.CanExecute(ComponentTask) == true)
        {
            DroppedOnTaskCommand.Execute(ComponentTask);
        }
    }
}