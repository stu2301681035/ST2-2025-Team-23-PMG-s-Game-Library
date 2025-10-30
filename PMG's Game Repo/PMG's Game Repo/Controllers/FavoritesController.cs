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
                .Include(f => f.Game)
                .Where(f => f.UserId == user.Id)
                .ToListAsync();

            if (User.Identity?.IsAuthenticated ?? false)
            {
                // Rename inner 'user' variable to avoid CS0136
                var currentUser = user;
                var favoriteIds = await _context.Favorites
                    .Where(f => f.UserId == currentUser.Id)
                    .Select(f => f.GameId)
                    .ToListAsync();

                ViewBag.FavoriteIds = favoriteIds;
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            return View(favorites);
        }

        // POST: Add to favorites
        [HttpPost]
        public async Task<IActionResult> Add(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var game = await _context.Games.FirstOrDefaultAsync(g => g.RawgId == gameId);

            if (game == null)
            {
                var rawgGame = await _rawgService.GetGameByIdAsync(gameId);
                if (rawgGame == null)
                {
                    TempData["Error"] = "Game not found in RAWG API.";
                    return RedirectToAction("Index", "Home");
                }

                game = new Game
                {
                    RawgId = rawgGame.Id,
                    Name = rawgGame.Name,
                    Released = rawgGame.Released,
                    Rating = rawgGame.Rating,
                    BackgroundImage = string.IsNullOrEmpty(rawgGame.BackgroundImage)
                        ? "/images/default_game.jpg"
                        : rawgGame.BackgroundImage,
                    Description = string.IsNullOrEmpty(rawgGame.Description)
                        ? "No description available."
                        : rawgGame.Description
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();
            }

            bool alreadyFavorited = await _context.Favorites
                .AnyAsync(f => f.GameId == game.Id && f.UserId == user.Id);

            if (!alreadyFavorited)
            {
                _context.Favorites.Add(new Favorite { GameId = game.Id, UserId = user.Id });
                await _context.SaveChangesAsync();
                TempData["Message"] = $"✅ {game.Name} added to your library!";
            }
            else
            {
                _context.Favorites.Remove(
                    await _context.Favorites.FirstAsync(f => f.GameId == game.Id && f.UserId == user.Id)
                );
                await _context.SaveChangesAsync();
                TempData["Message"] = $"❌ {game.Name} removed from your library.";
            }

            return RedirectToAction("Details", "Games", new { id = game.RawgId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.GameId == gameId && f.UserId == user.Id);

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

            var game = await _context.Games.FirstOrDefaultAsync(g => g.RawgId == gameId);
            if (game == null) return Json(new { isFavorite = false });

            bool isFavorite = await _context.Favorites.AnyAsync(f => f.GameId == game.Id && f.UserId == user.Id);
            return Json(new { isFavorite });
        }
    }
}
