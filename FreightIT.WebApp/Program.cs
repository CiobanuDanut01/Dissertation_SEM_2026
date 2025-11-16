using FreightIT.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- Add our "Phone Service" (HttpClientFactory) ---

// This is a "helper function" to make registering phones easier
void RegisterHttpClient(string name)
{
    var url = builder.Configuration[$"{name}Url"]; // e.g., "DriverServiceUrl"
    builder.Services.AddHttpClient(name, client =>
    {
        if (url != null) client.BaseAddress = new Uri(url);
    });
}

// Now we just call the helper for each service
RegisterHttpClient("DriverService");
RegisterHttpClient("TruckService");
RegisterHttpClient("InvoiceService"); // <-- NEW
RegisterHttpClient("OrderService");   // <-- NEW
RegisterHttpClient("RouteService");   // <-- NEW

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();