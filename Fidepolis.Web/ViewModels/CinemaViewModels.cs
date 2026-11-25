using System.ComponentModel.DataAnnotations;
using Fidepolis.Entities;
namespace Fidepolis.Web.ViewModels;
public sealed class CatalogVm
{
    public string Search { get; init; } = "";
    public string Genre { get; init; } = "";
    public IReadOnlyList<string> Genres { get; init; } = [];
    public IReadOnlyList<Movie> Movies { get; init; } = [];
}
public sealed class MovieDetailVm
{
    public Movie Movie { get; init; } = null!;
    public IReadOnlyList<ShowtimeInfo> Showtimes { get; init; } = [];
}
public sealed record ShowtimeInfo(Showtime Showtime, int Available, bool IsOpen);
public sealed class SeatsVm
{
    public Movie Movie { get; init; } = null!;
    public Showtime Showtime { get; init; } = null!;
    public IReadOnlySet<string> Occupied { get; init; } = new HashSet<string>();
    public List<string> Selected { get; init; } = [];
    public string? Message { get; init; }
}
public sealed class CheckoutVm
{
    public int ShowtimeId { get; set; }
    public string SeatCodes { get; set; } = "";
    [Required(ErrorMessage = "Ingresá tu nombre."), StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres.")]
    public string BuyerName { get; set; } = "";
    [Required(ErrorMessage = "Ingresá tu correo."), EmailAddress(ErrorMessage = "Ingresá un correo válido."), StringLength(150)]
    public string BuyerEmail { get; set; } = "";
    public Movie? Movie { get; set; }
    public Showtime? Showtime { get; set; }
    public List<string> Seats { get; set; } = [];
    public decimal Total { get; set; }
}
public sealed class ReceiptVm
{
    public Purchase Purchase { get; init; } = null!;
    public Movie Movie { get; init; } = null!;
    public Showtime Showtime { get; init; } = null!;
}
public sealed class SearchPurchaseVm
{
    [Required(ErrorMessage = "Ingresá el código de compra.")]
    public string Code { get; set; } = "";
}
