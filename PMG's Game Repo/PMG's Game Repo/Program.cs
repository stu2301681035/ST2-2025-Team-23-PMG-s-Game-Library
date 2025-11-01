using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Filters;
using PMG_s_Game_Repo.Models;
using PMG_s_Game_Repo.Services;

namespace PMG_s_Game_Repo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<PMG_s_Game_Repo.Services.RawgService>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<RawgService>();


            // Fix: Replace AddDefaultIdentity with AddIdentity  
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<BannedUserCheckFilter>();
            });

            builder.Services.AddScoped<BannedUserCheckFilter>();

            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Authentication & Authorization  
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.Run();
        }
    }
}
