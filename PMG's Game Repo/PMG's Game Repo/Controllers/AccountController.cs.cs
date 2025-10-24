using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.Models;
using PMG_s_Game_Repo.ViewModels;
using System.Security.Claims;

namespace PMG_s_Game_Repo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt");

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                // refresh claim for the profile picture
                var existingClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type == "ProfilePictureUrl");
                if (existingClaim != null)
                    await _userManager.RemoveClaimAsync(user, existingClaim);

                await _userManager.AddClaimAsync(user, new Claim("ProfilePictureUrl", user.ProfilePictureUrl ?? ""));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(User updatedUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.ProfileDescription = updatedUser.ProfileDescription;
                user.ProfilePictureUrl = string.IsNullOrEmpty(updatedUser.ProfilePictureUrl)
                    ? user.ProfilePictureUrl
                    : updatedUser.ProfilePictureUrl;

                await _userManager.UpdateAsync(user);

                var existingClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type == "ProfilePictureUrl");
                if (existingClaim != null)
                    await _userManager.RemoveClaimAsync(user, existingClaim);

                await _userManager.AddClaimAsync(user, new Claim("ProfilePictureUrl", user.ProfilePictureUrl ?? ""));
            }

            return RedirectToAction("Dashboard");
        }


    }
}
