using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniAuth.Models;
using Microsoft.AspNetCore.Authentication;

namespace MiniAuth.Controllers
{
    /// <summary>
    /// Controller for the default /Home route.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// GET /Home/Index
        /// 
        /// Displays the home page
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // attach the id_token information if user is signed in
                // for this to work, options.SaveTokens = true must be set
                // in Startup.ConfigureOpenIdConnect method.

                string idToken = await HttpContext.GetTokenAsync("id_token");

                ViewBag.IdToken = idToken;
            }

            return View();
        }

        /// <summary>
        /// Displays a customized error message.
        /// </summary>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
