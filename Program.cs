using HotelBookingAPI.Data;
using HotelBookingAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------
// Configure Entity Framework Core DbContext
// ---------------------------------------
// Using In-Memory database named "HotelBookingDb"
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseInMemoryDatabase("HotelBookingDb"));

// ---------------------------------------
// Register AutoMapper
// ---------------------------------------
// Scans all assemblies in current AppDomain for AutoMapper profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ---------------------------------------
// Register application services for dependency injection
// ---------------------------------------
builder.Services.AddScoped<IServices, Services>();

// ---------------------------------------
// Add controllers for API endpoints
// ---------------------------------------
builder.Services.AddControllers();

// ---------------------------------------
// Configure Swagger/OpenAPI generation and UI
// ---------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Booking API",
        Version = "v1"
    });

    // Include XML comments in Swagger if XML documentation file exists
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// ---------------------------------------
// Configure HTTP request pipeline
// ---------------------------------------

// Enable Swagger UI only in all environment
app.UseSwagger();
app.UseSwaggerUI();

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization middleware (adjust as needed for your auth setup)
app.UseAuthorization();

// Map controller routes for API
app.MapControllers();

// Run the application
app.Run();
