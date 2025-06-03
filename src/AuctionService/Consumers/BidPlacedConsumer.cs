using AuctionService.Data;

using Contracts;

using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer(AuctionDbContext dbContext) : IConsumer<BidPlacedContract>
{
    public async Task Consume(ConsumeContext<BidPlacedContract> context)
    {
        Console.WriteLine("--> Consuming Bid Placed");

        var auction = await dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

        if (auction != null && (auction.CurrentHighBid == 0
            || context.Message.BidStatus.Contains("Accepted")
            && context.Message.Amount > auction.CurrentHighBid))
        {
            auction.CurrentHighBid = context.Message.Amount;
            await dbContext.SaveChangesAsync();
        }
    }
}
