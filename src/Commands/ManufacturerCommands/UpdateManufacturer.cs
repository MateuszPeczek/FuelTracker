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
    public class UpdateManufacturer : ICommand
    {
        public UpdateManufacturer(Guid id, string name)
        {
            ModelId = id;
            Name = name;
        }

        public Guid ModelId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateManufacturerValidaotr : ICommandValidator<UpdateManufacturer>
    {
        public void Validate(UpdateManufacturer command)
        {
            if (command.ModelId == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class UpdateManufacturerHandler : ICommandHandler<UpdateManufacturer>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<UpdateManufacturer> commandValidator;

        public UpdateManufacturerHandler(ApplicationContext context, ICommandValidator<UpdateManufacturer> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateManufacturer command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var manufacturerToUpdate = context.Manufacturer.Single(e => e.Id == command.ModelId);

                    manufacturerToUpdate.Name = command.Name;

                    context.Entry(manufacturerToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
