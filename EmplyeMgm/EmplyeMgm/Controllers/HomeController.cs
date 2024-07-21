using EmplyeMgm.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmplyeMgm.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error( )
        {
            var context=HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = context?.Error.Message ?? "An error occured.",
                StatusCode = HttpContext.Response.StatusCode
            };
             return View(errorViewModel);
        }
    }
}
