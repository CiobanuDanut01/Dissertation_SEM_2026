// Import the "translator" (EF Core) and our "Driver" blueprint
using Microsoft.EntityFrameworkCore;
using FreightIT.DriverService; // This is your 'Driver.cs' and 'DriverDbContext.cs'

// 1. === SET UP THE "RESTAURANT" (The Web App Builder) ===

// This creates the "builder" for our new Web API application
var builder = WebApplication.CreateBuilder(args);

// 2. === HIRE THE "STAFF" (Configure Services) ===
// This is where we tell our app what "tools" it has.

// --- "Hire" the Head Librarian (DriverDbContext) ---
// First, get the "database address" we saved in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DriverDB");

// Now, tell the app how to "hire" the DriverDbContext.
// This says: "When someone asks for a DriverDbContext, create one,
// and give it these instructions: 'Use the Npgsql translator' and
// 'Use this connectionString as the address'."
builder.Services.AddDbContext<DriverDbContext>(options =>
    options.UseNpgsql(connectionString));


// --- Add a "Test Menu" service (Swagger) ---
// These two lines add a service that automatically
// builds a "test menu" webpage for our API. This is
// an amazing tool for testing our "waiter."
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 3. === OPEN THE "RESTAURANT" ===

// This "builds" the app with all the "staff" we hired.
var app = builder.Build();


// 4. === DEFINE THE "WAITER'S ORDER PAD" (The Endpoints) ===
// This is the "menu" of "orders" our waiter can take.

// This tells the app: "Use the 'Test Menu' (Swagger) page."
app.UseSwagger();
app.UseSwaggerUI();

// This handles the 'https' redirection we set up in Phase 1
app.UseHttpsRedirection(); 

// --- ORDER #1: "Get me a list of ALL drivers" ---
// This says: "If a 'GET' (read) request comes to the URL '/drivers',
// run the code inside the curly braces."
app.MapGet("/drivers", async (DriverDbContext db) =>
{
    // The app "hires" a 'db' (Head Librarian) for us.
    // We say: "Librarian, go to your 'Drivers' collection
    // and give us a List of everything."
    return await db.Drivers.ToListAsync();
});

// --- ORDER #2: "Create a new driver" ---
// This says: "If a 'POST' (create) request comes to the URL '/drivers',
// it will contain a 'driver' object in its 'body'. Run this code."
app.MapPost("/drivers", async (Driver driver, DriverDbContext db) =>
{
    // We tell the Librarian: "Add this new 'driver' to your 'Drivers' collection."
    // (This doesn't save yet, it just stages it).
    db.Drivers.Add(driver);

    // Now, we tell the Librarian: "Save all your changes to the
    // actual database 'filing cabinet'."
    await db.SaveChangesAsync();

    // We return a "Success" message to the "customer," letting them
    // know the new driver's ID.
    return Results.Created($"/drivers/{driver.Id}", driver);
});


// 5. === START THE "WAITER'S SHIFT" ===

// This tells the app to "start listening" for
// "orders" (HTTP requests) at its URL.
app.Run();
