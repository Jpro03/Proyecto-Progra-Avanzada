using Fidepolis.Entities;
namespace Fidepolis.Data;
public interface ICinemaRepository
{
    IReadOnlyList<Movie> GetMovies();
    IReadOnlyList<Showtime> GetShowtimes();
    IReadOnlyList<Purchase> GetPurchases();
    void SavePurchases(IReadOnlyList<Purchase> purchases);
}
