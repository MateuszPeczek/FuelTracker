using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Persistence;
using System;
using System.Linq;

namespace Commands.ModelCommands
{
    public class DeleteModel : ICommand
    {
        public Guid ManufacturerId { get; set; }
        public Guid Id { get; set; }

        public DeleteModel(Guid manufacturerId, Guid modelId)
        {
            ManufacturerId = manufacturerId;
            Id = modelId;
        }
    }

    public class DeleteModelValidator : ICommandValidator<DeleteModel>
    {
        public void Validate(DeleteModel command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();

            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteModelHandler : ICommandHandler<DeleteModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<DeleteModel> commandValidator;

        public DeleteModelHandler(IUnitOfWork unitOfWork, ICommandValidator<DeleteModel> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteModel command)
        {
            commandValidator.Validate(command);
            var modelToDelete = unitOfWork.Context.ModelName.Where(m => m.ManufacturerId == command.ManufacturerId).Single(m => m.Id == command.Id);

            if (modelToDelete == null)
                throw new ModelNotFoundException(command.ManufacturerId, command.Id);

            unitOfWork.Context.Entry(modelToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
    }
}
