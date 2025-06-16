using MassTransit;

using NotificationService.Consumers;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<AuctionFinishedConsumer>();
    x.AddConsumersFromNamespaceContaining<BidPlacedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification-service", false));

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

builder.Services.AddSignalR();


var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
