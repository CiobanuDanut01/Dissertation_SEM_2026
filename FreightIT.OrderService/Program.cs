using Microsoft.EntityFrameworkCore;
using FreightIT.OrderService;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// 1. "Hire" the Staff (Services)
var connectionString = builder.Configuration.GetConnectionString("OrderDB");
builder.Services.AddDbContext<OrderDbContext>(options =>
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

// --- ORDER #1: "Get me ALL transport orders" (READ ALL) ---
app.MapGet("/orders", async (OrderDbContext db) =>
{
    return await db.TransportOrders.ToListAsync();
});

// --- ORDER #2: "Create a new transport order" (CREATE) ---
app.MapPost("/orders", async (TransportOrder order, OrderDbContext db) =>
{
    // API Validation using MiniValidation
    if (!MiniValidator.TryValidate(order, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    db.TransportOrders.Add(order);
    await db.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order);
});

// --- ORDER #3: "Update a transport order" (UPDATE) ---
// NEW: This endpoint updates the Status and RecommendedRoute of an existing order
app.MapPut("/orders/{id}", async (int id, TransportOrder updatedOrder, OrderDbContext db) =>
{
    // First, find the existing order
    var existingOrder = await db.TransportOrders.FindAsync(id);

    if (existingOrder is null)
    {
        // If we can't find it, return a "Not Found" error
        return Results.NotFound();
    }

    // Update the Status and RecommendedRoute properties
    // These are the fields that can be modified (e.g., by AI or user)
    existingOrder.Status = updatedOrder.Status;
    existingOrder.RecommendedRoute = updatedOrder.RecommendedRoute;

    // Save the changes to the database
    await db.SaveChangesAsync();

    // Return the updated order
    return Results.Ok(existingOrder);
});

// --- ORDER #4: "Delete a transport order" (DELETE) ---
app.MapDelete("/orders/{id}", async (int id, OrderDbContext db) =>
{
    // First, find the order
    var order = await db.TransportOrders.FindAsync(id);

    if (order is null)
    {
        // If we can't find it, return a "Not Found" error
        return Results.NotFound();
    }

    // If we find it, "Remove" it and "Save" the change
    db.TransportOrders.Remove(order);
    await db.SaveChangesAsync();

    // Return a "No Content" (204) success message
    return Results.NoContent();
});

// 3. Start the "Shift"
app.Run();

