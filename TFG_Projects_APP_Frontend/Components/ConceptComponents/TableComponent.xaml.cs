using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Components.ConceptComponents;

public partial class TableComponent : ContentView
{
    public static readonly BindableProperty ComponentProperty =
    BindableProperty.Create(nameof(Component), typeof(ConceptComponent), typeof(ConceptBoardComponent), propertyChanged: OnComponentChanged);

    public static readonly BindableProperty TapCommandProperty =
    BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty DragEndCommandProperty =
    BindableProperty.Create(nameof(DragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty ChildDragEndCommandProperty =
    BindableProperty.Create(nameof(ChildDragEndCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

    public static readonly BindableProperty UpdateContentCommandProperty =
    BindableProperty.Create(nameof(UpdateContentCommandProperty), typeof(ICommand), typeof(NoteComponent), default(ICommand));

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

    public ICommand UpdateContentCommand
    {
        get => (ICommand)GetValue(UpdateContentCommandProperty);
        set => SetValue(UpdateContentCommandProperty, value);
    }

    private List<List<string>> _tableData = new List<List<string>>();

    private Point _startOffset;
    private Point _position;
    public TableComponent()
	{
        InitializeComponent();
        BindingContext = this;
        AddGestures();
    }

    private static void OnComponentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TableComponent view)
        {
            view.BindingContext = view;

            view._tableData.Clear();
            view.TableGrid.Children.Clear();
            view.TableGrid.RowDefinitions.Clear();
            view.TableGrid.ColumnDefinitions.Clear();

            var addRowButton = new Button
            {
                Text = "Add Row",
                Command = new Command(() => view.AddRow())
            };

            var addColumnButton = new Button
            {
                Text = "Add Column",
                Command = new Command(() => view.AddColumn())
            };

            var saveButton = new Button
            {
                Text = "Save",
                Command = new Command(() => view.EditTableData(view.Component))
            };

            view.Buttons.Add(addRowButton);
            view.Buttons.Add(addColumnButton);
            view.Buttons.Add(saveButton);

            var rows = view.Component.Content.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
            {
                var columns = row.Split("\t").ToList();
                view._tableData.Add(columns);
            }

            int rowCount = view._tableData.Count;
            int columnCount = view._tableData[0].Count;

            for (int i = 0; i < rowCount; i++)
            {
                view.TableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                for (int j = 0; j < columnCount; j++)
                {
                    if (i == 0)
                    {
                        view.TableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    }
                    var entry = new Entry
                    {
                        Text = view._tableData[i][j],
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(5),
                        ReturnCommand= new Command<ConceptComponent>(view.EditTableData),
                        ReturnCommandParameter = view.Component
                    };

                    Grid.SetRow(entry, i);
                    Grid.SetColumn(entry, j);
                    view.TableGrid.Children.Add(entry);
                }
            }
        }
    }

    private void AddRow()
    {
        var newRow = new List<string>(new string[_tableData[0].Count]);
        _tableData.Add(newRow);
        int rowIndex = _tableData.Count - 1;
        TableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (int j = 0; j < _tableData[0].Count; j++)
        {
            var entry = new Entry
            {
                Text = "",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Margin = new Thickness(5)
            };
            Grid.SetRow(entry, rowIndex);
            Grid.SetColumn(entry, j);
            TableGrid.Children.Add(entry);
        }
    }

    private void AddColumn()
    {
        _tableData.ForEach(row => row.Add(""));
        TableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        for (int i = 0; i < _tableData.Count; i++)
        {
            var entry = new Entry
            {
                Text = "",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Margin = new Thickness(5)
            };
            Grid.SetRow(entry, i);
            Grid.SetColumn(entry, _tableData[0].Count - 1);
            TableGrid.Children.Add(entry);
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

    private void EditTableData(ConceptComponent component)
    {
        if (UpdateContentCommand?.CanExecute(component) == true)
        {
            component.Content = string.Join("\n", _tableData.Select(row => string.Join("\t", row)));

            UpdateContentCommand.Execute(component);
        }
    }
}