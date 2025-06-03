
namespace Contracts;

public class AuctionFinishedContract
{

    public bool ItemSold { get; set; }
    public required string AuctionId { get; set; }
    public string Winner { get; set; } = string.Empty;
    public required string Seller { get; set; }
    public int Amount { get; set; }
}
