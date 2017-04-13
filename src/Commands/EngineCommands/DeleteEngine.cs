using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class DeleteManufacturer : ICommand
    {
        public Guid Id { get; set; }

        public DeleteManufacturer(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteEngineValidator : ICommandValidator<DeleteManufacturer>
    {
        public void Validate(DeleteManufacturer command)
        {
            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteManufacturer> commandValidator;

        public DeleteEngineHandler(ApplicationContext context, ICommandValidator<DeleteManufacturer> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteManufacturer command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToDelte = context.Engine.Single(e => e.Id == command.Id);

                    context.Entry(engineToDelte).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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
