namespace DemoApp.Models;

public enum TaskItemStatus
{
    Todo,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
}

public record CreateTaskRequest(string Title, string? Description, DateTime? DueDate);
public record ChangeTaskStatusRequest(TaskItemStatus Status);
