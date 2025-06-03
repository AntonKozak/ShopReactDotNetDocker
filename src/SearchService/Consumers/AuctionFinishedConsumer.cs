using Contracts;

using MassTransit;

using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinishedContract>
{
    public async Task Consume(ConsumeContext<AuctionFinishedContract> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId)
            ?? throw new MessageException(typeof(AuctionFinishedContract), "Cannot retrieve this auction");

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message?.Winner;
            auction.SoldAmount = context.Message?.Amount ?? 0;
        }

        auction.Status = "Finished";

        await auction.SaveAsync();
    }
}
