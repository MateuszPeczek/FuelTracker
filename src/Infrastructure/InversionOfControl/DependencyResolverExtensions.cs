using Infrastructure.Enum;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.InversionOfControl
{
    public static class DependencyResolverExtensions
    {
        public static bool RegisterHandlers(this IServiceCollection services, string sourceAssembly, HandlerType handlerType)
        {
            try
            {
                var handledInterfaceName = string.Empty;
                var handlerInterfaceName = string.Empty;
                var handledImplementationName = string.Empty;
                var handlerImplementationName = string.Empty;

                //TODO: move to resources/config file to enable easy convention change
                switch(handlerType)
                {
                    case HandlerType.Command:
                        handledInterfaceName = "ICommand";
                        handlerInterfaceName = "ICommandHandler";
                        handledImplementationName = "command";
                        handlerImplementationName = "commandhandler";
                        break;
                    case HandlerType.Query:
                        handledInterfaceName = "IQuery";
                        handlerInterfaceName = "IQueryHandler";
                        handledImplementationName = "query";
                        handlerImplementationName = "queryhandler";
                        break;
                }

                var assembly = Assembly.Load(GetAssemblyByName(sourceAssembly));
                var types = assembly.GetTypes();
                var commandTypes = types.Where(c => c.GetInterfaces().Where(i => i.Name == handledInterfaceName).Any()).ToList();
                var handlerTypes = types.Where(c => c.GetInterfaces().Where(i => i.Name.Contains(handlerInterfaceName)).Any()).ToList();

                var names = new HashSet<string>();

                if (commandTypes.Count != handlerTypes.Count)
                    return false;

                foreach (var type in commandTypes)
                {
                    names.Add(type.Name.ToLower().Replace(handledImplementationName, string.Empty));
                }

                foreach (var type in handlerTypes)
                {
                    names.Add(type.Name.ToLower().Replace(handlerImplementationName, string.Empty));
                }

                foreach (var typeName in names)
                {
                    services.AddScoped(
                        commandTypes.Single(c => c.Name.ToLower() == $"{typeName}{handledImplementationName}"),
                        handlerTypes.Single(c => c.Name.ToLower() == $"{typeName}{handlerImplementationName}")
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

        private static AssemblyName GetAssemblyByName(string assemblyName)
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
