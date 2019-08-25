using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Reflection;
using TindevApp.Backend.Models;
using TindevApp.Backend.Repositories;
using TindevApp.Backend.Repositories.Mongo;
using TindevApp.Backend.Services;

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

            services.AddTransient<IDeveloperRepository, MgDeveloperRepository>();

            services.Configure<MongoDbOptions>(opts =>
            {
                opts.ConnectionString = Configuration.GetConnectionString("Mongo");
                opts.Database = Configuration["MongoDb:Database"];
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            BsonClassMap.RegisterClassMap<Developer>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
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
