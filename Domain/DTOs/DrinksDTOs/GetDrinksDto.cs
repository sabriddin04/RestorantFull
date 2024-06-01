namespace Domain.DTOs.DrinksDTOs;

public class GetDrinksDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public double Capacity { get; set; }
    public decimal Price { get; set; }
    public string Photo { get; set; } = null!;
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}
