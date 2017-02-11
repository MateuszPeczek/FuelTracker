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
            services.AddSingleton<IEventPublisher, CommunicationBus>();
            services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();
            services.AddScoped((s) =>
            {
                return services;
            });

            //Register all commands to commands handler
            var commandsAssemblyName = GetAssemblyByName("Application");
            RegisterHandlersToCommands(services, commandsAssemblyName);


            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ApplicationStateDatabase")));

            services.AddMvc();

            services.AddIdentity<User, UserRole>(
                options =>
                    options.User.RequireUniqueEmail = true
                );
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
        }

        private bool RegisterHandlersToCommands(IServiceCollection services, AssemblyName sourceAssembly)
        {
            try
            {

                var assembly = Assembly.Load(sourceAssembly);
                var types = assembly.GetTypes();
                var commandTypes = types.Where(c => c.GetInterfaces().Where(i => i.Name == "ICommand").Any()).ToList();
                var handlerTypes = types.Where(c => c.GetInterfaces().Where(i => i.Name.Contains("ICommandHandler")).Any()).ToList();

                var names = new HashSet<string>();

                if (commandTypes.Count != handlerTypes.Count)
                    return false;

                foreach (var type in types)
                {
                    names.Add(type.Name.ToLower().Replace("commandhandler", string.Empty).Replace("command", string.Empty));
                }

                foreach (var typeName in names)
                {
                    services.AddScoped(
                        commandTypes.Single(c => c.Name.ToLower() == $"{typeName}command"), 
                        handlerTypes.Single(c => c.Name.ToLower() == $"{typeName}commandhandler")
                        );
                }

                return true;

            }
            catch (Exception ex)
            {
                //TODO: logging
                return false;
            }
        }

        private AssemblyName GetAssemblyByName(string assemblyName)
        {
            var referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            foreach (var assembly in referencedAssemblies)
            {
                if (assemblyName.ToLower().Trim().Equals(assembly.Name.ToLower().Trim()))
                    return assembly;

                continue;
            }

            return null;
        }
    }
}
