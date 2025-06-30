using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class Category : Base
    {
        public Guid Id { get; set; }

        public Guid? ParentCategoryId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Length(1, 500)]
        public string? Description { get; set; }
    }
}