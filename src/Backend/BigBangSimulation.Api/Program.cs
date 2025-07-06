using Microsoft.OpenApi.Models;
using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Infrastructure.Services;
using BigBangSimulation.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Application and Infrastructure Services manually
builder.Services.AddScoped<IBigBangSimulationService, BigBangSimulation.Application.Services.BigBangSimulationService>();
builder.Services.AddScoped<IQuantumComputingService, BigBangSimulation.Infrastructure.Services.QuantumComputingService>();
builder.Services.AddScoped<ISimulationLogger, SimulationLogger>();
builder.Services.AddScoped<ISimulationCache, MemorySimulationCache>();
builder.Services.AddScoped<ISimulationConfiguration, SimulationConfiguration>();
builder.Services.AddMemoryCache();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantum Big Bang Simulation API",
        Version = "v1",
        Description = "Kuantum algoritmalarıyla Big Bang simülasyonu yapan API",
        Contact = new OpenApiContact
        {
            Name = "Big Bang Simulation Team",
            Email = "info@bigbangsimulation.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantum Big Bang Simulation API v1");
        c.RoutePrefix = string.Empty; // Swagger'ı root'ta aç
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

// Use routing
app.UseRouting();

// Map controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/api/health", () => new
{
    Message = "🚀 Quantum Big Bang Simulation API",
    Version = "1.0.0",
    Status = "Running",
    Timestamp = DateTime.UtcNow,
    Documentation = "/swagger"
});

app.Run();
