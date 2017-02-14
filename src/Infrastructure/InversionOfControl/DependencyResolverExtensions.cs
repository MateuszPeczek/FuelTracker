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
        public static bool RegisterHandlersToCommands(this IServiceCollection services, string sourceAssembly)
        {
            try
            {

                var assembly = Assembly.Load(GetAssemblyByName(sourceAssembly));
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
