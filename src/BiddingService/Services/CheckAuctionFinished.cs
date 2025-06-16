using System;

using BiddingService.Models;

using Contracts;

using MassTransit;

using MongoDB.Entities;

namespace BiddingService.Services;

public class CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting check for finished auctions");

        stoppingToken.Register(() => logger.LogInformation("==> Auction check stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAuctions(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }
    private async Task CheckAuctions(CancellationToken stoppingToken)
    {
        try
        {
            var finishedAuctions = await DB.Find<Auction>()
                .Match(x => x.AuctionEnd <= DateTime.UtcNow)
                .Match(x => !x.Finished)
                .ExecuteAsync(stoppingToken);

            if (finishedAuctions.Count == 0) return;

            logger.LogInformation("==> Found {count} auctions that have completed", finishedAuctions.Count);

            using var scope = services.CreateScope();
            var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>(); foreach (var auction in finishedAuctions)
            {
                try
                {
                    auction.Finished = true;
                    await auction.SaveAsync(null, stoppingToken);

                    var winningBid = await DB.Find<Bid>()
                        .Match(a => a.AuctionId == auction.ID)
                        .Match(b => b.BidStatus == BidStatus.Accepted)
                        .Sort(x => x.Descending(s => s.Amount))
                        .ExecuteFirstAsync(stoppingToken);

                    if (winningBid != null)
                    {
                        logger.LogInformation("==> Auction {auctionId} finished with winning bid of {amount} by {bidder}",
                            auction.ID, winningBid.Amount, winningBid.Bidder);
                    }
                    else
                    {
                        logger.LogInformation("==> Auction {auctionId} finished with no bids", auction.ID);
                    }

                    await endpoint.Publish(new AuctionFinishedContract
                    {
                        ItemSold = winningBid != null,
                        AuctionId = auction.ID,
                        Winner = winningBid?.Bidder ?? "No winner",
                        Amount = winningBid?.Amount ?? 0,
                        Seller = auction.Seller
                    }, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "==> Error processing finished auction {auctionId}", auction.ID);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "==> Error checking for finished auctions");
        }
    }
}
