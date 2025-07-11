namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    public Guid Id { get; set; }
    public string? Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int? Mileage { get; set; }
    public string Color { get; set; } = string.Empty;
}
