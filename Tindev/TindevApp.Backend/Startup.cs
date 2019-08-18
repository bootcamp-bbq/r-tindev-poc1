using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
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
            services.AddCors();

            services.AddMvc();

            services.AddOptions();

            services.AddHttpClient<HttpGithubService>(c =>
            {
                c.BaseAddress = new Uri(Configuration["GithubApi:Uri"]);
                c.DefaultRequestHeaders.Add("User-Agent", Configuration["GithubApi:UserAgent"]);
            });

            services.AddHttpClient<HttpGithubService>();

            services.AddScoped<IGithubService>(ctx => ctx.GetService<HttpGithubService>());
            services.AddSingleton<IDevelopersRepository, DevelopersRepository>();
            services.AddSingleton<Db>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
