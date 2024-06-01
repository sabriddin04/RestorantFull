namespace Domain.Entities;

public class MenuDrinks:BaseEntity
{
    public int MenuId { get; set; }
    public int DrinkId { get; set; }

    public Menu? Menu { get; set; }
    public Drinks? Drink { get; set; }

}
