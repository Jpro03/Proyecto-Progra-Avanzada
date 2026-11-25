using Fidepolis.Business;
using Fidepolis.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace Fidepolis.Web.Controllers;
public sealed class SeatsController(ICinemaService cinema) : Controller
{
    [HttpGet]
    public IActionResult Select(int id, string? selected, string? message)
    {
        var s = cinema.FindShowtime(id);
        if (s is null) return NotFound();
        var movie = cinema.FindMovie(s.MovieId)!;
        return View(new SeatsVm { Showtime = s, Movie = movie, Occupied = cinema.Occupied(id),
            Selected = cinema.ParseSeats(selected).Where(x => !cinema.Occupied(id).Contains(x)).Take(6).ToList(), Message = message });
    }
    [HttpGet]
    public IActionResult Availability(int id)
    {
        if (cinema.FindShowtime(id) is null) return NotFound();
        return Json(new { occupied = cinema.Occupied(id).ToArray(), open = cinema.IsOpen(cinema.FindShowtime(id)!) });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Continue(int showtimeId, string seatCodes)
    {
        var seats = cinema.ParseSeats(seatCodes);
        var error = cinema.ValidateSelection(showtimeId, seats);
        if (error is not null) return RedirectToAction(nameof(Select), new { id = showtimeId, message = error });
        return RedirectToAction("Checkout", "Purchases", new { showtimeId, seatCodes = string.Join(',', seats) });
    }
}
