using Microsoft.EntityFrameworkCore;
using FreightIT.TruckService;
using MiniValidation; // <-- NEW: Import the validation tool

var builder = WebApplication.CreateBuilder(args);

// 1. "Hire" the Staff (Services)
var connectionString = builder.Configuration.GetConnectionString("TruckDB");
builder.Services.AddDbContext<TruckDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Define the "Order Pad" (Endpoints)
// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- ORDER #1: "Get me ALL trucks" (READ ALL) ---
app.MapGet("/trucks", async (TruckDbContext db) =>
{
    return await db.Trucks.ToListAsync();
});

// --- ORDER #2: "Create a new truck" (CREATE) ---
app.MapPost("/trucks", async (Truck truck, TruckDbContext db) =>
{
    // --- NEW: API Validation ---
    // This checks our [Required] sticky notes.
    if (!MiniValidator.TryValidate(truck, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    db.Trucks.Add(truck);
    await db.SaveChangesAsync();
    return Results.Created($"/trucks/{truck.Id}", truck);
});

// --- NEW: ORDER #3: "Delete a truck" (DELETE) ---
// We identify *which* truck to delete by its 'id' in the URL
app.MapDelete("/trucks/{id}", async (int id, TruckDbContext db) =>
{
    // First, find the truck
    var truck = await db.Trucks.FindAsync(id);

    if (truck is null)
    {
        // If we can't find it, return a "Not Found" error
        return Results.NotFound();
    }

    // If we find it, "Remove" it and "Save" the change
    db.Trucks.Remove(truck);
    await db.SaveChangesAsync();

    // Return a "No Content" (204) success message. This is standard for DELETE.
    return Results.NoContent();
});

// 3. Start the "Shift"
app.Run();