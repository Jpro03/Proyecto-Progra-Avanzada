using Fidepolis.Services;
using Fidepolis.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace Fidepolis.Controllers;
public sealed class MoviesController(ICinemaService cinema) : Controller
{
    [HttpGet]
    public IActionResult Index(string? search, string? genre) => View(new CatalogVm
    {
        Search = search ?? "", Genre = genre ?? "", Genres = cinema.Genres(), Movies = cinema.SearchMovies(search, genre)
    });
    [HttpGet]
    public IActionResult Details(int id)
    {
        var movie = cinema.FindMovie(id);
        if (movie is null) return NotFound();
        var showtimes = cinema.MovieShowtimes(id).Select(s => new ShowtimeInfo(s, 40 - cinema.Occupied(s.Id).Count, cinema.IsOpen(s))).ToList();
        return View(new MovieDetailVm { Movie = movie, Showtimes = showtimes });
    }
}
