using MassTransit;

using Polly;
using Polly.Extensions.Http;

using SearchService.Consumers;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetRetryPolicy());
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<AuctionUpdatedConsumer>();
    x.AddConsumersFromNamespaceContaining<AuctionDeletedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search-service", false));

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

        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("search-auction-updated", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));
            e.ConfigureConsumer<AuctionUpdatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("search-auction-deleted", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));
            e.ConfigureConsumer<AuctionDeletedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitializeAsync(app);
    }
    catch (System.Exception)
    {
        Console.WriteLine("An error occurred while initializing the database.");
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    => HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
