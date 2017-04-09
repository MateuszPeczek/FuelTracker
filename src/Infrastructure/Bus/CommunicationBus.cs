using Common.Interfaces;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Enums;
using Infrastructure.CommunicationModels;

namespace Infrastructure.Bus
{
    public class CommunicationBus : ICommandSender, IEventPublisher, IQuerySender
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;
        private readonly IQueryHandlerFactory queryHandlerFactory;

        public CommunicationBus(ICommandHandlerFactory commandHandlerFactory,
                                IQueryHandlerFactory queryHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            this.queryHandlerFactory = queryHandlerFactory;
        }

        public void Publish(IEvent applicationEvent)
        {
            throw new NotImplementedException();
        }

        public IQueryResult<T> Get<T>(IQuery query)
        {
            try
            {
                var handler = queryHandlerFactory.GetHandler(query);
                var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");
                T result;
                
                if (sendingMethod != null)
                {
                   result = (T)sendingMethod.Invoke(handler, new object[] { query });
                   return new QueryResult<T>() { Data = result };
                }

                throw new Exception($"Handle method not found on {handler.ToString()} object");
            }
            catch (Exception ex)
            {
                return new QueryResult<T>() { Data = default(T), ExceptionMessage = ex.Message };
            }
        }

        public ICommandResult Send(ICommand command)
        {
            try
            {
                var handler = commandHandlerFactory.GetHandler(command);
                var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");

                if (sendingMethod != null)
                    sendingMethod.Invoke(handler, new object[] { command });

                return new CommandResult() { Status = CommandStatus.Success, ExceptionMessage = string.Empty};
            }
            catch (Exception ex)
            {
                //TODO logging
                return new CommandResult() { Status = CommandStatus.Failure, ExceptionMessage = ex.Message };
            }
        }
    }
}
