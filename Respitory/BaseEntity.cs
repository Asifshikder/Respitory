namespace Respitory
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
        public DateTime? DeletedOn { get; set; }
    }
}