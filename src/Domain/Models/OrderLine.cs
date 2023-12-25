namespace Domain.Models
{
    public class OrderLine : Entity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Product ProductNavigation { get; set; } = default!;
    }
}
