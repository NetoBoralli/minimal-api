namespace PizzaStore;

public class Pizza
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsVeggie { get; set; }
    public string? Recipe { get; set; }
}