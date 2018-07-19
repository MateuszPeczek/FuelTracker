using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class AddModelName : ICommand
    {
        public Guid Id { get; set; }
        public Guid ManufacturerId { get; set; }
        public string Name { get; set; }

        public AddModelName(Guid manufacturerId, string name)
        {
            Id = Guid.NewGuid();
            ManufacturerId = manufacturerId;
            Name = name;
        }
    }

    public class AddModelNameValidator : ICommandValidator<AddModelName>
    {
        public void Validate(AddModelName command)
        {
            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();

            if (command.Id == new Guid())
                throw new InvalidModelIdException();

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new EmptyModelNameException();
        }
    }

    public class AddModelNameHandler : ICommandHandler<AddModelName>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<AddModelName> commandValidator;

        public AddModelNameHandler(IUnitOfWork unitOfWork, ICommandValidator<AddModelName> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddModelName command)
        {
            commandValidator.Validate(command);

            if (!unitOfWork.Context.Manufacturer.Any(m => m.Id == command.ManufacturerId))
                throw new ManufacturerNotFoundException(command.ManufacturerId);

            var modelToAdd = new ModelName() { Id = command.Id, ManufacturerId = command.ManufacturerId, Name = command.Name };

            unitOfWork.Context.ModelName.Add(modelToAdd);
            unitOfWork.SaveChanges();
        }
    }
}
