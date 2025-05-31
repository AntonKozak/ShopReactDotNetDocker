namespace SearchService.RequestHelpers;

public class SearchParams
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Seller { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public string OrderBy { get; set; } = string.Empty;
    public string FilterBy { get; set; } = string.Empty;
    public bool Ascending { get; set; } = true;
    public bool Descending { get; set; } = false;
    public bool IncludeDeleted { get; set; } = false;

    // Property-based filters
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int? Mileage { get; set; }
    public string Color { get; set; } = string.Empty;

}
