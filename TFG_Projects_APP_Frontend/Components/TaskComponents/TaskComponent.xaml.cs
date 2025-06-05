using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class TaskComponent : ContentView
{
    public static readonly BindableProperty ComponentTaskProperty =
    BindableProperty.Create(nameof(ComponentTaskProperty), typeof(ProjectTask), typeof(TaskComponent), propertyChanged: OnComponentChanged);

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



    public ProjectTask ComponentTask
    {
        get => (ProjectTask)GetValue(ComponentTaskProperty);
        set => SetValue(ComponentTaskProperty, value);
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
            view.TaskContainer.Children.Clear();

            if (view.ComponentTask.Children != null && view.ComponentTask.Children.Count > 0)
            {
                foreach (var task in view.ComponentTask.Children)
                {
                    ContentView renderedChild = new ChildTaskComponent
                    {
                        ComponentTask = task,
                        TapCommand = new Command<ProjectTask>(view.ChildTapped),
                        DroppedOnTaskCommand = new Command<ProjectTask>(view.ChildDroppedOnTask),
                        TaskGrabbedCommand = new Command<ProjectTask>(view.ChildTaskGrabbed),
                        DeleteCommand = new Command<ProjectTask>(view.ChildDeleted),
                    };

                    var abs_layout = new AbsoluteLayout();
                    abs_layout.Children.Add(renderedChild);
                    view.TaskContainer.Children.Add(abs_layout);
                }
            }
        }
    }

    private void AddGestures()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnTapped();
        this.GestureRecognizers.Add(tapGesture);
    }

    private void OnTaskGrabbed(object sender, DragStartingEventArgs e)
    {
        if (TaskGrabbedCommand?.CanExecute(ComponentTask) == true)
        {
            TaskGrabbedCommand.Execute(ComponentTask);
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

    private void OnDroppedOnTask(object sender, DropEventArgs e)
    {
        if (DroppedOnTaskCommand?.CanExecute(ComponentTask) == true)
        {
            DroppedOnTaskCommand.Execute(ComponentTask);
        }
    }
}