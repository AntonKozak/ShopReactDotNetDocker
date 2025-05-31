
using Contracts;

using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreatedContract>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreatedContract>> context)
    {
        Console.WriteLine("AuctionCreatedFaultConsumer: Auction created fault event received.");

        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException")
        {
            Console.WriteLine("Faulted due to an ArgumentException.");
            await context.Publish(context.Message.Message,
                x => x.Headers.Set("FaultReason", "ArgumentException occurred"));
        }
        else
        {
            Console.WriteLine("Faulted due to an unexpected exception.");
        }

        await Task.CompletedTask; // Simulate async operation
    }
}
