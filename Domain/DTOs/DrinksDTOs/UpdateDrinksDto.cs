using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.DrinksDTOs;

public class UpdateDrinksDto
{
    public int Id { get; set; }
    public string? Name { get; set; }=null!;
    public double Capacity { get; set; }
    public decimal Price { get; set; }
    public IFormFile Photo { get; set; } = null!;
}
