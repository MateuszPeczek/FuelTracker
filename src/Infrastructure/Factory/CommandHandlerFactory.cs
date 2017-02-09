using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Factory
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceCollection diContainer;
        private readonly IServiceProvider services;

        public CommandHandlerFactory(IServiceCollection diContainer)
        {
            this.diContainer = diContainer;
            services = diContainer.BuildServiceProvider();
        }

        public object GetHandler(ICommand command)
        {
            try
            {
                var commandType = command.GetType();
                var handlerObject = services.GetRequiredService(commandType);

                if (handlerObject == null)
                    throw new Exception("Handler not found");

                return handlerObject;
            }
            catch (Exception ex)
            {
                throw ex;
                //TODO logging
            }
        }

    }
}
