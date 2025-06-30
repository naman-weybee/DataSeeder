using Bogus;
using DataSeeder.Entities;

namespace DataSeeder.Generators
{
    public static class ProductGenerator
    {
        public static List<Product> GenerateProducts(int dataCount, List<Category> categories)
        {
            var faker = new Faker();
            var rand = new Random();
            var products = new List<Product>();
            var categoryIds = categories?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;

            for (int i = 0; i < dataCount; i++)
            {
                products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    CategoryId = categoryIds[rand.Next(categoryIds.Count)],
                    Name = faker.Commerce.ProductName(),
                    Description = faker.Commerce.ProductDescription(),
                    Price = faker.Random.Double(10, 5000),
                    Currency = "USD",
                    Stock = faker.Random.Int(0, 1000),
                    SKU = faker.Random.AlphaNumeric(8).ToUpper(),
                    Brand = faker.Company.CompanyName()
                });
            }

            return products;
        }
    }
}