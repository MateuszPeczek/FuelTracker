using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void Send(ICommand command)
        {
            var handler = commandHandlerFactory.GetHandler(command);
            handler.Handle(command);
        }
    }
}
