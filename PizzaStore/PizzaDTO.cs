namespace PizzaStore;

public class PizzaDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsVeggie { get; set; }

    public PizzaDTO() { }
    public PizzaDTO(Pizza pizza) => (Id, Name, IsVeggie) = (pizza.Id, pizza.Name!, pizza.IsVeggie);
}