using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProductSku: AuditableEntity, ISoftDelete, IHasDomainEvent
    {
        public long Id { get; set; }
        public string Sku { get; set; }
        public decimal SalePrice { get; set; }
        public long ProductId { get; set; }
        public bool IsDeleted { get; set; }
        public Product Product { get; set; }

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
