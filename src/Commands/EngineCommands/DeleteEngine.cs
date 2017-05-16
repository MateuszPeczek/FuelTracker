using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class DeleteEngine : ICommand
    {
        public Guid ModelId { get; set; }

        public DeleteEngine(Guid id)
        {
            ModelId = id;
        }
    }

    public class DeleteEngineValidator : ICommandValidator<DeleteEngine>
    {
        public void Validate(DeleteEngine command)
        {
            if (command.ModelId == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteEngine>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteEngine> commandValidator;

        public DeleteEngineHandler(ApplicationContext context, ICommandValidator<DeleteEngine> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteEngine command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToDelete = context.Engine.Single(e => e.Id == command.ModelId);

                    if (engineToDelete == null)
                        throw new EngineNotFoundException(command.ModelId);

                    context.Entry(engineToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
