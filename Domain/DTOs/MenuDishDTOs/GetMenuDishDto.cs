namespace Domain.DTOs.MenuDishDTOs;

public class GetMenuDishDto
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public int DishId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
    
}
