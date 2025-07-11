using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlacedContract>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<BidPlacedContract> context)
    {
        Console.WriteLine($"Bid placed: {context.Message}");
        var bid = context.Message;
        await _hubContext.Clients.All.SendAsync("BidPlaced", bid);
    }
}
