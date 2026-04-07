using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Data.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Data;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Web.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Anade.Khadamat.Web
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


            services.AddResponseCaching();
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                                       options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            services.AddScoped<DbContext, CommunicationDbContext>();
            services.AddDbContext<CommunicationDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("RiskFournisseurDbConnection")));



            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddTransient(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient(typeof(ReferentielBusinessService<,>));
            services.AddTransient(typeof(GenericBusinessService<,>));
       
            services.AddTransient(typeof(ActiviteBusinessService));
            services.AddTransient(typeof(ActiviteJourneeInfoBusinessService));

            services.AddTransient(typeof(ActiviteForumBusinessService));
            services.AddTransient(typeof(ActivitePresseBusinessService));
            services.AddTransient(typeof(ActiviteRadioBusinessService));
            services.AddTransient(typeof(ActiviteSalonBusinessService));
            services.AddTransient(typeof(ActiviteReunionExterneBusinessService));
            services.AddTransient(typeof(ActiviteTelevisionBusinessService));
            services.AddTransient(typeof(AgenceWilayaBusinessService));
            services.AddTransient(typeof(MoisClotureBusinessService));


            AppSettings appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.AddSingleton(appSettings);

            services.AddSingleton(new ApiServicesUrls(appSettings));
            services.AddCors(o =>
            {
                o.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                });
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
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
