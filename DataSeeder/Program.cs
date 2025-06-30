using DataSeeder.Data;
using DataSeeder.Generators;
using DataSeeder.Helpers;

namespace DummyDataGenerator
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var categoryCount = 10000;
                var productCount = 50000;

                var categories = CategoryGenerator.GenerateCategories(categoryCount);
                var categoryTable = DataHelper.ToDataTable(categories);

                ApplicationDbContext.BulkInsert(categoryTable, "Categories");

                var products = ProductGenerator.GenerateProducts(productCount, categories);
                var productTable = DataHelper.ToDataTable(products);

                ApplicationDbContext.BulkInsert(productTable, "Products");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex}");
            }

            Console.WriteLine("=== Done ===");
        }
    }
}