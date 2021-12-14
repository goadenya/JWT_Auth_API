
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyPortfolio_AuthAPI.Data.Entities;

[assembly: HostingStartup(typeof(MyPortfolio_AuthAPI.Data.IdentityHostingStartup))]
namespace MyPortfolio_AuthAPI.Data
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("LocalDB")));


                IdentityBuilder identityBuilder = services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddDefaultTokenProviders();
            });
        }
    }
}
