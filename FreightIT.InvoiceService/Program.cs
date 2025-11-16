using Microsoft.EntityFrameworkCore;
using FreightIT.InvoiceService;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// 1. "Hire" the Staff (Services)
var connectionString = builder.Configuration.GetConnectionString("InvoiceDB");
builder.Services.AddDbContext<InvoiceDbContext>(options =>
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

// --- ORDER #1: "Get me ALL invoices" (READ ALL) ---
app.MapGet("/invoices", async (InvoiceDbContext db) =>
{
    return await db.Invoices.ToListAsync();
});

// --- ORDER #2: "Create a new invoice" (CREATE) ---
app.MapPost("/invoices", async (Invoice invoice, InvoiceDbContext db) =>
{
    // API Validation using MiniValidation
    if (!MiniValidator.TryValidate(invoice, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    db.Invoices.Add(invoice);
    await db.SaveChangesAsync();
    return Results.Created($"/invoices/{invoice.Id}", invoice);
});

// --- ORDER #3: "Delete an invoice" (DELETE) ---
app.MapDelete("/invoices/{id}", async (int id, InvoiceDbContext db) =>
{
    // First, find the invoice
    var invoice = await db.Invoices.FindAsync(id);

    if (invoice is null)
    {
        // If we can't find it, return a "Not Found" error
        return Results.NotFound();
    }

    // If we find it, "Remove" it and "Save" the change
    db.Invoices.Remove(invoice);
    await db.SaveChangesAsync();

    // Return a "No Content" (204) success message
    return Results.NoContent();
});

// 3. Start the "Shift"
app.Run();

