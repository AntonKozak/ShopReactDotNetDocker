using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinishedContract>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinishedContract> context)
    {
        Console.WriteLine($"Auction finished: {context.Message}");
        var auction = context.Message;
        await _hubContext.Clients.All.SendAsync("AuctionFinished", auction);
    }
}
