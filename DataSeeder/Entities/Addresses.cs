using DataSeeder.Enum;
using System.ComponentModel.DataAnnotations;

namespace DataSeeder.Entities
{
    public class Addresses : Base
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public Guid UserId { get; set; }

        public Guid CountryId { get; set; }

        public Guid StateId { get; set; }

        public Guid CityId { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        public eAddressType AdderessType { get; set; }

        [StringLength(500)]
        public string AddressLine { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}