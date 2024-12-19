namespace PizzaStore.PizzaDB;

using Microsoft.EntityFrameworkCore;
using PizzaStore;

class PizzaDB(DbContextOptions<PizzaDB> options) : DbContext(options)
{
    public DbSet<Pizza> Pizzas => Set<Pizza>();
}