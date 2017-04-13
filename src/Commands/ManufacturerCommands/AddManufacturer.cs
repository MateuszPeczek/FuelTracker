using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Manufacturer;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.ManudaturerCommands
{
    public class AddManufacturer : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public AddManufacturer(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }

    public class AddManufacturerValidator : ICommandValidator<AddManufacturer>
    {
        public void Validate(AddManufacturer command)
        {
            if (command.Id == new Guid())
                throw new InvalidManufacturerIdException();

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new EmptyManufacturerNameException();
        }
    }

    public class AddManufacturerHandler : ICommandHandler<AddManufacturer>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddManufacturer> commandValidator;

        public AddManufacturerHandler(ApplicationContext context, ICommandValidator<AddManufacturer> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddManufacturer command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var manufacturerToAdd = new Manufacturer() { Id = command.Id, Name = command.Name };

                    context.Manufacturer.Add(manufacturerToAdd);
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
