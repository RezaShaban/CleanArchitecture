namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public long? LastUpdateUserId { get; set; }
    }
}
