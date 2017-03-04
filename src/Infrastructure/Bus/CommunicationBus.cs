using Common.Interfaces;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Enums;

namespace Infrastructure.Bus
{
    public class CommunicationBus : ICommandSender, IEventPublisher, IQuerySender
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;
        private readonly IQueryHandlerFactory queryHandlerFactory;
        //private readonly IEventHandlerFactory eventHandlerFactory;

        public CommunicationBus(ICommandHandlerFactory commandHandlerFactory,
                                IQueryHandlerFactory queryHandlerFactory/*, IEventHandlerFactory eventHandlerFactory*/)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            this.queryHandlerFactory = queryHandlerFactory;
            //this.eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish(IEvent applicationEvent)
        {
            throw new NotImplementedException();
        }

        public TResult Send<TResult>(IQuery query) where TResult : class
        {
            try
            {
                var handler = queryHandlerFactory.GetHandler(query);
                var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");
                TResult result;
                
                if (sendingMethod != null)
                {
                   result = (TResult)sendingMethod.Invoke(handler, new object[] { query });
                   return result;
                }

                throw new Exception($"Handle method not found on {handler.ToString()} object");
            }
            catch (Exception ex)
            {
                //TODO logging
                return null;
            }
        }

        public CommandResult Send(ICommand command)
        {
            try
            {
                var handler = commandHandlerFactory.GetHandler(command);
                var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");

                if (sendingMethod != null)
                    sendingMethod.Invoke(handler, new object[] { command });

                return CommandResult.Success;
            }
            catch (Exception ex)
            {
                //TODO logging
                return CommandResult.Failure;
            }
        }
    }
}
