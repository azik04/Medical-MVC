using MedicalMVC.Context;
using MedicalMVC.Services.Implementations;
using MedicalMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MedicalMVC.Initializers;

public static class Initializer
{
    public static void Initializ(this IServiceCollection services, IConfiguration configuration)
    {
        //Authentication Settings
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "UserLoginCookie";
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
           options.ExpireTimeSpan = TimeSpan.FromHours(3);
            options.SlidingExpiration = true;
        });


        //Authorization Settings
        services.AddAuthorization();


        //SQL Connection Settings
        services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


        //Serilog settings
        Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Information()
          .WriteTo.Console()
          .WriteTo.File("Logs/Mylogger.txt", rollingInterval: RollingInterval.Day)
          .CreateLogger();

        //Add Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRequestService, RequestService>();

    }
}
