using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.ManufacturerCommands
{
    public class DeleteManufacturer : ICommand
    {
        public Guid ModelId { get; set; }

        public DeleteManufacturer(Guid id)
        {
            ModelId = id;
        }
    }

    public class DeleteManufacturerValidator : ICommandValidator<DeleteManufacturer>
    {
        public void Validate(DeleteManufacturer command)
        {
            if (command.ModelId == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteManufacturer> commandValidator;

        public DeleteManufacturerHandler(ApplicationContext context, ICommandValidator<DeleteManufacturer> commandValidator)
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
                    var manufacturerToDelete = context.Manufacturer.Single(m => m.Id == command.ModelId);

                    context.Entry(manufacturerToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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
