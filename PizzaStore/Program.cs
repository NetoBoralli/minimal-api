using Microsoft.EntityFrameworkCore;
using PizzaStore.PizzaDB;
using PizzaStore;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddCors(options => {});

builder.Services.AddDbContext<PizzaDB>(opt => opt.UseInMemoryDatabase("PizzaDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "PizzaStore API";
    config.Title = " PizzaStore API v1";
    config.Version = "v1";
});

var app = builder.Build();

// app.UseCors("https://netoboralli.github.io");

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "PizzaStoreAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var pizzas = app.MapGroup("/pizzas");

pizzas.MapGet("/", GetAllPizza);
pizzas.MapGet("/veggie", GetVeggiePizzas);
pizzas.MapGet("/{id}", GetPizza);
pizzas.MapPost("/", CreatePizza);
pizzas.MapPut("/{id}", UpdatePizza);
pizzas.MapDelete("/{id}", RemovePizza);

app.Run();

static async Task<IResult> GetAllPizza(PizzaDB db)
{
    return TypedResults.Ok(await db.Pizzas.ToArrayAsync());
}

static async Task<IResult> GetVeggiePizzas(PizzaDB db)
{
    return TypedResults.Ok(await db.Pizzas.Where(p => p.IsVeggie).ToListAsync());
}

static async Task<IResult> GetPizza(int id, PizzaDB db)
{
    return await db.Pizzas.FindAsync(id)
    is Pizza pizza ? TypedResults.Ok(pizza)
    : TypedResults.NotFound();
}

static async Task<IResult> CreatePizza(Pizza pizza, PizzaDB db)
{
    db.Pizzas.Add(pizza);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/pizzas/{pizza.Id}", pizza);
}

static async Task<IResult> UpdatePizza(int id, Pizza updatePizza, PizzaDB db)
{
    var pizza = await db.Pizzas.FindAsync(id);

    if (pizza is null) return TypedResults.NotFound();

    pizza.Name = updatePizza.Name;
    pizza.IsVeggie = updatePizza.IsVeggie;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> RemovePizza(int id, PizzaDB db)
{
    if (await db.Pizzas.FindAsync(id) is Pizza pizza)
    {
        db.Pizzas.Remove(pizza);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}