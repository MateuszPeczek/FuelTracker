using Common.Interfaces;
using Infrastructure.Enum;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.DotNet.InternalAbstractions;
using System.Threading.Tasks;

namespace Infrastructure.InversionOfControl
{
    public static class DependencyResolverExtensions
    {
        public static bool RegisterHandlers(this IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            try
            {
                var assemblies = new HashSet<Assembly>();
                var types = new HashSet<Type>();

                foreach (var name in assemblyNames)
                {
                    var assemblyName = GetAssemblyByName(name);
                    assemblies.Add(Assembly.Load(assemblyName));
                }

                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        types.Add(type);
                    }
                }

                var handlersTypes = types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)).Any()).ToList() //all command handlers
                    .Concat(types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)).Any()).ToList()); //all query handlers

                foreach (var type in handlersTypes)
                {

                    var handledCommand = type.GetTypeInfo().ImplementedInterfaces.First().GenericTypeArguments.First();
                    var handler = type;
                    services.AddScoped(handledCommand, type);
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
