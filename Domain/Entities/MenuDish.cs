namespace Domain.Entities;

public class MenuDish:BaseEntity
{
    public int MenuId { get; set; }
    public int DishId { get; set; }


    public Menu? Menu { get; set; }
    public Dish? Dish { get; set; }
}
