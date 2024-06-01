using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; } 
    public DbSet<UserRole> UserRoles { get; set; } 
    public DbSet<Role> Roles { get; set; } 
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Drinks> Drinkses { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuDish> MenuDishes { get; set; }
    public DbSet<MenuDrinks> MenuDrinks { get; set; }
    public DbSet<OrderDish> OrderDishes { get; set; }
    public DbSet<OrderDrink> OrderDrinks { get; set; }
    public DbSet<Payment> Payments { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }
}