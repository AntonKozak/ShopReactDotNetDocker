using Microsoft.AspNetCore.Mvc;

using MongoDB.Entities;

using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;


using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;

    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        _logger.LogInformation("=== SEARCH REQUEST DEBUG ===");
        _logger.LogInformation("Raw FilterBy: '{FilterBy}' (Length: {Length})",
            searchParams.FilterBy ?? "NULL",
            (searchParams.FilterBy ?? "").Length);
        _logger.LogInformation("Is FilterBy null or empty: {IsNullOrEmpty}",
            string.IsNullOrEmpty(searchParams.FilterBy));
        _logger.LogInformation("OrderBy: '{OrderBy}'", searchParams.OrderBy ?? "NULL");
        _logger.LogInformation("PageNumber: {PageNumber}, PageSize: {PageSize}",
            searchParams.PageNumber, searchParams.PageSize);

        var query = DB.PagedSearch<Item, Item>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make))
                .Sort(x => x.Ascending(a => a.Model)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)),
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd > DateTime.UtcNow
                && x.AuctionEnd <= DateTime.UtcNow.AddHours(6)),
            "live" => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
        };

        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        _logger.LogInformation("Query returned {Count} results", result.Results.Count);

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount,
        });
    }
}
