namespace Domain.Constants;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return
        [
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete"
        ];
    }
    public static class Dishes
    {
        public const string View = "Permissions.Dishes.View";
        public const string Create = "Permissions.Dishes.Create";
        public const string Edit = "Permissions.Dishes.Edit";
        public const string Delete = "Permissions.Dishes.Delete";
    }
    public static class Drinkses
    {
        public const string View = "Permissions.Drinkses.View";
        public const string Create = "Permissions.Drinkses.Create";
        public const string Edit = "Permissions.Drinkses.Edit";
        public const string Delete = "Permissions.Drinkses.Delete";
    }
    public static class Orders
    {
        public const string View = "Permissions.Orders.View";
        public const string Create = "Permissions.Orders.Create";
        public const string Edit = "Permissions.Orders.Edit";
        public const string Delete = "Permissions.Orders.Delete";
    }
    public static class Menus
    {
        public const string View = "Permissions.Menus.View";
        public const string Create = "Permissions.Menus.Create";
        public const string Edit = "Permissions.Menus.Edit";
        public const string Delete = "Permissions.Menus.Delete";
    }
    public static class Tables
    {
        public const string View = "Permissions.Tables.View";
        public const string Create = "Permissions.Tables.Create";
        public const string Edit = "Permissions.Tables.Edit";
        public const string Delete = "Permissions.Tables.Delete";
    }
    public static class Payments
    {
        public const string View = "Permissions.Payments.View";
        public const string Create = "Permissions.Payments.Create";
        public const string Edit = "Permissions.Payments.Edit";
        public const string Delete = "Permissions.Payments.Delete";
    }
    public static class Reservations
    {
        public const string View = "Permissions.Reservations.View";
        public const string Create = "Permissions.Reservations.Create";
        public const string Edit = "Permissions.Reservations.Edit";
        public const string Delete = "Permissions.Reservations.Delete";
    }
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
    }
    public static class UserRoles
    {
        public const string View = "Permissions.UserRoles.View";
        public const string Create = "Permissions.UserRoles.Create";
        public const string Delete = "Permissions.UserRoles.Delete";
    }
    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Edit = "Permissions.Roles.Edit";
        public const string Delete = "Permissions.Roles.Delete";
    }
    public static class MenuDrinks
    {
        public const string View = "Permissions.MenuDrinks.View";
        public const string Create = "Permissions.MenuDrinks.Create";
        public const string Edit = "Permissions.MenuDrinks.Edit";
        public const string Delete = "Permissions.MenuDrinks.Delete";
    }
    public static class MenuDishes
    {
        public const string View = "Permissions.MenuDishes.View";
        public const string Create = "Permissions.MenuDishes.Create";
        public const string Edit = "Permissions.MenuDishes.Edit";
        public const string Delete = "Permissions.MenuDishes.Delete";
    }
    public static class OrderDishes
    {
        public const string View = "Permissions.OrderDishes.View";
        public const string Create = "Permissions.OrderDishes.Create";
        public const string Edit = "Permissions.OrderDishes.Edit";
        public const string Delete = "Permissions.OrderDishes.Delete";
    }
    public static class OrderDrinks
    {
        public const string View = "Permissions.OrderDrinks.View";
        public const string Create = "Permissions.OrderDrinks.Create";
        public const string Edit = "Permissions.OrderDrinks.Edit";
        public const string Delete = "Permissions.OrderDrinks.Delete";
    }


}
