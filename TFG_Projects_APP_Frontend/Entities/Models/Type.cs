namespace TFG_Projects_APP_Frontend.Entities.Models;

class Type
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Component>? Components { get; set; }
}
