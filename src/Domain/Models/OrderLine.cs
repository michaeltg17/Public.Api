namespace Domain.Models
{
    public class OrderLine : Entity
    {
        public long OrderId { get; init; }
        public long ProductId { get; init; }
        public int Quantity { get; init; }

        public virtual Product ProductNavigation { get; set; } = default!;
    }
}
