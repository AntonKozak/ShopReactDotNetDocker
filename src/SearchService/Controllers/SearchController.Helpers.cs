using MongoDB.Entities;

using SearchService.Models;

namespace SearchService.Controllers;

public static class SearchControllerHelpers
{
    public static void ApplySorting(PagedSearch<Item, Item> query, string? orderBy, bool descending = false)
    {
        switch (orderBy?.ToLower())
        {
            case "make":
                if (descending)
                    query.Sort(x => x.Descending(i => i.Make));
                else
                    query.Sort(x => x.Ascending(i => i.Make));
                break;
            case "model":
                if (descending)
                    query.Sort(x => x.Descending(i => i.Model));
                else
                    query.Sort(x => x.Ascending(i => i.Model));
                break;
            case "mileage":
                if (descending)
                    query.Sort(x => x.Descending(i => i.Mileage));
                else
                    query.Sort(x => x.Ascending(i => i.Mileage));
                break;
            case "seller":
                if (descending)
                    query.Sort(x => x.Descending(i => i.Seller));
                else
                    query.Sort(x => x.Ascending(i => i.Seller));
                break;
            default:
                if (descending)
                    query.Sort(x => x.Descending(i => i.AuctionEnd));
                else
                    query.Sort(x => x.Ascending(i => i.AuctionEnd));
                break;
        }
    }

    public static void ApplyFiltering(PagedSearch<Item, Item> query, string? filterBy)
    {
        switch (filterBy?.ToLower())
        {
            case "finished":
                query.Match(i => i.AuctionEnd < DateTime.UtcNow);
                break;
            case "endingsoon":
                query.Match(i => i.AuctionEnd > DateTime.UtcNow && i.AuctionEnd < DateTime.UtcNow.AddHours(6));
                break;
            case "new":
                query.Match(i => i.AuctionEnd > DateTime.UtcNow && i.CreatedAt > DateTime.UtcNow.AddDays(-1));
                break;
            default:
                query.Match(i => i.AuctionEnd > DateTime.UtcNow);
                break;
        }
    }
}
