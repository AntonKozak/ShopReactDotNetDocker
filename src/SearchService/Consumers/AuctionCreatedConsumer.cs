using AutoMapper;

using Contracts;

using MassTransit;

using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreatedContract>
{
    private readonly IMapper _mapper;
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;

    }
    public async Task Consume(ConsumeContext<AuctionCreatedContract> context)
    {
        Console.WriteLine("AuctionCreatedConsumer: Auction created event received.");
        var item = _mapper.Map<Item>(context.Message);

        await item.SaveAsync();
    }
}
