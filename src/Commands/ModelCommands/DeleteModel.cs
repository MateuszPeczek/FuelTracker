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

    public class DeleteEngineValidator : ICommandValidator<DeleteModel>
    {
        public void Validate(DeleteModel command)
        {
            if (command.Id == new Guid())
                throw new CustomExceptions.Model.InvalidModelIdException();

            if (command.ManufacturerId == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteModel> commandValidator;

        public DeleteEngineHandler(ApplicationContext context, ICommandValidator<DeleteModel> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteModel command)
        {
            commandValidator.Validate(command);
            var modelToDelete = context.ModelName.Where(m => m.ManufacturerId == command.ManufacturerId).Single(m => m.Id == command.Id);

            if (modelToDelete == null)
                throw new ModelNotFoundException(command.ManufacturerId, command.Id);

            context.Entry(modelToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
    }
}
