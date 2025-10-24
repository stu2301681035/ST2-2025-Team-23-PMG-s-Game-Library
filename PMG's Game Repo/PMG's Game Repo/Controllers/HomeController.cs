using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Services;

namespace PMG_s_Game_Repo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RawgService _rawgService;

        public HomeController(ApplicationDbContext context, RawgService rawgService)
        {
            _context = context;
            _rawgService = rawgService;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = await _context.Users.CountAsync();
            var gameCount = await _rawgService.GetTotalGameCountAsync();
            var libraryCount = 0; 

            ViewBag.UserCount = userCount;
            ViewBag.GameCount = gameCount;
            ViewBag.LibraryCount = libraryCount;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
