using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storm.Formification.WebWithDb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if NETCOREAPP3_1
using Microsoft.Extensions.Hosting;
#endif
using Storm.Formification.Core;
using MediatR;
using static Storm.Formification.Core.Forms;
using Storm.Formification.WebWithDb.Forms;
using Storm.Formification.WebWithDb.Forms.Preferences;

namespace Storm.Formification.WebWithDb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
#if NETCOREAPP3_1
                .AddDefaultUI()
#else
                .AddDefaultUI(UIFramework.Bootstrap4)
#endif
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMediatR(typeof(Startup).Assembly);

            services.AddMvc()
                .ConfigureForms(typeof(Startup).Assembly)
                .EnableFormsController(typeof(Startup).Assembly)
#if NETCOREAPP3_1
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
#else
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#endif

            services.AddHttpContextAccessor();

            services.AddScoped<IFormDataStore<PreferencesForm>, AzureStorageFormDataStore<PreferencesForm>>();
            services.AddScoped<IFormActions<PreferencesForm>, FormContainerActions<PreferencesForm>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#if  NETCOREAPP3_1
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#else
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

#if NETCOREAPP3_1
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
#else
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#endif
        }
    }
}
