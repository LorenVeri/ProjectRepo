using CalendarWork.Core;
using CalendarWork.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace CalendarWork.Controllers
{
    [AllowAnonymous]
    public class AuthenController : Controller
    {
        private readonly CalendarWorkDbContext _context;

        public AuthenController(CalendarWorkDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login(string UserName)
        {
            var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Name == UserName);
            if (user != null)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles.Name),
                };

                var claimIdentity = new ClaimsIdentity(claims, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));


                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var email = result.Principal.Identities
                .FirstOrDefault().Claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).SingleOrDefault();
            if (!string.IsNullOrEmpty(email) && email.Contains("@ftech.ai"))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect("/");
        }
    }
}
