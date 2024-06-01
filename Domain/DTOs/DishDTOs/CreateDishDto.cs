using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.DishDTOs;

public class CreateDishDto
{

    public string Name { get; set; }=null!;
    public string Description  { get; set; }=null!;
    public decimal Price { get; set; }
    public IFormFile Photo { get; set; }=null!;

}
