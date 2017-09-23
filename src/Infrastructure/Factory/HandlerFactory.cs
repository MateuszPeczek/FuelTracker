using Common.Interfaces;
using CustomExceptions.HandlerFactory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Factory
{
    public class HandlerFactory : ICommandHandlerFactory, IQueryHandlerFactory
    {
        private readonly IServiceProvider services;

        public HandlerFactory(IServiceProvider services)
        {
            this.services = services;
        }

        public object GetHandler(IQuery query)
        {
            return ResolveHandlers(query);
        }

        public object GetHandler(ICommand command)
        {
            return ResolveHandlers(command);
        }

        private object ResolveHandlers(object item)
        {
            var itemType = item.GetType();
            var handlerObject = services.GetRequiredService(itemType);

            if (handlerObject == null)
                throw new HandlerNotFoundException("Handler not found");

            return handlerObject;
        }

    }
}
