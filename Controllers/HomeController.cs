using Microsoft.AspNetCore.Mvc;
using MyMVCApp.Models;
using System.Diagnostics;

namespace MyMVCApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(model);
        }
    }
}
