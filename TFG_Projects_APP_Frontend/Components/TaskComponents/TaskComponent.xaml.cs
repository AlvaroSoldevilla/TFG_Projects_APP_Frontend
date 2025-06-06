using Microsoft.Maui.Controls.Internals;
using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

[Preserve(AllMembers = true)]
public partial class TaskComponent : ContentView
{

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty TaskGrabbedCommandProperty =
    BindableProperty.Create(nameof(TaskGrabbedCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty DroppedOnTaskCommandProperty =
    BindableProperty.Create(nameof(DroppedOnTaskCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildTaskGrabbedCommandProperty =
    BindableProperty.Create(nameof(ChildTaskGrabbedCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildDroppedOnTaskCommandProperty =
    BindableProperty.Create(nameof(ChildDroppedOnTaskCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildDeleteCommandProperty =
    BindableProperty.Create(nameof(ChildDeleteCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

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

    public ICommand DroppedOnTaskCommand
    {
        get => (ICommand)GetValue(DroppedOnTaskCommandProperty);
        set => SetValue(DroppedOnTaskCommandProperty, value);
    }

    public ICommand ChildDroppedOnTaskCommand
    {
        get => (ICommand)GetValue(ChildDroppedOnTaskCommandProperty);
        set => SetValue(ChildDroppedOnTaskCommandProperty, value);
    }

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public ICommand ChildDeleteCommand
    {
        get => (ICommand)GetValue(ChildDeleteCommandProperty);
        set => SetValue(ChildDeleteCommandProperty, value);
    }

    public ICommand ChildTaskGrabbedCommand
    {
        get => (ICommand)GetValue(ChildTaskGrabbedCommandProperty);
        set => SetValue(ChildTaskGrabbedCommandProperty, value);
    }

    public ICommand ChildTapCommand
    {
        get => (ICommand)GetValue(ChildTapCommandProperty);
        set => SetValue(ChildTapCommandProperty, value);
    }

    public TaskComponent()
	{
        InitializeComponent();
        AddGestures();
    }

    private void AddGestures()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnTapped();
        this.GestureRecognizers.Add(tapGesture);
    }

    private void OnTaskGrabbed(object sender, DragStartingEventArgs e)
    {
        if (BindingContext is ProjectTask componentTask && TaskGrabbedCommand?.CanExecute(componentTask) == true)
        {
            TaskGrabbedCommand.Execute(componentTask);
        }
    }

    private void ChildTapped(ProjectTask projectTask)
    {
        if (ChildTapCommand?.CanExecute(projectTask) == true)
        {
            ChildTapCommand.Execute(projectTask);
        }
    }

    private void ChildDroppedOnTask(ProjectTask projectTask)
    {
        if (ChildDroppedOnTaskCommand?.CanExecute(projectTask) == true)
        {
            ChildDroppedOnTaskCommand.Execute(projectTask);
        }
    }

    private void ChildDeleted(ProjectTask projectTask)
    {
        if (ChildDeleteCommand?.CanExecute(projectTask) == true)
        {
            ChildDeleteCommand.Execute(projectTask);
        }
    }

    private void ChildTaskGrabbed(ProjectTask projectTask)
    {
        if (ChildTaskGrabbedCommand?.CanExecute(projectTask) == true)
        {
            ChildTaskGrabbedCommand.Execute(projectTask);
        }
    }

    private void OnTapped()
    {
        if (BindingContext is ProjectTask componentTask && TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(componentTask);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ProjectTask componentTask && DeleteCommand?.CanExecute(componentTask) == true)
        {
            DeleteCommand.Execute(componentTask);
        }
    }

    private void OnDroppedOnTask(object sender, DropEventArgs e)
    {
        if (BindingContext is ProjectTask componentTask && DroppedOnTaskCommand?.CanExecute(componentTask) == true)
        {
            DroppedOnTaskCommand.Execute(componentTask);
        }
    }
}