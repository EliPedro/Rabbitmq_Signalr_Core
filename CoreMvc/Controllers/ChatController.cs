using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoreMvc.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
