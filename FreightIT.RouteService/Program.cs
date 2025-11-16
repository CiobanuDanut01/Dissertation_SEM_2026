using FreightIT.RouteService.Plugins;

// 1. === SET UP THE "RESTAURANT" ===
var builder = WebApplication.CreateBuilder(args);

// 2. === HIRE THE "STAFF" ===

// --- "Hire" our Route Plugin as a singleton ---
builder.Services.AddSingleton<RoutePlugin>(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. === DEFINE THE "WAITER'S ORDER PAD" ===

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// This is our route recommendation endpoint
app.MapPost("/route/recommend", (
    RouteRequest request,    // The "order" (Origin/Destination)
    RoutePlugin routePlugin  // Our route plugin (injected)
) =>
{
    // Call the GetRouteRecommendation method directly
    var recommendation = routePlugin.GetRouteRecommendation(
        request.Origin, 
        request.Destination
    );

    // Return the recommendation as plain text
    return Results.Ok(recommendation);
});

// 4. === START THE "SHIFT" ===
app.Run();

// This is a "helper class" to define what our "order" (request) looks like.
public record RouteRequest(string Origin, string Destination);

