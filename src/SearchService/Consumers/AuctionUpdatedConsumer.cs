using AutoMapper;

using Contracts;

using MassTransit;

using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdatedContract>
{
    private readonly IMapper _mapper;
    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdatedContract> context)
    {
        Console.WriteLine("AuctionUpdatedConsumer: Auction updated event received.");
        var contract = context.Message;
        var item = await DB.Find<Item>().OneAsync(contract.Id);
        if (item == null) return;
        // Update fields
        item.Make = contract.Make;
        item.Model = contract.Model;
        item.Year = contract.Year;
        item.Color = contract.Color;
        item.Mileage = contract.Mileage;
        item.UpdatedAt = DateTime.UtcNow;
        await item.SaveAsync();
    }
}
