namespace Domain.Entities;

public class Drinks : BaseEntity
{
    public string Name { get; set; }=null!;
    public double Capacity { get; set; }
    public decimal Price { get; set; }
    public string Photo { get; set; }=null!;


    public List<Menu>? Menus { get; set; }
    public List<Order>? Orders { get; set; }
}
