using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MiniAuth.Controllers
{
    /// <summary>
    /// Controller for the /Account route, provides account management functionality.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// GET /Account/Login
        /// 
        /// Initiates the Auth0 login flow.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync(Const.Auth0, new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        /// <summary>
        /// GET /Account/Logout
        /// 
        /// Intiates the Auth0 logout flow and clears local signin cookie.
        /// </summary>
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(Const.Auth0, new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}