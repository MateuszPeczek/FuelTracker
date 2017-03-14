using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using Common.Interfaces;
using Infrastructure.Bus;
using Infrastructure.Factory;
using Persistence;
using Domain.UserDomain;
using Infrastructure.InversionOfControl;
using Infrastructure.Enum;

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
            services.AddSingleton<ICommandSender, CommunicationBus>();
            services.AddSingleton<IQuerySender, CommunicationBus>();
            services.AddSingleton<IEventPublisher, CommunicationBus>();
            services.AddSingleton<ICommandHandlerFactory, HandlerFactory>();
            services.AddSingleton<IQueryHandlerFactory, HandlerFactory>();
            services.AddScoped((s) =>
            {
                return services;
            });

            services.RegisterHandlers(new List<string>() { "Commands", "Queries" });

            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ApplicationStateDatabase")));


            services.AddMvc();
            services.AddSwaggerGen();

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

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
        } 
    }
}
