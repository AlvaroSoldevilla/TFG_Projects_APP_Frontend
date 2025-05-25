namespace TFG_Projects_APP_Frontend.Components.CreateModal;

public partial class DynamicInputPage<T> : ContentPage where T : new()
{
    private readonly TaskCompletionSource<T> _tcs;
    private readonly Dictionary<string, View> _inputs = new();
    private readonly List<FieldDefinition> _fields;

    public DynamicInputPage(TaskCompletionSource<T> tcs)
    {
        _tcs = tcs;
        _fields = FormHelper.GetFieldDefinitions<T>();
        BuildForm();
    }

    private void BuildForm()
    {
        var layout = new VerticalStackLayout { Padding = 20, Spacing = 10 };
        var objectName = typeof(T).Name.Replace("Form", "").Replace("Create", "");
        layout.Children.Add(new Label
        {
            Text = $"Create {objectName}",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        });

        foreach (var field in _fields)
        {
            layout.Children.Add(new Label { Text = field.Label });

            View input;
            var baseType = Nullable.GetUnderlyingType(field.DataType) ?? field.DataType;

            if (baseType == typeof(string))
                input = new Entry();
            else if (baseType == typeof(DateTime))
                input = new DatePicker();
            else if (baseType == typeof(int))
                input = new Entry { Keyboard = Keyboard.Numeric };
            else if (baseType == typeof(bool))
                input = new Switch();
            else
                continue;

            _inputs[field.PropertyName] = input;
            layout.Children.Add(input);
        }

        var button = new Button { Text = "Create" };
        var cancelButton = new Button { Text = "Cancel" };
        button.Clicked += Submit;
        cancelButton.Clicked += Cancel;
        layout.Children.Add(button);
        layout.Children.Add(cancelButton);

        Content = new ScrollView { Content = layout };
    }

    private void Submit(object sender, EventArgs e)
    {
        var obj = new T();

        foreach (var field in _fields)
        {
            var prop = typeof(T).GetProperty(field.PropertyName);
            if (prop == null || !_inputs.ContainsKey(field.PropertyName))
                continue;

            var input = _inputs[field.PropertyName];

            object? value = field.DataType switch
            {
                var t when t == typeof(string) => ((Entry)input).Text,
                var t when t == typeof(int) =>
                    int.TryParse(((Entry)input).Text, out var i) ? i : 0,
                var t when t == typeof(DateTime) => ((DatePicker)input).Date,
                _ => null
            };

            prop.SetValue(obj, value);
        }

        _tcs.SetResult(obj);
        Application.Current.MainPage.Navigation.PopModalAsync();
    }

    private void Cancel(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopModalAsync();
    }
}
