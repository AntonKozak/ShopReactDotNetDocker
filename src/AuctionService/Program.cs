using AuctionService.Consumers;
using AuctionService.Data;
using AuctionService.Services;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.AddConsumersFromNamespaceContaining<BidPlacedConsumer>();
    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction-service", false));

    x.UsingRabbitMq((context, cfg) =>
   {
       cfg.UseMessageRetry(r =>
       {
           r.Handle<RabbitMqConnectionException>();
           r.Interval(5, TimeSpan.FromSeconds(10));
       });

       cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
       {
           h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
           h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
       });
       cfg.ConfigureEndpoints(context);
   });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false; // Set to true in production
        options.TokenValidationParameters.ValidateAudience = false; // Disable audience validation for simplicity
        options.TokenValidationParameters.NameClaimType = "username";

    });

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcAuctionService>();

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
