using Bogus;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyDataGenerator
{
    internal class Program
    {
        private static string connectionString = "Server=localhost;Database=ECommerce;TrustServerCertificate=True;Integrated Security=True;";

        private static void Main(string[] args)
        {
            Console.WriteLine("=== Dummy Data Generator for ECommerce Schema (.NET 9) ===");

            try
            {
                int categoryCount = 1000;   // For testing, scale as needed
                int productCount = 5000;

                Console.WriteLine($"Generating {categoryCount} categories...");
                var categories = GenerateCategories(categoryCount);
                Console.WriteLine($"Generated: {categories.Rows.Count} rows");
                BulkInsertCategories(categories);
                Console.WriteLine("Categories inserted ✅");

                Console.WriteLine($"Generating {productCount} products...");
                var products = GenerateProducts(productCount, categories);
                Console.WriteLine($"Generated: {products.Rows.Count} rows");
                BulkInsertProducts(products);
                Console.WriteLine("Products inserted ✅");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex}");
            }

            Console.WriteLine("=== Done ===");
        }

        private static DataTable GenerateCategories(int count)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("ParentCategoryId", typeof(Guid));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedDate", typeof(DateTime));
            table.Columns.Add("DeletedDate", typeof(DateTime));
            table.Columns.Add("IsDeleted", typeof(bool));

            var faker = new Faker();
            var categoryIds = new List<Guid>();

            // First generate all category Ids to use for ParentCategoryId references
            for (int i = 0; i < count; i++)
                categoryIds.Add(Guid.NewGuid());

            for (int i = 0; i < count; i++)
            {
                var id = categoryIds[i];

                // 20% chance of having a parent category (but no self-parenting)
                Guid? parentId = null;
                if (i > 0 && faker.Random.Bool(0.2f))
                {
                    // Pick a random parent from previously created categories
                    int parentIndex = faker.Random.Int(0, i - 1);
                    parentId = categoryIds[parentIndex];
                }

                string name = faker.Commerce.Categories(1)[0] + $" {i + 1}";
                string description = faker.Commerce.ProductDescription();
                DateTime created = faker.Date.Past(2);
                DateTime updated = faker.Date.Between(created, DateTime.Now);

                // Soft delete randomly 5% categories
                bool isDeleted = faker.Random.Bool(0.05f);
                DateTime? deletedDate = isDeleted ? faker.Date.Between(updated, DateTime.Now) : (DateTime?)null;

                table.Rows.Add(id,
                    parentId.HasValue ? (object)parentId.Value : DBNull.Value,
                    name,
                    description,
                    created,
                    updated,
                    deletedDate.HasValue ? (object)deletedDate.Value : DBNull.Value,
                    isDeleted);
            }

            return table;
        }

        private static void BulkInsertCategories(DataTable categories)
        {
            if (categories.Rows.Count == 0)
                throw new InvalidOperationException("No categories to insert!");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.Categories"
            };

            bulk.ColumnMappings.Add("Id", "Id");
            bulk.ColumnMappings.Add("ParentCategoryId", "ParentCategoryId");
            bulk.ColumnMappings.Add("Name", "Name");
            bulk.ColumnMappings.Add("Description", "Description");
            bulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bulk.ColumnMappings.Add("UpdatedDate", "UpdatedDate");
            bulk.ColumnMappings.Add("DeletedDate", "DeletedDate");
            bulk.ColumnMappings.Add("IsDeleted", "IsDeleted");

            bulk.WriteToServer(categories);
        }

        private static DataTable GenerateProducts(int count, DataTable categories)
        {
            if (categories.Rows.Count == 0)
                throw new InvalidOperationException("No categories found to assign products!");

            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("CategoryId", typeof(Guid));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Currency", typeof(string));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("SKU", typeof(string));
            table.Columns.Add("Brand", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedDate", typeof(DateTime));
            table.Columns.Add("DeletedDate", typeof(DateTime));
            table.Columns.Add("IsDeleted", typeof(bool));

            var faker = new Faker();
            var categoryIds = new List<Guid>();

            foreach (DataRow row in categories.Rows)
            {
                // Only pick categories not deleted
                if (!(row["IsDeleted"] is bool isDel && isDel))
                    categoryIds.Add((Guid)row["Id"]);
            }

            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var id = Guid.NewGuid();

                // Random CategoryId from non-deleted categories
                var categoryId = categoryIds[rand.Next(categoryIds.Count)];

                string name = faker.Commerce.ProductName();
                string description = faker.Commerce.ProductDescription();
                decimal price = faker.Random.Decimal(10, 5000);
                string currency = "USD"; // or faker.Finance.Currency().Code; but keep fixed for consistency
                int stock = faker.Random.Int(0, 1000);
                string sku = $"SKU-{faker.Random.AlphaNumeric(8).ToUpper()}";
                string brand = faker.Company.CompanyName();

                DateTime created = DateTime.UtcNow;
                DateTime updated = DateTime.UtcNow;

                bool isDeleted = faker.Random.Bool(0.05f);
                DateTime? deletedDate = isDeleted ? faker.Date.Between(updated, DateTime.Now) : (DateTime?)null;

                table.Rows.Add(id, categoryId, name, description, price, currency, stock, sku, brand, created, updated,
                    deletedDate.HasValue ? (object)deletedDate.Value : DBNull.Value, isDeleted);
            }

            return table;
        }

        private static void BulkInsertProducts(DataTable products)
        {
            if (products.Rows.Count == 0)
                throw new InvalidOperationException("No products to insert!");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.Products"
            };

            bulk.ColumnMappings.Add("Id", "Id");
            bulk.ColumnMappings.Add("CategoryId", "CategoryId");
            bulk.ColumnMappings.Add("Name", "Name");
            bulk.ColumnMappings.Add("Description", "Description");
            bulk.ColumnMappings.Add("Price", "Price");
            bulk.ColumnMappings.Add("Currency", "Currency");
            bulk.ColumnMappings.Add("Stock", "Stock");
            bulk.ColumnMappings.Add("SKU", "SKU");
            bulk.ColumnMappings.Add("Brand", "Brand");
            bulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bulk.ColumnMappings.Add("UpdatedDate", "UpdatedDate");
            bulk.ColumnMappings.Add("DeletedDate", "DeletedDate");
            bulk.ColumnMappings.Add("IsDeleted", "IsDeleted");

            bulk.WriteToServer(products);
        }
    }
}