using Hangfire;
using Hangfire.Mongo;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Swashbuckle.AspNetCore.Swagger;
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

                if (!string.IsNullOrEmpty(Configuration["GithubApi:UserAgent"]))
                    c.DefaultRequestHeaders.Add("User-Agent", Configuration["GithubApi:UserAgent"]);

                if (!string.IsNullOrEmpty(Configuration["GithubApi:Token"]))
                    c.DefaultRequestHeaders.Add("Authorization", $"token {Configuration["GithubApi:Token"]}");
            });

            services.AddScoped<DeveloperDomain>();

            services.AddHttpClient<HttpGithubService>();
            services.AddScoped<IGithubService>(ctx => ctx.GetService<HttpGithubService>());
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDeveloperRepository, MgDeveloperRepository>();

            //adding mediatR dependencies
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //configuring options for MongoDb
            services.Configure<MongoDbOptions>(opts =>
            {
                opts.ConnectionString = Configuration.GetConnectionString("Mongo");
                opts.Database = Configuration["MongoDb:Database"];
            });

            //adding MongoDb mapping
            BsonClassMap.RegisterClassMap<Developer>(cm =>
            {
                cm.MapProperty(x => x.Avatar).SetIgnoreIfNull(true);
                cm.MapProperty(x => x.Bio);
                cm.MapProperty(x => x.Deslikes);
                cm.MapProperty(x => x.GithubUri);
                cm.MapProperty(x => x.Likes);
                cm.MapProperty(x => x.Name);
                cm.MapProperty(x => x.Username);

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

            using (var scoped = services.BuildServiceProvider(false).CreateScope())
            {
                var mongoOpts = scoped.ServiceProvider.GetService<IOptions<MongoDbOptions>>().Value;

                services.AddHealthChecks()
                    .AddGithub(uriOpts =>
                    {
                        uriOpts.Uri = new Uri(Configuration["GithubApi:Uri"]);
                        uriOpts.Token = Configuration["GithubApi:Token"];
                        uriOpts.UserAgent = Configuration["GithubApi:UserAgent"];
                    }, "GithubApi", HealthStatus.Degraded)

                    .AddMongoDb(mongoOpts.ConnectionString, "mongo", HealthStatus.Unhealthy);


                // Add framework services.
                services.AddHangfire(config =>
                {
                    var migrationOptions = new MongoMigrationOptions
                    {
                        Strategy = MongoMigrationStrategy.Drop,
                        BackupStrategy = MongoBackupStrategy.None
                    };
                    var opts = new MongoStorageOptions { Prefix = "hf_", CheckConnection = false, MigrationOptions = migrationOptions };
                    config.UseMongoStorage(mongoOpts.ConnectionString, mongoOpts.Database, opts);
                });
            }

            services.AddHealthChecksUI();

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new Info() { Title = "TinDev App", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TinDev");
            });

            app.UseHealthChecks("/health-checks");
            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(opts => opts.UIPath = "/health-checks-ui");

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
