using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class TaskSectionComponent : ContentView
{

    public static readonly BindableProperty EditCommandProperty =
    BindableProperty.Create(nameof(EditCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty DroppedOnSectionCommandProperty =
    BindableProperty.Create(nameof(DroppedOnSectionCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveLeftCommandProperty =
    BindableProperty.Create(nameof(MoveLeftCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveRightCommandProperty =
    BindableProperty.Create(nameof(MoveRightCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));


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

    public ICommand DroppedOnSectionCommand
    {
        get => (ICommand)GetValue(DroppedOnSectionCommandProperty);
        set => SetValue(DroppedOnSectionCommandProperty, value);
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

    public TaskSectionComponent()
	{
        InitializeComponent();
        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection taskSection)
        {
            TaskContainer.Children.Clear();

            if (BindingContext is not TaskSection section)
                return;

            if (section.Tasks == null || section.Tasks.Count == 0)
                return;

            foreach (var task in section.Tasks)
            {
                TaskComponent child = new()
                {
                    BindingContext = task,
                    TapCommand = new Command<ProjectTask>(ChildTapped),
                    ChildTapCommand = new Command<ProjectTask>(ChildTapped),
                    TaskGrabbedCommand = new Command<ProjectTask>(TaskGrabbed),
                    ChildTaskGrabbedCommand = new Command<ProjectTask>(TaskGrabbed),
                    DroppedOnTaskCommand = new Command<ProjectTask>(DroppedOnTask),
                    ChildDroppedOnTaskCommand = new Command<ProjectTask>(DroppedOnTask),
                    DeleteCommand = new Command<ProjectTask>(ChildDeleted),
                    ChildDeleteCommand = new Command<ProjectTask>(ChildDeleted)
                };

                TaskContainer.Children.Add(child);
            }
        }
    }

    private void TaskGrabbed(ProjectTask task)
    {
        if (TaskGrabbedCommand?.CanExecute(task) == true)
        {
            DropZone.IsVisible = true;
            TaskGrabbedCommand.Execute(task);
        }
    }

    private void ChildTapped(ProjectTask projectTask)
    {
        if (ChildTapCommand?.CanExecute(projectTask) == true)
        {
            ChildTapCommand.Execute(projectTask);
        }
    }

    private void DroppedOnTask(ProjectTask projectTask)
    {
        if (DroppedOnTaskCommand?.CanExecute(projectTask) == true)
        {
            DropZone.IsVisible = false;
            DroppedOnTaskCommand.Execute(projectTask);
        }
    }
    private void DroppedOnSection(object sender, DropEventArgs e)
    {
        if (BindingContext is TaskSection section && DroppedOnSectionCommand?.CanExecute(section) == true)
        {
            DropZone.IsVisible = false;
            DroppedOnSectionCommand.Execute(section);
        }
    }

    private void ChildDeleted(ProjectTask projectTask)
    {
        if (BindingContext is TaskSection section && ChildDeleteCommand?.CanExecute(projectTask) == true)
        {
            ChildDeleteCommand.Execute(projectTask);
        }
    }

    private void Left_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection section && MoveLeftCommand?.CanExecute(section) == true)
        {
            MoveLeftCommand.Execute(section);
        }
    }

    private void Right_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection section && MoveRightCommand?.CanExecute(section) == true)
        {
            MoveRightCommand.Execute(section);
        }
    }

    private void Edit_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection section && EditCommand?.CanExecute(section) == true)
        {
            EditCommand.Execute(section);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection section && DeleteCommand?.CanExecute(section) == true)
        {
            DeleteCommand.Execute(section);
        }
    }

    private void Create_Task_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskSection section && CreateTaskCommand?.CanExecute(section) == true)
        {
            CreateTaskCommand.Execute(section);
        }
    }
}