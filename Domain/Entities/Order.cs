using Domain.Enums;

namespace Domain.Entities;

public class Order:BaseEntity
{
    public string Name { get; set; }=null!;
    public int TableId { get; set; } 
    public OrderStatus Status { get; set; } 

    


    public Table? Table { get; set; }
    public List<Drinks>? Drinks { get; set; }
    public List<Dish>? Dishes { get; set; }
}
