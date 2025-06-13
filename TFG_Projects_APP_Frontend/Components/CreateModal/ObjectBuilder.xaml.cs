namespace TFG_Projects_APP_Frontend.Components.CreateModal;

/*An Object used to create a form that dynamically changes depending on the class it recieves, which can be any class, but it is recomended to use a specific class for the form*/
public partial class DynamicInputPage<T> : ContentPage where T : new()
{
    private readonly TaskCompletionSource<T> _tcs;
    private readonly Dictionary<string, View> _inputs = new();
    private readonly List<FieldDefinition> _fields;
    private string Title { get; set; }

    public DynamicInputPage(TaskCompletionSource<T> tcs, string title)
    {
        _tcs = tcs;
        _fields = FormHelper.GetFieldDefinitions<T>();
        this.Title = title;
        BuildForm();
    }

    /*Logic for building the creation form. Goes through each field of the object and creates an appropiate input*/
    private void BuildForm()
    {
        var layout = new VerticalStackLayout { Padding = 20, Spacing = 10 };
        layout.Children.Add(new Label
        {
            Text = Title,
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
                input = new Entry();
            else if (baseType == typeof(bool))
                input = new Switch();
            else
                continue;

            _inputs[field.PropertyName] = input;
            layout.Children.Add(input);
        }

        var button = new Button { Text = "Create", BackgroundColor = (Color)Application.Current.Resources["Create"] };
        var cancelButton = new Button { Text = "Cancel", BackgroundColor = (Color)Application.Current.Resources["Misc"] };
        button.Clicked += Submit;
        cancelButton.Clicked += Cancel;
        layout.Children.Add(button);
        layout.Children.Add(cancelButton);

        Content = new ScrollView { Content = layout };
    }

    /*Logic for clicking the submit button. Goes through each field, parses the information and the returns the built object*/
    private void Submit(object sender, EventArgs e)
    {
        var obj = new T();

        foreach (var field in _fields)
        {
            var prop = typeof(T).GetProperty(field.PropertyName);
            if (prop == null || !_inputs.ContainsKey(field.PropertyName))
                continue;

            var input = _inputs[field.PropertyName];
            var baseType = Nullable.GetUnderlyingType(field.DataType) ?? field.DataType;

            object? value = null;

            if (baseType == typeof(string))
            {
                value = ((Entry)input).Text;
            }
            else if (baseType == typeof(int))
            {
                var text = ((Entry)input).Text?.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    value = Nullable.GetUnderlyingType(field.DataType) != null ? null : 0;
                }
                else if (int.TryParse(text, out var i))
                {
                    value = i;
                }
                else
                {
                    value = Nullable.GetUnderlyingType(field.DataType) != null ? null : 0;
                }
            }
            else if (baseType == typeof(bool))
            {
                value = ((Switch)input).IsToggled;
            }
            else if (baseType == typeof(DateTime))
            {
                value = ((DatePicker)input).Date;
            }

            prop.SetValue(obj, value);
        }

        _tcs.SetResult(obj);
        Application.Current.MainPage.Navigation.PopModalAsync();
    }

    /*Logic for pressing cancel button*/
    private void Cancel(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopModalAsync();
    }
}
