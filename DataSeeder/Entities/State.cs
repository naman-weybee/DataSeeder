using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class State : Base
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public Guid CountryId { get; set; }
    }
}