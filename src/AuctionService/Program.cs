using AuctionService.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
}
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
try
{
    // Initialize the database with seed data
    DbInitializer.InitDb(app);
    // Apply migrations and seed data
    Console.WriteLine("Database migration and seeding completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error migrating database: {ex.Message}");
}

app.Run();
