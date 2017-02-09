using Common.Interfaces;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Enums;

namespace Infrastructure.Bus
{
    public class CommunicationBus : ICommandSender, IEventPublisher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;
        //private readonly IEventHandlerFactory eventHandlerFactory;

        public CommunicationBus(ICommandHandlerFactory commandHandlerFactory/*, IEventHandlerFactory eventHandlerFactory*/)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            //this.eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish(IEvent applicationEvent)
        {
            throw new NotImplementedException();
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
