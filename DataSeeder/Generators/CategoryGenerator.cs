using Bogus;
using DataSeeder.Entities;

namespace DataSeeder.Generators
{
    public static class CategoryGenerator
    {
        public static List<Category> GenerateCategories(int dataCount)
        {
            var faker = new Faker();
            var categoryIds = new List<Guid>();
            var categories = new List<Category>();

            for (int i = 0; i < dataCount; i++)
            {
                var id = Guid.NewGuid();
                Guid? parentId = null;
                categoryIds.Add(id);

                if (i > 0 && faker.Random.Bool(0.2f))
                {
                    var parentIndex = faker.Random.Int(0, categories.Count - 2);
                    parentId = categoryIds[parentIndex];
                }

                categories.Add(new Category
                {
                    Id = id,
                    ParentCategoryId = parentId,
                    Name = faker.Commerce.Categories(1)[0] + $"_{i}",
                    Description = faker.Commerce.ProductDescription()
                });
            }

            return categories;
        }
    }
}