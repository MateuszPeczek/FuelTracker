using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.InversionOfControl
{
    public static class DependencyResolverExtensions
    {
        public static bool RegisterHandlersAndValidators(this IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            try
            {
                var assemblies = new HashSet<Assembly>();
                var types = new HashSet<Type>();

                foreach (var name in assemblyNames)
                {
                    var assemblyName = GetAssemblyByName(name);

                    if (assemblyName != null)
                        assemblies.Add(Assembly.Load(assemblyName));
                    else
                        throw new ArgumentNullException(name.ToString());
                }

                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        types.Add(type);
                    }
                }

                var handlersTypes = types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))) //all command handlers
                    .Concat(types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))).ToList()); //all query handlers

                var validators = types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandValidator<>)).Any()).ToList();

                foreach (var type in handlersTypes)
                {

                    var handledCommand = type.GetTypeInfo().ImplementedInterfaces.First().GenericTypeArguments.First();
                    services.AddScoped(handledCommand, type);
                }

                foreach (var type in validators)
                {
                    var implementedInterface = type.GetTypeInfo().ImplementedInterfaces.First();
                    services.AddScoped(implementedInterface, type);
                }

                return true;

            }
            catch (Exception)
            {
                //TODO: logging
                throw;
            }
        }

        private static AssemblyName GetAssemblyByName(string assemblyName)
        {
            var referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            foreach (var assembly in referencedAssemblies)
            {
                if (assemblyName.ToLower().Trim().Equals(assembly.Name.ToLower().Trim()))
                    return assembly;
            }

            return null;
        }

        
    }
}
