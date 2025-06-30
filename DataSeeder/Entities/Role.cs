using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class Role : Base
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}