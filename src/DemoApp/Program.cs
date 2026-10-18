using Microsoft.EntityFrameworkCore;
using DemoApp.Data;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.Services.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskFlowDb"));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskPriorityStrategy, DueDatePriorityStrategy>();

var app = builder.Build();

app.MapGet("/tasks", async (ITaskService service) =>
{
    var tasks = await service.GetAllAsync();
    return Results.Ok(tasks);
});

app.MapGet("/tasks/{id:int}", async (int id, ITaskService service) =>
{
    var task = await service.GetByIdAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

app.MapPost("/tasks", async (CreateTaskRequest request, ITaskService service) =>
{
    try
    {
        var created = await service.CreateAsync(request);
        return Results.Created($"/tasks/{created.Id}", created);
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPatch("/tasks/{id:int}/status", async (int id, ChangeTaskStatusRequest request, ITaskService service) =>
{
    try
    {
        var result = await service.ChangeStatusAsync(id, request.Status);
        return result ? Results.NoContent() : Results.NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapDelete("/tasks/{id:int}", async (int id, ITaskService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
