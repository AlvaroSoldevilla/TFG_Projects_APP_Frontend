namespace TFG_Projects_APP_Frontend.Components.CreateModal;

public static class FormHelper
{
    /*Extracts the Field Definitions for the craetion form*/
    public static List<FieldDefinition> GetFieldDefinitions<T>()
    {
        return typeof(T)
            .GetProperties()
            .Where(p => p.CanWrite)
            .Select(p => new FieldDefinition
            {
                Label = p.Name,
                PropertyName = p.Name,
                DataType = p.PropertyType
            })
            .ToList();
    }
}
