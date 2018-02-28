using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CoreMvc.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMvc.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Chat()
        {
            ViewBag.ClienteId = new SelectList
            (
               _userManager.Users.ToList(),
               "UserName",
               "UserName"
            );

            return View();
        }
    }
}
