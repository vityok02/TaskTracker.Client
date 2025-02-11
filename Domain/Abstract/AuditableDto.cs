namespace Domain.Abstract;

public class AuditableDto
{
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdateBy { get; set; }
}
