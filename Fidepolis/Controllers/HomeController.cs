using Microsoft.AspNetCore.Mvc;
namespace Fidepolis.Controllers;
public sealed class HomeController : Controller
{
    public IActionResult Error() => View();
}
