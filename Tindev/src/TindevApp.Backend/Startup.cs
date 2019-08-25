using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Reflection;
using System.Text;
using TindevApp.Backend.Domains;
using TindevApp.Backend.Infrastructure.Mvc;
using TindevApp.Backend.Models;
using TindevApp.Backend.Repositories.Mongo;
using TindevApp.Backend.Services;
using TindevApp.Backend.Services.Authentication;
using TindevApp.Backend.Services.Http;

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

            services.AddTransient<RequestPopulatorActionFilter>();
            services.AddMvc(config => config.Filters.AddService<RequestPopulatorActionFilter>());

            services.AddOptions();

            services.AddHttpClient<HttpGithubService>(c =>
            {
                c.BaseAddress = new Uri(Configuration["GithubApi:Uri"]);
                c.DefaultRequestHeaders.Add("User-Agent", Configuration["GithubApi:UserAgent"]);
            });

            services.AddScoped<DeveloperDomain>();

            services.AddHttpClient<HttpGithubService>();
            services.AddScoped<IGithubService>(ctx => ctx.GetService<HttpGithubService>());
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDeveloperRepository, MgDeveloperRepository>();

            services.Configure<MongoDbOptions>(opts =>
            {
                opts.ConnectionString = Configuration.GetConnectionString("Mongo");
                opts.Database = Configuration["MongoDb:Database"];
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            BsonClassMap.RegisterClassMap<Developer>(cm =>
            {
                cm.MapProperty(x => x.Avatar).SetIgnoreIfNull(true);
                cm.MapProperty(x => x.Bio);
                cm.MapProperty(x => x.Deslikes);
                cm.MapProperty(x => x.GithubUri);
                cm.MapProperty(x => x.Likes);
                cm.MapProperty(x => x.Name);
                cm.MapProperty(x => x.Username);

                //cm.MapIdProperty(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);

                cm.MapIdProperty(x => x.Id).SetSerializer(IdBsonSerializer.Instance).SetIdGenerator(ObjectIdGenerator.Instance);
            });


            services.Configure<JwtUserOptions>(opts =>
            {
                opts.Secret = Configuration["AppSettings:Secret"];
            });

            var byteSecret = Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(byteSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
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

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
