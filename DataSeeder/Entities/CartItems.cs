namespace DataSeeder.Entities
{
    public class CartItems : Base
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}