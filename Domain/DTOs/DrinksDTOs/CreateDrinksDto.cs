using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.DrinksDTOs;

public class CreateDrinksDto
{
    public string Name { get; set; }=null!;
    public double Capacity { get; set; }
    public decimal Price { get; set; }
    public IFormFile Photo { get; set; } = null!;


}
