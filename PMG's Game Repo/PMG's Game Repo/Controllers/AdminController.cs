using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.Models;

namespace PMG_s_Game_Repo.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("AccessDenied", "Account", new { returnUrl = "/Admin/Index" });
            }

            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            var userToBan = await _userManager.FindByIdAsync(id);
            if (userToBan != null)
            {
                userToBan.IsBanned = true; 
                await _userManager.UpdateAsync(userToBan);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            var userToToggle = await _userManager.FindByIdAsync(id);
            if (userToToggle != null)
            {
                userToToggle.IsAdmin = !userToToggle.IsAdmin;
                await _userManager.UpdateAsync(userToToggle);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                await _userManager.DeleteAsync(userToDelete);
            }
            return RedirectToAction("Index");
        }
    }
}