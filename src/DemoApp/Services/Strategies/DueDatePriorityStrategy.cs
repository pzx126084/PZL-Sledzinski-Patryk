using DemoApp.Models;

namespace DemoApp.Services.Strategies;

public class DueDatePriorityStrategy : ITaskPriorityStrategy
{
    public TaskPriority Calculate(TaskItem task)
    {
        if (task.DueDate is null)
            return TaskPriority.Low;

        var hoursLeft = (task.DueDate.Value - DateTime.UtcNow).TotalHours;

        return hoursLeft switch
        {
            <= 24 => TaskPriority.Critical,
            <= 72 => TaskPriority.High,
            _ => TaskPriority.Medium
        };
    }
}
