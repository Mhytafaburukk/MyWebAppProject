using Microsoft.EntityFrameworkCore;
using TaskApi;
using MyWebAppProject;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// EKLENEN 1 & 2
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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

// --- User Endpoints ---

app.MapGet("/api/users", async (TaskDbContext dbContext) =>
{
    try 
    {
        // Performance: AsNoTracking() gereksiz takip mekanizmasını kapatıp hızı artırır.
        var users = await dbContext.Users.AsNoTracking().ToListAsync();
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem("Veritabanı sorgusu sırasında bir hata oluştu: " + ex.Message);
    }
}).WithName("GetUsers");

app.MapGet("/api/users/{id}", async (int id, TaskDbContext dbContext) =>
{
    try
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null)
        {
            // Error Handling: Kullanıcı bulunamadığında 404 dön.
            return Results.NotFound(new { Message = $"ID'si {id} olan kullanıcı bulunamadı." });
        }
        return Results.Ok(user);
    }
    catch (Exception ex)
    {
        return Results.Problem("Kullanıcı getirilirken bir hata oluştu: " + ex.Message);
    }
}).WithName("GetUserById");

app.MapPost("/api/users", async (MyWebAppProject.Models.User user, TaskDbContext dbContext) =>
{
    // Validation: Boş isim ve geçersiz email kontrolü
    if (string.IsNullOrWhiteSpace(user.Name))
    {
        return Results.BadRequest(new { Message = "İsim alanı boş bırakılamaz." });
    }

    if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@") || !user.Email.Contains("."))
    {
        return Results.BadRequest(new { Message = "Geçerli bir email adresi girilmelidir." });
    }

    try 
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/api/users/{user.Id}", user);
    }
    catch (Exception ex)
    {
        return Results.Problem("Kullanıcı oluşturulurken bir hata oluştu: " + ex.Message);
    }
}).WithName("CreateUser");

// ----------------------

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

// EKLENEN 3
app.MapBlazorHub();

app.Run();
