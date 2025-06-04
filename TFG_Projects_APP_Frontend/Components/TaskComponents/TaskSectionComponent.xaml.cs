using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class TaskSectionComponent : ContentView
{
    public static readonly BindableProperty SectionProperty =
    BindableProperty.Create(nameof(SectionProperty), typeof(TaskSection), typeof(TaskSectionComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty EditCommandProperty =
    BindableProperty.Create(nameof(EditCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty HoverEnterCommandProperty =
    BindableProperty.Create(nameof(HoverEnterCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty HoverExitCommandProperty =
    BindableProperty.Create(nameof(HoverExitCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveLeftCommandProperty =
    BindableProperty.Create(nameof(MoveLeftCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty MoveRightCommandProperty =
    BindableProperty.Create(nameof(MoveRightCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(TaskSectionComponent), default(ICommand));


    public static readonly BindableProperty ChildDragEndCommandProperty =
    BindableProperty.Create(nameof(ChildDragEndCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildTapCommandProperty =
    BindableProperty.Create(nameof(ChildTapCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildDeleteCommandProperty =
    BindableProperty.Create(nameof(ChildDeleteCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildHoverEnterCommandProperty =
    BindableProperty.Create(nameof(ChildHoverEnterCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty ChildHoverExitCommandProperty =
    BindableProperty.Create(nameof(ChildHoverExitCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty CreateTaskCommandProperty =
    BindableProperty.Create(nameof(CreateTaskCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));


    public TaskSection Section
    {
        get => (TaskSection)GetValue(SectionProperty);
        set => SetValue(SectionProperty, value);
    }

    public ICommand EditCommand
    {
        get => (ICommand)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
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

    public ICommand ChildHoverEnterCommand
    {
        get => (ICommand)GetValue(ChildHoverEnterCommandProperty);
        set => SetValue(ChildHoverEnterCommandProperty, value);
    }

    public ICommand ChildHoverExitCommand
    {
        get => (ICommand)GetValue(ChildHoverExitCommandProperty);
        set => SetValue(ChildHoverExitCommandProperty, value);
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

    public TaskSectionComponent()
	{
        InitializeComponent();
        AddGestures();
        this.BindingContextChanged += (s, e) => OnBindingContextChanged();
    }


    private void OnBindingContextChanged()
    {
        if (BindingContext is TaskSection section)
        {
            Section = section;
        }
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TaskSectionComponent view && newValue is TaskSection)
        {
            view.BindingContext = view;
            view.TaskContainer.Children.Clear();

            if (view.Section.Tasks != null && view.Section.Tasks.Count > 0)
            {
                foreach (var task in view.Section.Tasks)
                {
                    ContentView renderedChild = new TaskComponent
                    {
                        ComponentTask = task,
                        TapCommand = new Command<ProjectTask>(view.ChildTapped),
                        DragEndCommand = new Command<ProjectTask>(view.ChildDragEnded),
                        ChildDragEndCommand = new Command<ProjectTask>(view.ChildDragEnded),
                        ChildTapCommand = new Command<ProjectTask>(view.ChildTapped),
                        HoverEnterCommand = new Command<ProjectTask>(view.ChildHoverEntered),
                        HoverExitCommand = new Command<ProjectTask>(view.ChildHoverExited),
                        DeleteCommand = new Command<ProjectTask>(view.ChildDeleted),
                        ChildDeleteCommand = new Command<ProjectTask>(view.ChildDeleted)
                    };
                    view.TaskContainer.Children.Add(renderedChild);
                }
            }
        }
    }

    private void AddGestures()
    {

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

    private void OnHoverExit()
    {
        if (HoverExitCommand?.CanExecute(Section) == true)
        {
            HoverExitCommand.Execute(Section);
        }
    }

    private void OnHoverEnter()
    {
        if (HoverEnterCommand?.CanExecute(Section) == true)
        {
            HoverEnterCommand.Execute(Section);
        }
    }

    private void ChildTapped(ProjectTask projectTask)
    {
        if (ChildTapCommand?.CanExecute(projectTask) == true)
        {
            ChildTapCommand.Execute(projectTask);
        }
    }

    private void ChildDragEnded(ProjectTask projectTask)
    {
        if (ChildDragEndCommand?.CanExecute(projectTask) == true)
        {
            ChildDragEndCommand.Execute(projectTask);
        }
    }

    private void ChildDeleted(ProjectTask projectTask)
    {
        if (ChildDeleteCommand?.CanExecute(projectTask) == true)
        {
            ChildDeleteCommand.Execute(projectTask);
        }
    }

    private void ChildHoverExited(ProjectTask projectTask)
    {
        if (ChildHoverExitCommand?.CanExecute(projectTask) == true)
        {
            ChildHoverExitCommand.Execute(projectTask);
        }
    }

    private void ChildHoverEntered(ProjectTask projectTask)
    {
        if (ChildHoverEnterCommand?.CanExecute(projectTask) == true)
        {
            ChildHoverEnterCommand.Execute(projectTask);
        }
    }

    private void Left_Clicked(object sender, EventArgs e)
    {
        if (MoveLeftCommand?.CanExecute(Section) == true)
        {
            MoveLeftCommand.Execute(Section);
        }
    }

    private void Right_Clicked(object sender, EventArgs e)
    {
        if (MoveRightCommand?.CanExecute(Section) == true)
        {
            MoveRightCommand.Execute(Section);
        }
    }

    private void Edit_Clicked(object sender, EventArgs e)
    {
        if (EditCommand?.CanExecute(Section) == true)
        {
            EditCommand.Execute(Section);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (DeleteCommand?.CanExecute(Section) == true)
        {
            DeleteCommand.Execute(Section);
        }
    }

    private void Create_Task_Clicked(object sender, EventArgs e)
    {
        if (CreateTaskCommand?.CanExecute(Section) == true)
        {
            CreateTaskCommand.Execute(null);
        }
    }

    private void OnDrop(object sender, DropEventArgs e)
    {

    }
}