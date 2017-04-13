using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.ModelCommands
{
    public class DeleteModel : ICommand
    {
        public Guid Id { get; set; }

        public DeleteModel(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteEngineValidator : ICommandValidator<DeleteModel>
    {
        public void Validate(DeleteModel command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteModel> commandValidator;

        public DeleteEngineHandler(ApplicationContext context, ICommandValidator<DeleteModel> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteModel command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var modelToDelete = context.ModelName.Single(m => m.Id == command.Id);

                    context.Entry(modelToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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
