// Controllers/AIController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.Services;

namespace PMG_s_Game_Repo.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly AIService _aiService;

        public AIController(AIService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet]
        public IActionResult Query()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Query(string userQuery)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
            {
                TempData["Error"] = "Please enter a query";
                return View();
            }

            try
            {
                var response = await _aiService.SendChatQueryAsync(userQuery);
                ViewBag.AIResponse = response;
                ViewBag.UserQuery = userQuery;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error connecting to AI service: {ex.Message}";
            }

            return View();
        }
    }
}