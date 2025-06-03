using Contracts;

using MassTransit;

using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlacedContract>
{
    public async Task Consume(ConsumeContext<BidPlacedContract> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId)
            ?? throw new MessageException(typeof(AuctionFinishedContract), "Cannot retrieve this auction");

        if (context.Message.BidStatus.Contains("Accepted")
            && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }
    }
}
