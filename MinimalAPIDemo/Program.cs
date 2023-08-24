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
var ConnectionURI = Environment.GetEnvironmentVariable("ConnectionURI");

// var PORT = Environment.GetEnvironmentVariable("PORT");
// var APP_URI = Environment.GetEnvironmentVariable("APP_URI");
// Console.WriteLine($"the application is running port {APP_URI} on {PORT}");
var app = builder.Build();

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

app.MapGet("/api/todos{id}", async (string id, MongoClient connection) =>
{
    try
    {

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<Todo>(collectionName);
        var result = await collection.Find(record => record._id == id).FirstOrDefaultAsync();

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

app.MapGet("/api/health", () => "running");

// if (string.IsNullOrWhiteSpace(APP_URI))
// {
//     app.Run();
// }
// else
// {
//     app.Run(APP_URI + ":" + PORT);
// }

app.Run();
