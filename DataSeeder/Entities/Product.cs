using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class Product : Base
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public int Stock { get; set; }

        public string SKU { get; set; }

        public string? Brand { get; set; }
    }
}