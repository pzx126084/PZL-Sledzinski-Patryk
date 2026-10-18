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

// 1. DANE TESTOWE I CREATE
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

// 2. READ + ASNOTRACKING + 3 ZAPYTANIA LINQ
// LINQ 1: Where + AsNoTracking
var pendingTasks = await db.Tasks
    .AsNoTracking()
    .Include(t => t.Project)
    .Where(t => !t.IsCompleted)
    .ToListAsync();

// LINQ 2: OrderBy + AsNoTracking
var sortedUsers = await db.Users
    .AsNoTracking()
    .OrderBy(u => u.Name)
    .ToListAsync();

// LINQ 3: CountAsync
var completedCount = await db.Tasks
    .CountAsync(t => t.IsCompleted);

Console.WriteLine($"Niezrealizowane zadania: {pendingTasks.Count}, Zrealizowane: {completedCount}");

// 3. UPDATE
var taskToUpdate = await db.Tasks.FirstOrDefaultAsync(t => !t.IsCompleted);
if (taskToUpdate != null)
{
    taskToUpdate.IsCompleted = true;
    await db.SaveChangesAsync();
    Console.WriteLine($"Zaktualizowano zadanie: {taskToUpdate.Description}");
}

// 4. DELETE
var taskToDelete = await db.Tasks.FirstOrDefaultAsync(t => t.Description == "Skonfigurowac EF Core");
if (taskToDelete != null)
{
    db.Tasks.Remove(taskToDelete);
    await db.SaveChangesAsync();
    Console.WriteLine("Usunieto zadanie testowe.");
}
