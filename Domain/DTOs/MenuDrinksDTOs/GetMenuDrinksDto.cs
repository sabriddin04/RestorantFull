namespace Domain.DTOs.MenuDrinksDTOs;

public class GetMenuDrinksDto
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public int DrinkId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}
