﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EffectiveTimeUsageTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ObjectiveTimeTracker.Stopwatches;
using ObjectiveTimeTracker.Objectives;

namespace EffectiveTimeUsageTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; set; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStopwatchRepository, StopwatchRepository>();
            services.AddSingleton<IMongoClient>(serveceProvider => new MongoClient(Configuration.GetConnectionString("MongoDB")));
            services.AddTransient<IUserObjectivesRepository, UserObjectivesRepository>();
            services.AddDbContext<UsersIdentityDbContext>(options 
                => options.UseSqlServer(Configuration.GetConnectionString("LocalDB")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<UsersIdentityDbContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Timer", action = "Index" } );
            });
        }
    }
}
