namespace TaskManager.Api.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string StatusName { get; set; }
    public DateTime? DueDate { get; set; }
    public int CommentsCount { get; set; }
}
