using Microsoft.AspNetCore.Mvc;

using MongoDB.Entities;

using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;


using Microsoft.Extensions.Logging;

using static SearchService.Controllers.SearchControllerHelpers;

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
        try
        {
            var query = DB.PagedSearch<Item, Item>();

            // Full-text search and fallback property-based search
            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
                // Fallback: also match against Model, Make, Color directly (case-insensitive, contains)
                query.Match(i =>
                    i.Make.ToLower().Contains(searchParams.SearchTerm.ToLower()) ||
                    i.Model.ToLower().Contains(searchParams.SearchTerm.ToLower()) ||
                    i.Color.ToLower().Contains(searchParams.SearchTerm.ToLower())
                );
            }

            // Sorting (support asc/desc)
            ApplySorting(query, searchParams.OrderBy, searchParams.Descending);

            // Filtering (status-based)
            ApplyFiltering(query, searchParams.FilterBy);

            // Property-based filters (case-insensitive)
            if (!string.IsNullOrEmpty(searchParams.Make))
                query.Match(i => i.Make.ToLower() == searchParams.Make.ToLower());
            if (!string.IsNullOrEmpty(searchParams.Model))
                query.Match(i => i.Model.ToLower() == searchParams.Model.ToLower());
            if (searchParams.Year.HasValue)
                query.Match(i => i.Year == searchParams.Year.Value);
            if (searchParams.Mileage.HasValue)
                query.Match(i => i.Mileage == searchParams.Mileage.Value);
            if (!string.IsNullOrEmpty(searchParams.Color))
                query.Match(i => i.Color.ToLower() == searchParams.Color.ToLower());

            // Additional filters (case-insensitive for Seller and Winner)
            if (!string.IsNullOrEmpty(searchParams.Seller))
                query.Match(i => i.Seller.ToLower() == searchParams.Seller.ToLower());

            if (!string.IsNullOrEmpty(searchParams.Winner))
                query.Match(i => i.Winner != null && i.Winner.ToLower() == searchParams.Winner.ToLower());

            // Paging
            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            var result = await query.ExecuteAsync();

            return Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchItems");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
