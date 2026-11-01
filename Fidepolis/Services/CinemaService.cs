using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Fidepolis.Data;
using Fidepolis.Models;
namespace Fidepolis.Services;
public sealed class CinemaService(ICinemaRepository repository) : ICinemaService
{
    private readonly object _purchaseLock = new();
    private static readonly Regex SeatPattern = new("^[A-E][1-8]$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    public IReadOnlyList<Movie> SearchMovies(string? text, string? genre) => repository.GetMovies()
        .Where(m => (string.IsNullOrWhiteSpace(text) || m.Title.Contains(text.Trim(), StringComparison.OrdinalIgnoreCase))
                 && (string.IsNullOrWhiteSpace(genre) || string.Equals(m.Genre, genre, StringComparison.OrdinalIgnoreCase)))
        .ToList();
    public IReadOnlyList<string> Genres() => repository.GetMovies().Select(m => m.Genre).Distinct().OrderBy(x => x).ToList();
    public Movie? FindMovie(int id) => repository.GetMovies().FirstOrDefault(m => m.Id == id);
    public Showtime? FindShowtime(int id) => repository.GetShowtimes().FirstOrDefault(s => s.Id == id);
    public IReadOnlyList<Showtime> MovieShowtimes(int movieId) => repository.GetShowtimes().Where(s => s.MovieId == movieId && IsOpen(s)).OrderBy(s => s.StartsAt).ToList();
    public bool IsOpen(Showtime s) => s.StartsAt > DateTime.Now;
    public IReadOnlySet<string> Occupied(int showtimeId) => repository.GetPurchases().Where(p => p.ShowtimeId == showtimeId && !p.IsCanceled).SelectMany(p => p.Seats).ToHashSet(StringComparer.Ordinal);
    public IReadOnlyList<string> ParseSeats(string? csv) => (csv ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToUpperInvariant()).ToList();
    public string? ValidateSelection(int showtimeId, IReadOnlyList<string> seats)
    {
        var showtime = FindShowtime(showtimeId);
        if (showtime is null) return "La función no existe.";
        if (!IsOpen(showtime)) return "La función ya comenzó.";
        if (seats.Count is < 1 or > 6) return "Seleccioná entre 1 y 6 asientos.";
        if (seats.Any(s => !SeatPattern.IsMatch(s)) || seats.Distinct(StringComparer.Ordinal).Count() != seats.Count)
            return "La selección de asientos no es válida.";
        if (seats.Any(s => Occupied(showtimeId).Contains(s))) return "Uno o más asientos ya están ocupados. Volvé a seleccionar.";
        return null;
    }
    public (Purchase?, string?) Buy(int showtimeId, IReadOnlyList<string> seats, string name, string email)
    {
        lock (_purchaseLock)
        {
            var error = ValidateSelection(showtimeId, seats);
            if (error is not null) return (null, error);
            var showtime = FindShowtime(showtimeId)!;
            var purchases = repository.GetPurchases().ToList();
            string code;
            do { code = "FID-" + Convert.ToHexString(RandomNumberGenerator.GetBytes(5)); }
            while (purchases.Any(p => p.Code == code));
            var purchase = new Purchase
            {
                Code = code, ShowtimeId = showtimeId, Seats = seats.OrderBy(s => s).ToList(),
                BuyerName = name.Trim(), BuyerEmail = email.Trim(), PricePerSeat = showtime.Price,
                Total = showtime.Price * seats.Count, PurchasedAt = DateTime.Now
            };
            purchases.Add(purchase);
            repository.SavePurchases(purchases);
            return (purchase, null);
        }
    }
    public Purchase? FindPurchase(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        lock (_purchaseLock) return repository.GetPurchases().FirstOrDefault(p => p.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));
    }
    public bool Cancel(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;
        lock (_purchaseLock)
        {
            var purchases = repository.GetPurchases().ToList();
            var purchase = purchases.FirstOrDefault(p => p.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));
            if (purchase is null || purchase.IsCanceled) return false;
            var showtime = FindShowtime(purchase.ShowtimeId);
            if (showtime is null || !IsOpen(showtime)) return false;
            purchase.IsCanceled = true;
            repository.SavePurchases(purchases);
            return true;
        }
    }
}
