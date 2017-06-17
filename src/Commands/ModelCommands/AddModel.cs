using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class AddModel : ICommand
    {
        public Guid Id { get; set; }
        public Guid ManufacturerId { get; set; }
        public string Name { get; set; }

        public AddModel(Guid manufacturerId, string name)
        {
            Id = Guid.NewGuid();
            ManufacturerId = manufacturerId;
            Name = name;
        }
    }

    public class AddModelValidator : ICommandValidator<AddModel>
    {
        public void Validate(AddModel command)
        {
            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();

            if (command.Id == new Guid())
                throw new InvalidModelIdException();
        }
    }

    public class AddModelHandler : ICommandHandler<AddModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddModel> commandValidator;

        public AddModelHandler(ApplicationContext context, ICommandValidator<AddModel> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddModel command)
        {
            commandValidator.Validate(command);

            if (!context.Manufacturer.Any(m => m.Id == command.ManufacturerId))
                throw new ManufacturerNotFoundException(command.ManufacturerId);

            var modelToAdd = new ModelName() { Id = command.Id, ManufacturerId = command.ManufacturerId, Name = command.Name };

            context.ModelName.Add(modelToAdd);
        }
    }
}
