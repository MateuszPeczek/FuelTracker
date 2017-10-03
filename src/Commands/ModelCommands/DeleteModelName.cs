using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class DeleteModelName : ICommand
    {
        public Guid ManufacturerId { get; set; }
        public Guid Id { get; set; }

        public DeleteModelName(Guid manufacturerId, Guid modelId)
        {
            ManufacturerId = manufacturerId;
            Id = modelId;
        }
    }

    public class DeleteModelNameValidator : ICommandValidator<DeleteModelName>
    {
        public void Validate(DeleteModelName command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();

            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteModelNameHandler : ICommandHandler<DeleteModelName>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<DeleteModelName> commandValidator;

        public DeleteModelNameHandler(IUnitOfWork unitOfWork, ICommandValidator<DeleteModelName> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteModelName command)
        {
            commandValidator.Validate(command);
            var modelToDelete = unitOfWork.Context.ModelName.Where(m => m.ManufacturerId == command.ManufacturerId).Single(m => m.Id == command.Id);

            if (modelToDelete == null)
                throw new ModelNotFoundException(command.ManufacturerId, command.Id);

            unitOfWork.Context.Entry(modelToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
    }
}
