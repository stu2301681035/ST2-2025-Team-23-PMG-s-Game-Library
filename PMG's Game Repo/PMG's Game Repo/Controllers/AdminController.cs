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
        public async Task<IActionResult> ToggleBan(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            if (currentUser.Id == id)
            {
                TempData["Error"] = "You cannot ban yourself.";
                return RedirectToAction("Index");
            }


            var userToToggleBan = await _userManager.FindByIdAsync(id);
            if (userToToggleBan != null)
            {
                userToToggleBan.IsBanned = !userToToggleBan.IsBanned;
                await _userManager.UpdateAsync(userToToggleBan);

                string action = userToToggleBan.IsBanned ? "banned" : "unbanned";
                TempData["Success"] = $"User {userToToggleBan.UserName} has been {action}.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            if (currentUser.Id == id)
            {
                TempData["Error"] = "You cannot modify your own admin status.";
                return RedirectToAction("Index");
            }

            var userToToggle = await _userManager.FindByIdAsync(id);
            if (userToToggle != null)
            {
                userToToggle.IsAdmin = !userToToggle.IsAdmin;
                await _userManager.UpdateAsync(userToToggle);
                TempData["Success"] = $"Admin status for {userToToggle.UserName} has been updated.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
                return RedirectToAction("AccessDenied", "Account");

            if (currentUser.Id == id)
            {
                TempData["Error"] = "You cannot delete yourself.";
                return RedirectToAction("Index");
            }

            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                await _userManager.DeleteAsync(userToDelete);
                TempData["Success"] = $"User {userToDelete.UserName} has been deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}