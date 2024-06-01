namespace Domain.Entities;

public class Dish:BaseEntity
{
    public string Name { get; set; }=null!;
    public string Description  { get; set; }=null!;
    public decimal Price { get; set; }
    public string Photo { get; set; }=null!;


    public List<Menu>? Menus { get; set; }
    public List<Order>? Orders { get; set; }
  
}
