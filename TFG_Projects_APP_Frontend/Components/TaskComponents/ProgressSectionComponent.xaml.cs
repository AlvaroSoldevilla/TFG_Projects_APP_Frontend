using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class ProgressSectionComponent : ContentView
{
    public static readonly BindableProperty EditCommandProperty =
    BindableProperty.Create(nameof(EditCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty DroppedOnProgressCommandProperty =
    BindableProperty.Create(nameof(DroppedOnProgressCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveLeftCommandProperty =
    BindableProperty.Create(nameof(MoveLeftCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveRightCommandProperty =
    BindableProperty.Create(nameof(MoveRightCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(ProgressSectionComponent), default(ICommand));


    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildDeleteCommandProperty =
    BindableProperty.Create(nameof(ChildDeleteCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty TaskGrabbedCommandProperty =
    BindableProperty.Create(nameof(TaskGrabbedCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty DroppedOnTaskCommandProperty =
    BindableProperty.Create(nameof(DroppedOnTaskCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty CreateTaskCommandProperty =
    BindableProperty.Create(nameof(CreateTaskCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public ICommand EditCommand
    {
        get => (ICommand)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public ICommand DroppedOnProgressCommand
    {
        get => (ICommand)GetValue(DroppedOnProgressCommandProperty);
        set => SetValue(DroppedOnProgressCommandProperty, value);
    }

    public ICommand MoveLeftCommand
    {
        get => (ICommand)GetValue(MoveLeftCommandProperty);
        set => SetValue(MoveLeftCommandProperty, value);
    }

    public ICommand MoveRightCommand
    {
        get => (ICommand)GetValue(MoveRightCommandProperty);
        set => SetValue(MoveRightCommandProperty, value);
    }

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public ICommand CreateTaskCommand
    {
        get => (ICommand)GetValue(CreateTaskCommandProperty);
        set => SetValue(CreateTaskCommandProperty, value);
    }


    public ICommand ChildDeleteCommand
    {
        get => (ICommand)GetValue(ChildDeleteCommandProperty);
        set => SetValue(ChildDeleteCommandProperty, value);
    }

    public ICommand DroppedOnTaskCommand
    {
        get => (ICommand)GetValue(DroppedOnTaskCommandProperty);
        set => SetValue(DroppedOnTaskCommandProperty, value);
    }

    public ICommand TaskGrabbedCommand
    {
        get => (ICommand)GetValue(TaskGrabbedCommandProperty);
        set => SetValue(TaskGrabbedCommandProperty, value);
    }

    public ICommand ChildTapCommand
    {
        get => (ICommand)GetValue(ChildTapCommandProperty);
        set => SetValue(ChildTapCommandProperty, value);
    }

    public ProgressSectionComponent()
    {
        InitializeComponent();
        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        TaskContainer.Children.Clear();

        if (BindingContext is not TaskProgress taskProgress)
            return;

        if (taskProgress.Tasks == null || taskProgress.Tasks.Count == 0)
            return;

        foreach (var task in taskProgress.Tasks)
        {
            var taskComponent = new TaskComponent
            {
                BindingContext = task,
                TapCommand = new Command<ProjectTask>(OnTaskTapped),
                ChildTapCommand = new Command<ProjectTask>(OnTaskTapped),
                TaskGrabbedCommand = new Command<ProjectTask>(OnTaskGrabbed),
                ChildTaskGrabbedCommand = new Command<ProjectTask>(OnTaskGrabbed),
                DroppedOnTaskCommand = new Command<ProjectTask>(OnDroppedOnTask),
                ChildDroppedOnTaskCommand = new Command<ProjectTask>(OnDroppedOnTask),
                DeleteCommand = new Command<ProjectTask>(OnTaskDeleted),
                ChildDeleteCommand = new Command<ProjectTask>(OnTaskDeleted)
            };

            TaskContainer.Children.Add(taskComponent);
        }
    }

    private void OnTaskTapped(ProjectTask task)
    {
        if (ChildTapCommand?.CanExecute(task) == true)
            ChildTapCommand.Execute(task);
    }

    private void OnTaskDeleted(ProjectTask task)
    {
        if (ChildDeleteCommand?.CanExecute(task) == true)
            ChildDeleteCommand.Execute(task);
    }

    private void OnTaskGrabbed(ProjectTask task)
    {
        if (TaskGrabbedCommand?.CanExecute(task) == true)
        {
            DropZone.IsVisible = true;
            TaskGrabbedCommand.Execute(task);
        }
    }

    private void OnDroppedOnTask(ProjectTask task)
    {
        if (DroppedOnTaskCommand?.CanExecute(task) == true)
        {
            DropZone.IsVisible = false;
            DroppedOnTaskCommand.Execute(task);
        }
    }

    private void DroppedOnProgress(object sender, DropEventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && DroppedOnProgressCommand?.CanExecute(taskProgress) == true)
        {
            DropZone.IsVisible = false;
            DroppedOnProgressCommand.Execute(taskProgress);
        }
    }

    private void Left_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && MoveLeftCommand?.CanExecute(taskProgress) == true)
        {
            MoveLeftCommand.Execute(taskProgress);
        }
    }

    private void Right_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && MoveRightCommand?.CanExecute(taskProgress) == true)
        {
            MoveRightCommand.Execute(taskProgress);
        }
    }

    private void Edit_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && EditCommand?.CanExecute(taskProgress) == true)
        {
            EditCommand.Execute(taskProgress);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && DeleteCommand?.CanExecute(taskProgress) == true)
        {
            DeleteCommand.Execute(taskProgress);
        }
    }

    private void Create_Task_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskProgress taskProgress && CreateTaskCommand?.CanExecute(taskProgress) == true)
        {
            CreateTaskCommand.Execute(taskProgress);
        }
    }
}