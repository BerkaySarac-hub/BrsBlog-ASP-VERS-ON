using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BrsBlogWeb.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using BrsBlogWeb.Controllers;
using Microsoft.AspNetCore.Http;

namespace BrsBlogWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            
            services.AddDbContextPool<BrsBlogWebContext>(options =>
                  options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                  new MySqlServerVersion
                  (ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))),
                  options=>options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                  ));
            services.AddDbContextPool<BrsProjectsContext>(options =>
                  options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                  new MySqlServerVersion
                  (ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))),
                  options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                  ));
            services.AddDbContextPool<BrsAdminsContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                 new MySqlServerVersion
                 (ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))),
                 options => options.EnableRetryOnFailure(
                   maxRetryCount: 5,
                   maxRetryDelay: System.TimeSpan.FromSeconds(30),
                   errorNumbersToAdd: null)
                 ));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60*100000);
                
            });
            services.AddDistributedMemoryCache();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.LoginPath = "/Admins/AdminLogin";
            });
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            
            app.UseSession();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
           
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStatusCodePages();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Anasayfa}/{id?}");
            });
        }
    }
}
