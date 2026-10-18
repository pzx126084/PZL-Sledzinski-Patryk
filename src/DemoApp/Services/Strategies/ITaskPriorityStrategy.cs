using DemoApp.Models;

namespace DemoApp.Services.Strategies;

public interface ITaskPriorityStrategy
{
    TaskPriority Calculate(TaskItem task);
}
