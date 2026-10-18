using Microsoft.EntityFrameworkCore;
using DemoApp.Data;
using DemoApp.Models;
using DemoApp.Services.Strategies;

namespace DemoApp.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;
    private readonly ITaskPriorityStrategy _priorityStrategy;

    public TaskService(AppDbContext context, ITaskPriorityStrategy priorityStrategy)
    {
        _context = context;
        _priorityStrategy = priorityStrategy;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.AsNoTracking().ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> CreateAsync(CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Tytul zadania nie moze byc pusty.");

        if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
            throw new InvalidOperationException("Termin wykonania zadania nie moze byc w przeszlosci.");

        var task = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description,
            DueDate = request.DueDate,
            Status = TaskItemStatus.Todo
        };

        task.Priority = _priorityStrategy.Calculate(task);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> ChangeStatusAsync(int id, TaskItemStatus status)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null) return false;

        if (task.Status == TaskItemStatus.Done && status == TaskItemStatus.Todo)
            throw new InvalidOperationException("Closed task cannot return to Todo without reopening.");

        task.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
