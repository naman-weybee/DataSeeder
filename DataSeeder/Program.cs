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

                var countries = Generators.GenerateCountries(Constants.CountryCount);
                var countryTable = DataHelper.ToDataTable(countries);
                ApplicationDbContext.BulkInsert(countryTable, "Countries");

                var states = Generators.GenerateCountries(Constants.StateCount);
                var stateTable = DataHelper.ToDataTable(states);
                ApplicationDbContext.BulkInsert(stateTable, "States");

                var cities = Generators.GenerateCountries(Constants.CityCount);
                var cityTable = DataHelper.ToDataTable(cities);
                ApplicationDbContext.BulkInsert(cityTable, "Cities");
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