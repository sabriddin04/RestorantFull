namespace Domain.Entities;

public class OrderDrink:BaseEntity
{
    public int OrderId { get; set; }
    public int DrinkId { get; set; }
    public int Quantity  { get; set; }

    

    public Order? Order { get; set; }
    public Drinks? Drink { get; set; }

}
