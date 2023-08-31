using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product : AuditableEntity, ISoftDelete, IHasDomainEvent
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string Code { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductSku>? ProductSkus { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
