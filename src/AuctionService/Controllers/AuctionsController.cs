using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AuctionDbContext _context;
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {

        var query = _context.Auctions
            .OrderBy(x => x.Item!.Make).AsQueryable();
        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(a => a.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
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

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto);

        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();

        var createdAuctionDto = _mapper.Map<AuctionDto>(auction);
        return CreatedAtAction(nameof(GetAuctionById), new { id = createdAuctionDto.Id }, createdAuctionDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {

        var auction = await _context.Auctions.Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        auction.Item!.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;

        _context.Auctions.Update(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return NoContent();

        return StatusCode(500, "An error occurred while updating the auction.");

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction == null)
        {
            return NotFound();
        }

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return NoContent();

        return StatusCode(500, "An error occurred while deleting the auction.");
    }
}
