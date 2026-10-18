using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DemoApp;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
    })
    .Build();

using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

await db.Database.MigrateAsync();

if (!await db.Users.AnyAsync())
{
    var user = new User { Name = "Patryk Sledzinski" };
    var project = new Project { Title = "System Zarzadzania Zadaniami" };

    var task1 = new TaskItem { Description = "Skonfigurowac EF Core", IsCompleted = true, User = user, Project = project };
    var task2 = new TaskItem { Description = "Zaimplementowac CRUD", IsCompleted = false, User = user, Project = project };

    await db.AddRangeAsync(user, project, task1, task2);
    await db.SaveChangesAsync();
    Console.WriteLine("Dodano dane testowe.");
}

var pendingTasks = await db.Tasks
    .AsNoTracking()
    .Include(t => t.Project)
    .Where(t => !t.IsCompleted)
    .ToListAsync();

var sortedUsers = await db.Users
    .AsNoTracking()
    .OrderBy(u => u.Name)
    .ToListAsync();

var completedCount = await db.Tasks
    .CountAsync(t => t.IsCompleted);

Console.WriteLine($"Niezrealizowane zadania: {pendingTasks.Count}, Zrealizowane: {completedCount}");

var taskToUpdate = await db.Tasks.FirstOrDefaultAsync(t => !t.IsCompleted);
if (taskToUpdate != null)
{
    taskToUpdate.IsCompleted = true;
    await db.SaveChangesAsync();
    Console.WriteLine($"Zaktualizowano zadanie: {taskToUpdate.Description}");
}

var taskToDelete = await db.Tasks.FirstOrDefaultAsync(t => t.Description == "Skonfigurowac EF Core");
if (taskToDelete != null)
{
    db.Tasks.Remove(taskToDelete);
    await db.SaveChangesAsync();
    Console.WriteLine("Usunieto zadanie testowe.");
}
