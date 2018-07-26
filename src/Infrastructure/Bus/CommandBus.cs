using Common.Enums;
using Common.Interfaces;
using CustomExceptions.CommandBus;
using Infrastructure.CommunicationModels;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Bus
{
    public class CommandBus : ICommandSender
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;
        private readonly IExceptionTypeResolver exceptionTypeResolver;
        private readonly IUnitOfWork unitOfWork;

        private readonly ICollection<Guid> commandIds;
        public IEnumerable<Guid> CommandIds { get { return commandIds; } }

        private readonly ICollection<ICommand> commandsList;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory,
                          IExceptionTypeResolver exceptionTypeResolver,
                          IUnitOfWork unitOfWork)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            this.exceptionTypeResolver = exceptionTypeResolver;
            this.unitOfWork = unitOfWork;

            commandsList = new List<ICommand>();
            commandIds = new List<Guid>();
        }

        public void AddCommand(ICommand command)
        {
            commandsList.Add(command);
            commandIds.Add(command.Id);
        }

        public ICommandResult InvokeCommandsQueue()
        {
            try
            {
                if (!commandsList.Any())
                    throw new EmptyCommandsQueueException("No commands in queue");
                
                foreach (var command in commandsList)
                {
                    var handler = commandHandlerFactory.GetHandler(command);
                    var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");

                    if (sendingMethod != null)
                        sendingMethod.Invoke(handler, new object[] { command });
                }

                unitOfWork.SaveChanges();

                return new CommandResult() { Status = ActionStatus.Success, ExceptionMessage = string.Empty };
            }
            catch (Exception ex)
            {         
                return new CommandResult() { Status = exceptionTypeResolver.ReturnStatusForException(ex), ExceptionMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message };
            }
        }
    }
}
