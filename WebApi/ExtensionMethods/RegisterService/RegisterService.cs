using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Permissions;
using Infrastructure.Seed;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.DishService;
using Infrastructure.Services.DrinksService;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Infrastructure.Services.MenuDishService;
using Infrastructure.Services.MenuDrinksService;
using Infrastructure.Services.MenuService;
using Infrastructure.Services.OrderDishService;
using Infrastructure.Services.OrderDrinksService;
using Infrastructure.Services.OrderService;
using Infrastructure.Services.PaymentService;
using Infrastructure.Services.ReservationService;
using Infrastructure.Services.RoleService;
using Infrastructure.Services.UserRoleService;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IDishService,DishService>();
        services.AddScoped<IPaymentService,PaymentService>();
        services.AddScoped<IDrinksService,DrinksService>();
        services.AddScoped<IReservationService,ReservationService>();
        services.AddScoped<IRoleService,RoleService>();
        services.AddScoped<IUserRoleService,UserRoleService>();
        services.AddScoped<IMenuService,MenuService>();
        services.AddScoped<IOrderService,OrderService>();
        services.AddScoped<IMenuDishService,MenuDishService>();
        services.AddScoped<IMenuDrinksService,MenuDrinksService>();
        services.AddScoped<IOrderDishService,OrderDishService>();
        services.AddScoped<IOrderDrinksService,OrderDrinksService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
    }
}