using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connectionString = builder.Configuration.GetSection("MongoDBSettings").GetValue<string>("ConnectionURI");
string collectionName = builder.Configuration.GetSection("MongoDBSettings").GetValue<string>("CollectionName");
string databaseName = builder.Configuration.GetSection("MongoDBSettings").GetValue<string>("DatabaseName");

builder.Services.AddTransient<MongoClient>((_provider) => new MongoClient(connectionString));
var app = builder.Build();
var PORT = Environment.GetEnvironmentVariable("PORT");

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/api/todos", async (MongoClient connection) =>
{
    try
    {

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<Todo>(collectionName);
        var results = await collection.Find(_ => true).ToListAsync().ConfigureAwait(false);

        return Results.Ok(results);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

app.MapGet("/api/todos{name}", async (string name, MongoClient connection) =>
{
    try
    {

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<Todo>(collectionName);
        var result = await collection.Find(record => record.Name == name).FirstOrDefaultAsync();

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

app.MapPost("/api/todos", async (Todo record, MongoClient connection) =>
{
    try
    {
        var database = connection.GetDatabase("tododb");
        var collection = database.GetCollection<Todo>(collectionName);
        await collection.InsertOneAsync(record).ConfigureAwait(false);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

app.MapDelete("/api/todos{id}", async (string id, MongoClient connection) =>
{
    try
    {

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<Todo>(collectionName);
        var result = await collection.DeleteOneAsync(record => record._id == id);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

app.MapGet("/", () => "Hello World!");

// app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.ToListAsync());
// app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
// {

//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return Results.Created($"/todoitems/{todo.Id}", todo);
// });

// app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.NoContent();
//     }

//     return Results.NotFound();
// });
app.MapGet("/sum", (int? n1, int? n2) => n1 + n2);

app.Run();
