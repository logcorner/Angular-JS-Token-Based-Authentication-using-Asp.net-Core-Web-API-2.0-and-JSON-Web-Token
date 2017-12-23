﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenAuthWebApiCore.Server.Models;
using TokenAuthWebApiCore.Server.Repository;

namespace TokenAuthWebApiCore.Server
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            // Add framework services.
            services.AddMvc();
            SetUpDataBase(services);
            services.AddIdentity<MyUser, MyRole>().AddEntityFrameworkStores<SecurityContext>();

            services.AddAuthentication((cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtSecurityToken:Issuer"],
                    ValidAudience = Configuration["JwtSecurityToken:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityToken:Key"])),
                    ValidateLifetime = true
                };
            });

            services.AddCors(cfg =>
            {
                cfg.AddPolicy("AngularClient", bldr =>
                {
                    bldr.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3276", "http://localhost:51610");
                });
            });
            services.AddTransient<ProductRepository, ProductRepository>();
        }

        public virtual void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<SecurityContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("SecurityConnection"), sqlOptions => sqlOptions.MigrationsAssembly("TokenAuthWebApiCore.Server")));
        }

        public virtual void EnsureDatabaseCreated(SecurityContext dbContext)
        {
            // run Migrations
            dbContext.Database.Migrate();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Error);
            app.UseCors("AngularClient");
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
              .CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<SecurityContext>();
                EnsureDatabaseCreated(dbContext);
            }
        }
    }
}