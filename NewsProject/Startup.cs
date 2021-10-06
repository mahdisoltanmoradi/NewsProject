using Autofac;
using Common;
using Data;
using DataLayer.Entities.Users;
using infrastructure.Services;
using infrastructure.Services.Attributes;
using infrastructure.Services.SeedSevices;
using infrastructure.Services.SendEmails;
using infrastructure.WebFramework.Configuration;
using infrastructure.WebFramework.CustomMapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace NewsProject
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>(); 
            services.AddSingleton(emailConfig);
            services.AddControllersWithViews(opt=>opt.Filters.Add<PermissionAuthorizeAttribute>());

            services.AddAutoMapper(Assembly.GetEntryAssembly());

            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.InitializeAutoMapper();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));

            });

            #region Autentication

            services.AddCustomIdentity(_siteSetting.IdentitySettings);

            #region Autentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });
            #endregion

            #endregion

            services.AddRazorPages();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseCustomExceptionHandler();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<ISeedRepository>();
                seedService.SeedAsync().GetAwaiter().GetResult();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "{controller=Default}/{action=Index}/{id?}"
            );
                endpoints.MapRazorPages();
            });

        }
    }
}
