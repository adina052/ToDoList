
using TodoApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenPolicy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
});

builder.Services.AddDbContext<ToDoDBContext>();
var app = builder.Build();

app.UseCors("OpenPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/items", (ToDoDBContext context) =>
{
    return context.Items.ToList();
});

app.MapPost("/items", async (ToDoDBContext context, Item item) =>
{
    context.Add(item);
    await context.SaveChangesAsync();
    return item;
});

app.MapPut("/items/{id}", async (ToDoDBContext context, [FromBody] Item item, int id) =>
{
    var exsistItem = await context.Items.FindAsync(id);
    if (exsistItem is null) return Results.NotFound();

    //exsistItem.Name = item.Name;
    exsistItem.IsComplete = item.IsComplete;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (ToDoDBContext context, int id) =>
{
    var exsistItem = await context.Items.FindAsync(id);
    if (exsistItem is null) return Results.NotFound();

    context.Items.Remove(exsistItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();