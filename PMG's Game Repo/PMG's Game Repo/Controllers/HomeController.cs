using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Services;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var libraryCount = userId != null
                ? await _context.Favorites.CountAsync(ug => ug.UserId == userId)
                : 0;

            //Featured game section
            var mostFavoritedGame = await _context.Favorites
               .GroupBy(f => f.RawgId)
               .Select(g => new
               {
                   RawgId = g.Key,
                   FavoriteCount = g.Count()
               })
               .OrderByDescending(x => x.FavoriteCount)
               .FirstOrDefaultAsync();

            RawgGameDetailsDto featuredGame = null;
            int favoriteCount = 0;

            if (mostFavoritedGame != null)
            {
                featuredGame = await _rawgService.GetGameByIdAsync(mostFavoritedGame.RawgId);
                favoriteCount = mostFavoritedGame.FavoriteCount;
            }

            ViewBag.UserCount = userCount;
            ViewBag.GameCount = gameCount;
            ViewBag.LibraryCount = libraryCount;
            ViewBag.FeaturedGame = featuredGame;
            ViewBag.FavoriteCount = favoriteCount;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
