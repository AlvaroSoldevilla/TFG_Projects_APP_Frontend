using System.Windows.Input;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Properties;

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

    /*Adds the events when the component is interacted with*/
    private void AddGestures()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnTapped();

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        this.GestureRecognizers.Add(tapGesture);
        this.GestureRecognizers.Add(panGesture);
    }

    /*Sets the Binding context and creates the table layout*/
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
                Text = Properties.Resources.CreateRowButton,
                FontSize = 12,
                BackgroundColor = (Color)Application.Current.Resources["Create"],
                Command = new Command(() => view.AddRow())
            };

            var addColumnButton = new Button
            {
                Text = Properties.Resources.CreateColumnButton,
                FontSize = 12,
                BackgroundColor = (Color)Application.Current.Resources["Create"],
                Command = new Command(() => view.AddColumn())
            };

            var saveButton = new Button
            {
                Text = Properties.Resources.SaveButton,
                FontSize = 12,
                BackgroundColor = (Color)Application.Current.Resources["Save"],
                Command = new Command(() => view.EditTableData(view.Component))
            };

            var removeColButton = new Button
            {
                Text = Properties.Resources.RemoveColumnButton,
                FontSize = 12,
                BackgroundColor = (Color)Application.Current.Resources["Delete"],
                Command = new Command(() => view.RemoveColumn(view._tableData[0].Count-1))
            };

            var removeRowButton = new Button
            {
                Text = Properties.Resources.RemoveRowButton,
                FontSize = 12,
                BackgroundColor = (Color)Application.Current.Resources["Delete"],
                Command = new Command(() => view.RemoveRow(view._tableData.Count - 1))
            };

            view.Buttons.Add(addRowButton);
            view.Buttons.Add(addColumnButton);
            view.Buttons.Add(saveButton);
            view.DeleteButtons.Add(removeColButton);
            view.DeleteButtons.Add(removeRowButton);

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
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(3),
                        BackgroundColor = Colors.White,
                        TextColor = Colors.Black,
                        FontSize = 14,
                        HeightRequest = 40,
                        ReturnCommand = new Command<ConceptComponent>(view.EditTableData),
                        ReturnCommandParameter = view.Component
                    };

                    Grid.SetRow(entry, i);
                    Grid.SetColumn(entry, j);
                    view.TableGrid.Children.Add(entry);
                }
            }
        }
    }

    /*Logic for adding a row to the end*/
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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(3),
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                FontSize = 14,
                HeightRequest = 40,
                ReturnCommand = new Command<ConceptComponent>(EditTableData),
                ReturnCommandParameter = Component
            };
            Grid.SetRow(entry, rowIndex);
            Grid.SetColumn(entry, j);
            TableGrid.Children.Add(entry);
        }
    }

    /*logic for adding a column to the end*/
    private void AddColumn()
    {
        _tableData.ForEach(row => row.Add(""));
        TableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        for (int i = 0; i < _tableData.Count; i++)
        {
            var entry = new Entry
            {
                Text = "",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(3),
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                FontSize = 14,
                HeightRequest = 40,
                ReturnCommand = new Command<ConceptComponent>(EditTableData),
                ReturnCommandParameter = Component
            };
            Grid.SetRow(entry, i);
            Grid.SetColumn(entry, _tableData[0].Count - 1);
            TableGrid.Children.Add(entry);
        }
    }

    /*Logic for removing a row from the end*/
    private void RemoveRow(int rowNum)
    {
        if (rowNum>0)
        {
            _tableData.RemoveAt(rowNum);
            EditTableData(Component);
        }
        
    }

    /*Logic for removing a column from the end*/
    private void RemoveColumn(int colNum)
    {
        if (colNum>0)
        {
            foreach (var row in _tableData)
            {
                row.RemoveAt(colNum);
            }
            EditTableData(Component);
        }
    }

    /*Logic for dragging the component*/
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

    /*Logic for dropping the component*/
    private void OnDragEnded(double x, double y)
    {

        if (DragEndCommand?.CanExecute(this) == true)
        {
            Component.PosX = x;
            Component.PosY = y;
            DragEndCommand.Execute(Component);
        }
    }

    /*Logic for clicking on the component*/
    private void OnTapped()
    {
        if (TapCommand?.CanExecute(this) == true)
        {
            TapCommand.Execute(Component);
        }
    }

    /*Logic for editing the content of the table, goes through _tableData and transforms it into a single string where rows are separated by line returns and columns are separated by tabs*/
    private void EditTableData(ConceptComponent component)
    {
        int rowCount = _tableData.Count;
        int columnCount = _tableData[0].Count;

        foreach (var child in TableGrid.Children)
        {
            if (child is Entry entry)
            {
                int row = Grid.GetRow(entry);
                int col = Grid.GetColumn(entry);

                if (row < rowCount && col < columnCount)
                {
                    _tableData[row][col] = entry.Text ?? string.Empty;
                }
            }
        }

        if (UpdateContentCommand?.CanExecute(component) == true)
        {
            component.Content = string.Join("\n", _tableData.Select(row => string.Join("\t", row.Select(col => col))));

            UpdateContentCommand.Execute(component);
        }
    }
}