using HealthChecks.UI.Client;
using Hangfire;
using Hangfire.Mongo;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mime;
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
          
            using (var scoped = services.BuildServiceProvider(false).CreateScope())
            {
                var mongoOpts = scoped.ServiceProvider.GetService<IOptions<MongoDbOptions>>().Value;

                services.AddHealthChecks()

                    .AddUrlGroup(uriOpts =>
                    {
                        uriOpts.AddUri(new Uri(Configuration["GithubApi:Uri"]));
                        uriOpts.UseHttpMethod(System.Net.Http.HttpMethod.Head);
                    }, "GithubApi", HealthStatus.Degraded)

                    .AddMongoDb(mongoOpts.ConnectionString, "mongo", HealthStatus.Unhealthy);
            }

            services.AddHealthChecksUI();
            var mongoOptions = services.BuildServiceProvider().GetService<IOptions<MongoDbOptions>>();
            // Add framework services.
            services.AddHangfire(config =>
            {
                var migrationOptions = new MongoMigrationOptions
                {
                    Strategy = MongoMigrationStrategy.Drop,
                    BackupStrategy = MongoBackupStrategy.None
                };
                var opts = new MongoStorageOptions { Prefix = "hf_", CheckConnection = false, MigrationOptions = migrationOptions };
                config.UseMongoStorage(mongoOptions.Value.ConnectionString, mongoOptions.Value.Database, opts);
            });

            // Add the processing server as IHostedService
            services.AddHangfireServer();
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

            app.UseHealthChecks("/health-checks");

            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
