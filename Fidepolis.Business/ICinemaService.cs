using Fidepolis.Entities;
namespace Fidepolis.Business;
public interface ICinemaService
{
    IReadOnlyList<Movie> SearchMovies(string? text, string? genre);
    IReadOnlyList<string> Genres();
    Movie? FindMovie(int id);
    Showtime? FindShowtime(int id);
    IReadOnlyList<Showtime> MovieShowtimes(int movieId);
    bool IsOpen(Showtime showtime);
    IReadOnlySet<string> Occupied(int showtimeId);
    IReadOnlyList<string> ParseSeats(string? csv);
    string? ValidateSelection(int showtimeId, IReadOnlyList<string> seats);
    (Purchase? Purchase, string? Error) Buy(int showtimeId, IReadOnlyList<string> seats, string name, string email);
    Purchase? FindPurchase(string? code);
    bool Cancel(string? code);
}
