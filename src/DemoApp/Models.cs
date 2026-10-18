using System.ComponentModel.DataAnnotations;

namespace DemoApp;

public class User
{
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public List<TaskItem> Tasks { get; set; } = new();
}

public class Project
{
    public int Id { get; set; }
    
    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    
    public List<TaskItem> Tasks { get; set; } = new();
}

public class TaskItem
{
    public int Id { get; set; }
    
    [Required, MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
