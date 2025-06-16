using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreatedContract>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionCreatedContract> context)
    {
        Console.WriteLine($"Auction created: {context.Message.Id}");
        var auction = context.Message;
        await _hubContext.Clients.All.SendAsync("AuctionCreated", auction);
    }
}
