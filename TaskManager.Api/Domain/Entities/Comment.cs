namespace TaskManager.Api.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public required string AuthorName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TaskItemId { get; set; }
    public TaskItem? TaskItem { get; set; }
}
