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

        public ICommandHandler GetHandler(ICommand command)
        {
            return (ICommandHandler)services.GetRequiredService(command.GetType());
        }
    }
}
