using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TheLamboProject.Data;

namespace TheLamboProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Program.Configuration = Configuration;
            Program.HangfireService = new Data.Services.HangfireService();
            Program.CoinspotService = new Data.Services.CoinspotService();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            //Remove Later
            services.AddSingleton<WeatherForecastService>();

            // Database Initialize
            services.AddDbContextFactory<Data.DataBases.DataWharehouse.DataWharehouseCXT>(config => {
                config.UseSqlite($@"Data Source={AppDomain.CurrentDomain.BaseDirectory}\Data\DataBases\DataWharehouse\DataWharehouse.db");
            });

            //Hangfire Connection
            services.AddHangfire(config =>
            {
                config.UseSQLiteStorage($@"{AppDomain.CurrentDomain.BaseDirectory}\Data\DataBases\Hangfire\HangfireDatabase.db");

            });
            //services.AddHangfireServer(options =>
            //{
            //    options.Queues = new[] { 
            //        Configuration.GetSection("HangfireSettings")["PriceCacheQueue"], 
            //        "default" 
            //    };
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions() { 
                Queues = new[] {
                    Configuration.GetSection("HangfireSettings")["PriceCacheQueue"],
                    "default"
                }
            });

            app.UseHangfireDashboard();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            Program.HangfireService.StartCachingPrices();
            //Program.CoinspotService.CacheCurrentPrices();
        }
    }
}
