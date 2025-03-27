namespace Domain.Models;

public class UpdateTaskStateModel
{
    public Guid StateId { get; set; }
    public Guid? BeforeTaskId { get; set; }
}
