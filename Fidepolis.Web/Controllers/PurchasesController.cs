using Fidepolis.Business;
using Fidepolis.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace Fidepolis.Web.Controllers;
public sealed class PurchasesController(ICinemaService cinema) : Controller
{
    [HttpGet]
    public IActionResult Checkout(int showtimeId, string seatCodes)
    {
        var vm = new CheckoutVm { ShowtimeId = showtimeId, SeatCodes = seatCodes };
        return RenderCheckout(vm);
    }
    private IActionResult RenderCheckout(CheckoutVm vm)
    {
        var s = cinema.FindShowtime(vm.ShowtimeId);
        if (s is null) return NotFound();
        var seats = cinema.ParseSeats(vm.SeatCodes);
        var error = cinema.ValidateSelection(vm.ShowtimeId, seats);
        if (error is not null)
            return RedirectToAction("Select", "Seats", new { id = vm.ShowtimeId, message = error });
        vm.Showtime = s; vm.Movie = cinema.FindMovie(s.MovieId); vm.Seats = seats.ToList(); vm.Total = seats.Count * s.Price;
        return View("Checkout", vm);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Confirm(CheckoutVm vm)
    {
        if (!ModelState.IsValid) return RenderCheckout(vm);
        var seats = cinema.ParseSeats(vm.SeatCodes);
        var (purchase, error) = cinema.Buy(vm.ShowtimeId, seats, vm.BuyerName, vm.BuyerEmail);
        if (error is not null)
        {
            TempData["PurchaseError"] = error;
            return RedirectToAction("Select", "Seats", new { id = vm.ShowtimeId, message = error });
        }
        return RedirectToAction(nameof(Receipt), new { code = purchase!.Code });
    }
    [HttpGet]
    public IActionResult Receipt(string code)
    {
        var p = cinema.FindPurchase(code);
        if (p is null) return NotFound();
        var s = cinema.FindShowtime(p.ShowtimeId)!;
        return View(ReceiptVm(p, s));
    }
    [HttpGet]
    public IActionResult Lookup() => View(new SearchPurchaseVm());
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Lookup(SearchPurchaseVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var purchase = cinema.FindPurchase(vm.Code);
        if (purchase is null)
        {
            ModelState.AddModelError(nameof(vm.Code), "No existe ninguna compra con ese código.");
            return View(vm);
        }
        return RedirectToAction(nameof(Details), new { code = purchase.Code });
    }
    [HttpGet]
    public IActionResult Details(string code)
    {
        var p = cinema.FindPurchase(code);
        if (p is null) return NotFound();
        var s = cinema.FindShowtime(p.ShowtimeId)!;
        return View(ReceiptVm(p, s));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Cancel(string code)
    {
        TempData["CancelResult"] = cinema.Cancel(code) ? "Compra cancelada. Los asientos volvieron a estar disponibles." : "No se pudo cancelar. Puede que la función haya comenzado o que la compra ya esté cancelada.";
        return RedirectToAction(nameof(Details), new { code });
    }
    private ReceiptVm ReceiptVm(Fidepolis.Entities.Purchase purchase, Fidepolis.Entities.Showtime s) =>
        new() { Purchase = purchase, Showtime = s, Movie = cinema.FindMovie(s.MovieId)! };
}
