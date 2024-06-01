namespace Domain.Entities;

public class Menu:BaseEntity
{
   public string Name { get; set; } =null!;
   public string Description { get; set; }=null!;



   public List<Dish>? Dishes { get; set; }
   public List<Drinks>? Drinks { get; set; }

}
