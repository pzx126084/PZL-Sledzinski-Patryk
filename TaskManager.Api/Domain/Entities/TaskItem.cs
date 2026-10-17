using TaskManager.Api.Domain.Enums;

namespace TaskManager.Api.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
