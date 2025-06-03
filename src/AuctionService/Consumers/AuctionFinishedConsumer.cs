using AuctionService.Data;
using AuctionService.Entities;

using Contracts;

using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer(AuctionDbContext dbContext) : IConsumer<AuctionFinishedContract>
{
    public async Task Consume(ConsumeContext<AuctionFinishedContract> context)
    {
        var auction = await dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId))
            ?? throw new InvalidOperationException($"Auction with ID {context.Message.AuctionId} not found.");

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        auction.Status = auction.SoldAmount > auction.ReservePrice
            ? Status.Finished
            : Status.ReserveNotMet;

        await dbContext.SaveChangesAsync();
    }
}
