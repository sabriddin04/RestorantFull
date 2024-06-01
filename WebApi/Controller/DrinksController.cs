using Domain.Constants;
using Domain.DTOs.DrinksDTOs;
using Domain.DTOs.TableDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.DrinksService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinksController(IDrinksService drinksService) : ControllerBase
{

    [HttpGet("drinkses")]
    [PermissionAuthorize(Permissions.Drinkses.View)]
    public async Task<Response<List<GetDrinksDto>>> GetDrinkses([FromQuery]DrinksFilter filter)
    {
       return await drinksService.GetDrinksesAsync(filter);
    }

    [HttpGet("{drinksId:int}")]
    [PermissionAuthorize(Permissions.Drinkses.View)]
    public async Task<Response<GetDrinksDto>> GetDrinksById(int id)
        => await drinksService.GetDrinksByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Drinkses.Create)]
    public async Task<Response<string>> CreateDrinks([FromForm]CreateDrinksDto create)
        => await drinksService.CreateDrinksAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Drinkses.Edit)]
    public async Task<Response<string>> UpdateDrinks([FromForm]UpdateDrinksDto update)
        => await drinksService.UpdateDrinksAsync(update);

    [HttpDelete("{drinksId:int}")]
    [PermissionAuthorize(Permissions.Drinkses.Delete)]
    public async Task<Response<bool>> DeleteDrinks(int id)
        => await drinksService.DeleteDrinksAsync(id);
}