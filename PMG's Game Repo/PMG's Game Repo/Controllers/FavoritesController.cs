using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Models;
using PMG_s_Game_Repo.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PMG_s_Game_Repo.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RawgService _rawgService;

    public FavoritesController(ApplicationDbContext context, UserManager<User> userManager, RawgService rawgService)
        {
            _context = context;
            _userManager = userManager;
            _rawgService = rawgService;
        }

        // GET: Favorites list
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _context.Favorites
                .Where(f => f.UserId == user.Id)
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();

            var gamesWithDetails = new List<RawgGameDetailsDto>();
            foreach (var fav in favorites)
            {
                var game = await _rawgService.GetGameByIdAsync(fav.RawgId);
                if (game != null)
                {
                    gamesWithDetails.Add(game);
                }
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                ViewBag.FavoriteIds = favorites.Select(f => f.RawgId).ToList();
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            return View(gamesWithDetails);
        }

        // POST: Add to favorites
        [HttpPost]
        public async Task<IActionResult> Add(int gameId, string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var rawgGame = await _rawgService.GetGameByIdAsync(gameId);

            if (rawgGame == null)
            {

                TempData["Error"] = "Game not found in RAWG API.";
                return RedirectToAction("Index", "Home");
            }

           

            bool alreadyFavorited = await _context.Favorites
                .AnyAsync(f => f.RawgId == gameId && f.UserId == user.Id);

            if (!alreadyFavorited)
            {
                _context.Favorites.Add(new Favorite { RawgId = gameId, UserId = user.Id });
                await _context.SaveChangesAsync();
                TempData["Message"] = $"✅ {rawgGame.Name} added to your library!";
            }
            else
            {
                _context.Favorites.Remove(
                    await _context.Favorites.FirstAsync(f => f.RawgId == gameId && f.UserId == user.Id)
                );
                await _context.SaveChangesAsync();
                TempData["Message"] = $"❌ {rawgGame.Name} removed from your library.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.RawgId == gameId && f.UserId == user.Id);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Game removed from your library.";
            }

            return RedirectToAction("Index");
        }

        // GET: Check if a game is already favorited (for button toggling)
        [HttpGet]
        public async Task<JsonResult> IsFavorite(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { isFavorite = false });

            bool isFavorite = await _context.Favorites.AnyAsync(f => f.RawgId == gameId && f.UserId == user.Id);
            return Json(new { isFavorite });
        }
    }
}
