namespace Fidepolis.Models;
public sealed class Purchase
{
    public string Code { get; set; } = string.Empty;
    public int ShowtimeId { get; set; }
    public List<string> Seats { get; set; } = [];
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public decimal PricePerSeat { get; set; }
    public decimal Total { get; set; }
    public DateTime PurchasedAt { get; set; }
    public bool IsCanceled { get; set; }
}
