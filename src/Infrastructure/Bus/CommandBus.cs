using Common.Enums;
using Common.Interfaces;
using Infrastructure.CommunicationModels;
using Persistence;
using System;
using System.Collections;
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

        private ICollection<ICommand> commandsList;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory,
                          IExceptionTypeResolver exceptionTypeResolver,
                          IUnitOfWork unitOfWork)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            this.exceptionTypeResolver = exceptionTypeResolver;
            this.unitOfWork = unitOfWork;

            commandsList = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            commandsList.Add(command);
        }

        public ICommandResult InvokeCommandsQueue()
        {
            try
            {
                if (!commandsList.Any())
                    throw new Exception("No commands in queue");

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
                //TODO logging
                return new CommandResult() { Status = exceptionTypeResolver.ReturnCommandStatusForException(ex), ExceptionMessage = ex.Message };
            }
        }
    }
}
