using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Linq;

namespace Commands.EngineCommands
{
    public class DeleteEngine : ICommand
    {
        public Guid Id { get; set; }

        public DeleteEngine(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteEngineValidator : ICommandValidator<DeleteEngine>
    {
        public void Validate(DeleteEngine command)
        {
            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteEngine>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<DeleteEngine> commandValidator;

        public DeleteEngineHandler(IUnitOfWork unitOfWork, ICommandValidator<DeleteEngine> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteEngine command)
        {
            commandValidator.Validate(command);

            var engineToDelete = unitOfWork.Context.Engine.SingleOrDefault(e => e.Id == command.Id);

            if (engineToDelete == null)
                throw new EngineNotFoundException(command.Id);

            unitOfWork.Context.Entry(engineToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
    }
}
