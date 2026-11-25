using Microsoft.AspNetCore.Mvc;
namespace Fidepolis.Web.Controllers;
public sealed class HomeController : Controller
{
    public IActionResult Error() => View();
}
