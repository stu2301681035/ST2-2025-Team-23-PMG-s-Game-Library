using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.Data;
using Microsoft.EntityFrameworkCore;

namespace PMG_s_Game_Repo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = await _context.Users.CountAsync();
            var gameCount = await _context.Games.CountAsync();

            ViewBag.UserCount = userCount;
            ViewBag.GameCount = gameCount;

            ViewBag.IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
            ViewBag.UserEmail = User.Identity?.Name;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
