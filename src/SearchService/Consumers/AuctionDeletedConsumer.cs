using Contracts;

using MassTransit;

using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeletedContract>
{
    public async Task Consume(ConsumeContext<AuctionDeletedContract> context)
    {
        Console.WriteLine("AuctionDeletedConsumer: Auction deleted event received.");
        var contract = context.Message;
        await DB.DeleteAsync<Item>(contract.Id);
    }
}
