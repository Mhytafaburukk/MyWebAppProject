using Microsoft.EntityFrameworkCore;
using TaskApi;
using MyWebAppProject;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/tasks", async (TaskDbContext dbContext) =>
{
    var tasks = await dbContext.Tasks.ToListAsync();
    return Results.Ok(tasks);
}).WithName("GetTasks");

app.MapGet("/api/tasks/{id}", async (int id, TaskDbContext dbContext) =>
{
    var task = await dbContext.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(task);
}).WithName("GetTaskById");

app.MapPost("/api/tasks", async (TaskItem task, TaskDbContext dbContext) =>
{
    dbContext.Tasks.Add(task);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
}).WithName("CreateTask");

app.MapPut("/api/tasks/{id}", async (int id, TaskItem updatedTask, TaskDbContext dbContext) =>
{
    var task = await dbContext.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }

    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.IsCompleted = updatedTask.IsCompleted;

    await dbContext.SaveChangesAsync();
    return Results.Ok(task);
}).WithName("UpdateTask");

app.MapDelete("/api/tasks/{id}", async (int id, TaskDbContext dbContext) =>
{
    var task = await dbContext.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }

    dbContext.Tasks.Remove(task);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
}).WithName("DeleteTask");

app.MapGet("/api/products", () =>
{
    var products = new[]
    {
        new { Id = 1, Name = "Product 1", Price = 10.99 },
        new { Id = 2, Name = "Product 2", Price = 19.99 },
        new { Id = 3, Name = "Product 3", Price = 5.99 }
    };
    return Results.Ok(products);
}).WithName("GetProducts");

app.MapPost("/api/products", (Product product) =>
{
    var newProduct = new Product
    {
        Id = new Random().Next(1000, 9999),
        Name = product.Name,
        Price = product.Price
    };
    return Results.Created($"/api/products/{newProduct.Id}", newProduct);
}).WithName("CreateProduct");

app.MapPut("/api/products/{id}", (int id, Product updatedProduct) =>
{
    var product = new Product
    {
        Id = id,
        Name = updatedProduct.Name,
        Price = updatedProduct.Price
    };
    return Results.Ok(product);
}).WithName("UpdateProduct");

app.MapDelete("/api/products/{id}", (int id) =>
{
    return Results.NoContent();
}).WithName("DeleteProduct");

app.Run();
// Get all products
