using AuctionService;

using BiddingService.Models;

using Grpc.Net.Client;

namespace BiddingService.Services;

public class GrpcAuctionClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GrpcAuctionClient> _logger;
    public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Auction GetAuction(string id)
    {
        _logger.LogInformation("==> Received Grpc request for auction with id: {Id}", id);

        var channel = GrpcChannel.ForAddress(_configuration["GrpcAuction"]);

        var client = new GrpcAuction.GrpcAuctionClient(channel);

        var request = new GetAuctionRequest { Id = id };

        try
        {
            var reply = client.GetAuction(request);
            var auction = new Auction
            {
                ID = reply.Auction.Id,
                AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                Seller = reply.Auction.Seller,
                ReservePrice = reply.Auction.ReservePrice
            };
            _logger.LogInformation("==> Grpc request for auction with id: {Id} completed successfully", id);
            return auction;
        }
        catch (Exception ex)
        {
            _logger.LogError("==> Grpc request for auction with id: {Id} failed", id);
            _logger.LogError("==> Exception: {Message}", ex.Message);
            return null;
        }

    }
}
