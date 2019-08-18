using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TindevApp.Backend.Infrastructure;
using TindevApp.Backend.Services;
using TindevApp.Backend.Data;
using TindevApp.Backend.Data.Repository.Abstract;
using TindevApp.Backend.Data.Repository.Impl;

namespace TindevApp.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            Environment = hostingEnvironment;
        }

        public IHostingEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddOptions();

            services.Configure<GithubServiceOptions>(cfg =>
            {
                cfg.ApiUri = Configuration["GithubApi"];
            });

            services.Configure<DbConnectionOptions>(cfg =>
            {
                cfg.ConnectionString = Configuration["ConnectionString"];
            });

            services.AddHttpClient<HttpGithubService>();

            services.AddSingleton<IGithubService, HttpGithubService>();
            services.AddSingleton<IDevelopersRepository, DevelopersRepository>();
            services.AddSingleton<Db, Db>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
