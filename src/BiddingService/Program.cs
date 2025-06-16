using BiddingService.Consumers;
using BiddingService.Services;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bidding-service", false));

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
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("bidDb", MongoClientSettings
.FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();
