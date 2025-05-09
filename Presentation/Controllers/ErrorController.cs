using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{code?}")]
        public IActionResult Index(int? code)
        {
            var model = new ErrorViewModel
            {
                StatusCode = code ?? 500,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
