using DataSeeder.Enum;
using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class RoleEntity
    {
        public eRoleEntity Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}