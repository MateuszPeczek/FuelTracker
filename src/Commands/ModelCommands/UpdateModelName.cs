using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class UpdateModelName : ICommand
    {
        public UpdateModelName(Guid modelId, Guid manufacturerId, string name)
        {
            Id = modelId;
            ManufacturerId = manufacturerId;
            Name = name;
        }

        public Guid ManufacturerId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateModelNameValidator : ICommandValidator<UpdateModelName>
    {
        public void Validate(UpdateModelName command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();

            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new EmptyModelNameException();
        }
    }

    public class UpdateModelNameHandler : ICommandHandler<UpdateModelName>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateModelName> commandValidator;

        public UpdateModelNameHandler(IUnitOfWork unitOfWork, ICommandValidator<UpdateModelName> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateModelName command)
        {
            commandValidator.Validate(command);

            if (!unitOfWork.Context.Manufacturer.Any(m => m.Id == command.ManufacturerId))
                throw new ManufacturerNotFoundException(command.ManufacturerId);

            var modelToUpdate = unitOfWork.Context.ModelName.Single(m => m.Id == command.Id);

            modelToUpdate.Name = command.Name;
            modelToUpdate.ManufacturerId = command.ManufacturerId;

            unitOfWork.Context.Entry(modelToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
