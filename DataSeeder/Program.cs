using DataSeeder;
using DataSeeder.Data;
using DataSeeder.Helpers;

namespace DummyDataGenerator
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var categories = Generators.GenerateCategories(Constants.CategoryCount);
                var categoryTable = DataHelper.ToDataTable(categories);
                ApplicationDbContext.BulkInsert(categoryTable, "Categories");

                var products = Generators.GenerateProducts(Constants.ProductCount, categories);
                var productTable = DataHelper.ToDataTable(products);
                ApplicationDbContext.BulkInsert(productTable, "Products");

                var roles = Generators.GenerateRoles();
                var roleTable = DataHelper.ToDataTable(roles);
                ApplicationDbContext.BulkInsert(roleTable, "Roles");

                var roleEntities = Generators.GenerateRoleEntities();
                var roleEntityTable = DataHelper.ToDataTable(roleEntities);
                ApplicationDbContext.BulkInsert(roleEntityTable, "RoleEntities");

                var rolePermissions = Generators.GenerateRolePermissions(Constants.RolePermissionCount, roles);
                var rolePermissionsTable = DataHelper.ToDataTable(rolePermissions);
                ApplicationDbContext.BulkInsert(rolePermissionsTable, "RolePermissions");

                var genders = Generators.GenerateGenders();
                var genderTable = DataHelper.ToDataTable(genders);
                ApplicationDbContext.BulkInsert(genderTable, "Gender");

                var users = Generators.GenerateUsers(Constants.UserCount, roles, genders);
                var userTable = DataHelper.ToDataTable(users);
                ApplicationDbContext.BulkInsert(userTable, "Users");

                var countries = Generators.GenerateCountries(Constants.CountryCount);
                var countryTable = DataHelper.ToDataTable(countries);
                ApplicationDbContext.BulkInsert(countryTable, "Countries");

                var states = Generators.GenerateStates(Constants.StateCount, countries);
                var stateTable = DataHelper.ToDataTable(states);
                ApplicationDbContext.BulkInsert(stateTable, "States");

                var cities = Generators.GenerateCities(Constants.CityCount, states);
                var cityTable = DataHelper.ToDataTable(cities);
                ApplicationDbContext.BulkInsert(cityTable, "Cities");

                var addresses = Generators.GenerateAddresses(Constants.AddressCount, users, countries, states, cities);
                var addressTable = DataHelper.ToDataTable(addresses);
                ApplicationDbContext.BulkInsert(addressTable, "Address");

                var cartItems = Generators.GenerateCartItems(Constants.CartItemCount, users, products);
                var orderIds = Generators.GenerateOrderIds(Constants.OrderCount);

                var orderItems = Generators.GenerateOrderItems(Constants.OrderItemCount, orderIds, addresses, cartItems, products);
                var orderItemTable = DataHelper.ToDataTable(orderItems);
                ApplicationDbContext.BulkInsert(orderItemTable, "OrderItems");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }

            Console.WriteLine("========== Data Added Successfully !!! ==========");
            Console.ReadKey();
        }
    }
}