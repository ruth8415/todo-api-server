using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת DbContext
var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// הוספת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                  "https://todolist-client-rnej.onrender.com",
                  "http://localhost:3000",
                  "http://localhost:5173"
              )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// הפעלת CORS - חייב להיות ראשון!
app.UseCors("AllowAll");

// הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Health check endpoint
app.MapGet("/", () => "API is running!")
   .RequireCors("AllowAll");

// Routes

// GET: שליפת כל המשימות
app.MapGet("/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
})
.WithName("GetAllItems")
.RequireCors("AllowAll");

// GET: שליפת משימה לפי ID
app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetItemById")
.RequireCors("AllowAll");

// POST: הוספת משימה חדשה
app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
})
.WithName("CreateItem")
.RequireCors("AllowAll");

// PUT: עדכון משימה מלא
app.MapPut("/items/{id}", async (int id, Item updatedItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
    {
        return Results.NotFound();
    }

    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateItem")
.RequireCors("AllowAll");

// PATCH: עדכון חלקי - רק isComplete
app.MapMethods("/items/{id}/complete", new[] { "PATCH" }, async (int id, ToDoDbContext db, bool isComplete) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
    {
        return Results.NotFound();
    }

    item.IsComplete = (sbyte)(isComplete ? 1 : 0);

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateItemComplete")
.RequireCors("AllowAll");

// DELETE: מחיקת משימה
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
    {
        return Results.NotFound();
    }

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteItem")
.RequireCors("AllowAll");

app.Run();
