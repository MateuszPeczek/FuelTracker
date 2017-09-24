using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class UpdateModel : ICommand
    {
        public UpdateModel(Guid manufacturerId, Guid modelId, string name)
        {
            ManufacturerId = manufacturerId;
            Id = modelId;
            Name = name;
        }

        public Guid ManufacturerId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateModelValidator : ICommandValidator<UpdateModel>
    {
        public void Validate(UpdateModel command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();

            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new EmptyModelNameException();
        }
    }

    public class UpdateModelHandler : ICommandHandler<UpdateModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateModel> commandValidator;

        public UpdateModelHandler(IUnitOfWork unitOfWork, ICommandValidator<UpdateModel> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateModel command)
        {
            commandValidator.Validate(command);

            if (!unitOfWork.Context.Manufacturer.Any(m => m.Id == command.Id))
                throw new ManufacturerNotFoundException(command.ManufacturerId);

            var modelToUpdate = unitOfWork.Context.ModelName.Single(m => m.Id == command.Id);

            modelToUpdate.Name = command.Name;
            modelToUpdate.ManufacturerId = command.ManufacturerId;

            unitOfWork.Context.Entry(modelToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
