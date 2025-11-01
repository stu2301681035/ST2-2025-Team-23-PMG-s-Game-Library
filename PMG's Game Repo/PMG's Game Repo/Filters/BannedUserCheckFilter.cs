using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PMG_s_Game_Repo.Filters
{
    public class BannedUserCheckFilter : IAsyncActionFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public BannedUserCheckFilter(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(context.HttpContext.User);

                if (user != null && user.IsBanned)
                {
                    await _signInManager.SignOutAsync();
                    context.Result = new RedirectToActionResult("Banned", "Account", null);
                    return;
                }
            }

            await next();
        }
    }
}
