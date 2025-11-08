using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Services;
using PMG_s_Game_Repo.ViewModels;
using System.Security.Claims;

namespace PMG_s_Game_Repo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RawgService _rawgService;
        private readonly ILogger<HomeController> _logger;
        private readonly AiQueryInterpreter _ai;
        private readonly GameRepository _repo;


        public HomeController(ApplicationDbContext context, RawgService rawgService, ILogger<HomeController> logger, AiQueryInterpreter ai, GameRepository repo)
        {
            _context = context;
            _rawgService = rawgService;
            _logger = logger;
            _ai = ai;
            _repo = repo;
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
        [HttpGet]
        public IActionResult IntelligentSearch()
        {
            return View(new IntelligentSearchVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IntelligentSearch(IntelligentSearchVm model)
        {
            if (string.IsNullOrWhiteSpace(model.NaturalQuery))
            {
                model.Error = "Please enter a search phrase.";
                return View(model);
            }

            var (plan, rawJson, error) = await _ai.GetPlanFromNaturalQueryAsync(model.NaturalQuery!);
            model.JsonPlan = rawJson;

            if (error != null)
            {
                model.Error = error;
                return View(model);
            }

            if (plan == null)
            {
                model.Error = "No valid plan returned.";
                return View(model);
            }

            var results = _repo.SelectGamesAdvanced(plan);
            model.Results = results;
            return View(model);
        }
    }
}
