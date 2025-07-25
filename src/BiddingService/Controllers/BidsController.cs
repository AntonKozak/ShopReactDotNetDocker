using AutoMapper;

using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;

using Contracts;

using MassTransit;
using MassTransit.Testing;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcAuctionClient _grpcAuctionClient;
    public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcAuctionClient)
    {
        _grpcAuctionClient = grpcAuctionClient;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
    {
        var auction = await DB.Find<Auction>()
            .OneAsync(auctionId);

        if (auction == null)
        {
            auction = _grpcAuctionClient.GetAuction(auctionId);
            if (auction == null)
            {
                return BadRequest("Auction not found.");
            }

        }

        if (auction.Seller == User.Identity?.Name)
        {
            return BadRequest("You cannot bid on your own auction.");
        }

        var bid = new Bid
        {
            AuctionId = auctionId,
            Amount = amount,
            Bidder = User.Identity?.Name ?? "Unknown",
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()

            .Match(b => b.AuctionId == auctionId)
            .Sort(b => b.Descending(b => b.Amount))
            .ExecuteFirstAsync();

            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice
                ? BidStatus.Accepted
                : BidStatus.AcceptedBelowReserve;
            }

            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);
        await _publishEndpoint.Publish(_mapper.Map<BidPlacedContract>(bid));
        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(a => a.BidTime))
            .ExecuteAsync();

        if (bids == null)
            return NotFound("No bids found for this auction. GetBidsForAuction BiddsController");

        return Ok(bids.Select(_mapper.Map<BidDto>).ToList());
    }


}
