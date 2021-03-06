﻿using Common.Interfaces;
using CustomExceptions.CommandBus;
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

        public void InvokeCommandsQueue()
        {
            if (!commandsList.Any())
                throw new EmptyCommandsQueueException("No commands in queue");

            try
            {
                foreach (var command in commandsList)
                {
                    var handler = commandHandlerFactory.GetHandler(command);
                    var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");

                    if (sendingMethod != null)
                        sendingMethod.Invoke(handler, new object[] { command });
                    else
                        throw new Exception($"Handle method not found on {handler.ToString()} object");
                }

                unitOfWork.SaveChanges();
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
