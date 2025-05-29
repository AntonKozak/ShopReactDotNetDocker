
using AuctionService.Data;
using AuctionService.DTOs;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController(AuctionDbContext context, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly AuctionDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions
            .Include(a => a.Item)
            .ToListAsync();

        var listOfAuctions = _mapper.Map<List<AuctionDto>>(auctions);

        return listOfAuctions;
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
            .Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        var auctionDto = _mapper.Map<AuctionDto>(auction);
        return auctionDto;
    }
}
