using Domain.Constants;
using Domain.DTOs.RolePermissionDTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class Seeder(DataContext context, IHashService hashService)
{
    public async Task Initial()
    {
        await SeedRole();
        await DefaultUsers();
    }


    #region SeedRole

    private async Task SeedRole()
    {
        try
        {
            var newRoles = new List<Role>()
            {
                new()
                {
                    Name = Roles.SuperAdmin,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.Admin,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.User,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
            };

            var existing = await context.Roles.ToListAsync();
            foreach (var role in newRoles)
            {
                if (existing.Exists(e => e.Name == role.Name) == false)
                {
                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            //ignored
        }
    }

    #endregion

    #region DefaultUsers

    private async Task DefaultUsers()
    {
        try
        {
            //super-admin
            var existingSuperAdmin = await context.Users.FirstOrDefaultAsync(x => x.Username == "SuperAdmin");
            if (existingSuperAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "superadmin@gmail.com",
                    Phone = "123456780",
                    Username = "SuperAdmin",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "SuperAdmin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await SeedClaimsForSuperAdmin();
            }


            //admin
            var existingAdmin = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
            if (existingAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "admin@gmail.com",
                    Phone = "123456780",
                    Username = "Admin",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await AddAdminPermissions();
            }

            //user
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
            if (user is null)
            {
                var superAdmin = new User()
                {
                    Email = "user@gmail.com",
                    Phone = "123456780",
                    Username = "User",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await AddUserPermissions();
            }
        }
        catch (Exception e)
        {
            //ignored;
        }
    }

    #endregion

    #region SeedClaimsForSuperAdmin

    private async Task SeedClaimsForSuperAdmin()
    {
        try
        {
            var superAdminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
            if (superAdminRole == null) return;
            var roleClaims = new List<RoleClaimsDto>();
            roleClaims.GetPermissions(typeof(Domain.Constants.Permissions));
            var existingClaims = await context.RoleClaims.Where(x => x.RoleId == superAdminRole.Id).ToListAsync();
            foreach (var claim in roleClaims)
            {
                if (existingClaims.Any(c => c.ClaimValue == claim.Value) == false)
                    await context.AddPermissionClaim(superAdminRole, claim.Value);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    #endregion


    #region AddUserPermissions

    private async Task AddUserPermissions()
    {
        //add claims
        var userRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
        if (userRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Dishes.View),
            new("Permissions", Domain.Constants.Permissions.Drinkses.View),
            new("Permissions", Domain.Constants.Permissions.Menus.View),
            new("Permissions", Domain.Constants.Permissions.Tables.View),
            new("Permissions", Domain.Constants.Permissions.Payments.Create),
            new("Permissions", Domain.Constants.Permissions.Reservations.Create),
        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == userRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(userRole, claim.Value);
            }
        }
    }

    #endregion

    #region AddAdminPermissions

    private async Task AddAdminPermissions()
    {
        //add claims
        var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
        if (adminRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Dishes.View),
            new("Permissions", Domain.Constants.Permissions.Dishes.Create),
            new("Permissions", Domain.Constants.Permissions.Dishes.Edit),
            new("Permissions", Domain.Constants.Permissions.Drinkses.View),
            new("Permissions", Domain.Constants.Permissions.Drinkses.Create),
            new("Permissions", Domain.Constants.Permissions.Drinkses.Edit),
            new("Permissions", Domain.Constants.Permissions.Tables.View),
            new("Permissions", Domain.Constants.Permissions.Tables.Create),
            new("Permissions", Domain.Constants.Permissions.Tables.Edit),
            new("Permissions", Domain.Constants.Permissions.Orders.View),
            new("Permissions", Domain.Constants.Permissions.Orders.Create),
            new("Permissions", Domain.Constants.Permissions.Orders.Edit),
            new("Permissions", Domain.Constants.Permissions.Reservations.View),
            new("Permissions", Domain.Constants.Permissions.Reservations.Create),
            new("Permissions", Domain.Constants.Permissions.Reservations.Edit),
            new("Permissions", Domain.Constants.Permissions.Payments.View),
            new("Permissions", Domain.Constants.Permissions.Payments.Create),
            new("Permissions", Domain.Constants.Permissions.Payments.Edit),
            new("Permissions", Domain.Constants.Permissions.Menus.View),
            new("Permissions", Domain.Constants.Permissions.Menus.Create),
            new("Permissions", Domain.Constants.Permissions.Menus.Edit),
            new("Permissions", Domain.Constants.Permissions.OrderDishes.View),
            new("Permissions", Domain.Constants.Permissions.OrderDishes.Create),
            new("Permissions", Domain.Constants.Permissions.OrderDishes.Edit),
            new("Permissions", Domain.Constants.Permissions.OrderDrinks.View),
            new("Permissions", Domain.Constants.Permissions.OrderDrinks.Create),
            new("Permissions", Domain.Constants.Permissions.OrderDrinks.Edit),
            new("Permissions", Domain.Constants.Permissions.MenuDishes.View),
            new("Permissions", Domain.Constants.Permissions.MenuDishes.Create),
            new("Permissions", Domain.Constants.Permissions.MenuDishes.Edit),
            new("Permissions", Domain.Constants.Permissions.MenuDrinks.View),
            new("Permissions", Domain.Constants.Permissions.MenuDrinks.Create),
            new("Permissions", Domain.Constants.Permissions.MenuDrinks.Edit),
        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == adminRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(adminRole, claim.Value);
            }
        }
    }

    #endregion
}