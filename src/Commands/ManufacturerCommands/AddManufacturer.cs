using Common.Interfaces;
using CustomExceptions.Manufacturer;
using Domain.VehicleDomain;
using Persistence;
using System;

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

            var manufacturerToAdd = new Manufacturer() { Id = command.Id, Name = command.Name };

            context.Manufacturer.Add(manufacturerToAdd);
        }
    }
}
