using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.TaskComponents;

public partial class TaskComponent : ContentView
{
    public static readonly BindableProperty ComponentTaskProperty =
    BindableProperty.Create(nameof(ComponentTaskProperty), typeof(ProjectTask), typeof(TaskComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty HoverEnterCommandProperty =
    BindableProperty.Create(nameof(HoverEnterCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty HoverExitCommandProperty =
    BindableProperty.Create(nameof(HoverExitCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

    public static readonly BindableProperty DeleteCommandProperty =
    BindableProperty.Create(nameof(DeleteCommandProperty), typeof(ICommand), typeof(TaskComponent), default(ICommand));

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

    public ICommand DragEndCommand
    {
        get => (ICommand)GetValue(DragEndCommandProperty);
        set => SetValue(DragEndCommandProperty, value);
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

    private Point _startOffset;
    private bool _suppressPan = false;

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
                        DragEndCommand = new Command<ProjectTask>(view.ChildDragEnded),
                        HoverEnterCommand = new Command<ProjectTask>(view.ChildHoverEntered),
                        HoverExitCommand = new Command<ProjectTask>(view.ChildHoverExited),
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

    private void OnDragStarting(object sender, DragStartingEventArgs e)
    {
        e.Data.Properties["Task"] = ComponentTask;
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

    private void OnDropCompleted(object sender, DropCompletedEventArgs e)
    {
        if (DragEndCommand?.CanExecute(this) == true)
        {
            DragEndCommand.Execute(ComponentTask);
        }
    }

    private void OnTapped()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(ComponentTask);
        }
    }

    private void OnHoverExit()
    {
        if (HoverExitCommand?.CanExecute(ComponentTask) == true)
        {
            HoverExitCommand.Execute(ComponentTask);
        }
    }

    private void OnHoverEnter()
    {
        if (HoverEnterCommand?.CanExecute(ComponentTask) == true)
        {
            HoverEnterCommand.Execute(ComponentTask);
        }
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (DeleteCommand?.CanExecute(ComponentTask) == true)
        {
            DeleteCommand.Execute(ComponentTask);
        }
    }

    
}