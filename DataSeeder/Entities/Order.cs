using DataSeeder.Enum;

namespace DataSeeder.Entities
{
    public class Order : Base
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid BillingAddressId { get; set; }

        public Guid ShippingAddressId { get; set; }

        public eOrderStatus OrderStatus { get; set; } = eOrderStatus.Placed;

        public DateTime? OrderPlacedDate { get; set; } = DateTime.UtcNow;

        public DateTime? OrderShippedDate { get; set; }

        public DateTime? OrderDeliveredDate { get; set; }

        public DateTime? OrderCanceledDate { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentMethod { get; set; }
    }
}