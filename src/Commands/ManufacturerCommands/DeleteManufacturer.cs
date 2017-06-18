using Common.Interfaces;
using CustomExceptions.Manufacturer;
using Persistence;
using System;
using System.Linq;

namespace Commands.ManufacturerCommands
{
    public class DeleteManufacturer : ICommand
    {
        public Guid Id { get; set; }

        public DeleteManufacturer(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteManufacturerValidator : ICommandValidator<DeleteManufacturer>
    {
        public void Validate(DeleteManufacturer command)
        {
            if (command.Id == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<DeleteManufacturer> commandValidator;

        public DeleteManufacturerHandler(IUnitOfWork unitOfWork, ICommandValidator<DeleteManufacturer> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteManufacturer command)
        {
            commandValidator.Validate(command);

            var manufacturerToDelete = unitOfWork.Context.Manufacturer.Single(m => m.Id == command.Id);
            var modelsToDelete = unitOfWork.Context.ModelName.Where(m => m.ManufacturerId == manufacturerToDelete.Id);

            unitOfWork.Context.Entry(manufacturerToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            foreach (var model in modelsToDelete)
            {
                unitOfWork.Context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
        }
    }
}
