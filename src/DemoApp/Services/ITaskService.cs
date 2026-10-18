using DemoApp.Models;

namespace DemoApp.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(CreateTaskRequest request);
    Task<bool> ChangeStatusAsync(int id, TaskItemStatus status);
    Task<bool> DeleteAsync(int id);
}
