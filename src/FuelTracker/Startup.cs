using Common.Interfaces;
using Domain.UserDomain;
using Infrastructure.Bus;
using Infrastructure.ExceptionHandling;
using Infrastructure.Factory;
using Infrastructure.InversionOfControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System.Collections.Generic;

namespace FuelTracker
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICommandSender, CommandBus>();
            services.AddScoped<IQuerySender, QueryBus>();
            services.AddScoped<ICommandHandlerFactory, HandlerFactory>();
            services.AddScoped<IQueryHandlerFactory, HandlerFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExceptionTypeResolver, ExceptionTypeResolver>();
            services.AddScoped((s) =>
            {
                return services;
            });

            services.RegisterHandlersAndValidators(new List<string>() { "Commands", "Queries" });

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationStateDatabase")),
                ServiceLifetime.Scoped);
                
            services.AddMvc();

            services.AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                }
            );

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
            });
            

            services.AddIdentity<User, UserRole>(
                options =>
                    options.User.RequireUniqueEmail = true
                );

            services.SeedData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected exception happened");
                    });
                });
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default",
                    template: "api/v{version:apiVersion}/{controller=Default}/{action=Get}/{id?}");
            });


            app.UseSwagger();
            app.UseSwaggerUi();
        } 
    }
}
