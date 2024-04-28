using AOSAMY.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.AspNetCore.Http;
namespace AOSAMY
{
    public class Startup
    {
        

  

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // ضبط وقت انتهاء الجلسة
                options.Cookie.HttpOnly = true; // تعيين خاصية HttpOnly للكوكيز
                options.Cookie.IsEssential = true; // جعل الكوكيز ضرورية للجلسة
            });


        }
        public void Configure(IApplicationBuilder app)
        {

            app.UseSession();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }
    }
}
