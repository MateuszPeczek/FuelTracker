using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Factory
{
    public class HandlerFactory : ICommandHandlerFactory, IQueryHandlerFactory
    {
        private readonly IServiceCollection diContainer;
        private readonly IServiceProvider services;

        public HandlerFactory(IServiceCollection diContainer)
        {
            this.diContainer = diContainer;
            services = diContainer.BuildServiceProvider();
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
            try
            {
                var itemType = item.GetType();
                var handlerObject = services.GetRequiredService(itemType);

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
