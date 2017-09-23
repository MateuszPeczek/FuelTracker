using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Linq;

namespace Commands.EngineCommands
{
    public class UpdateEngine : ICommand
    {
        public UpdateEngine(Guid id, string name, int? power, int? torque, int? cylinders, float? displacement)
        {
            Id = id;
            Name = name;
            Power = power;
            Torque = torque;
            Cylinders = cylinders;
            Displacement = displacement;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public int? Cylinders { get; set; }
        public float? Displacement { get; set; }
    }

    public class UpdateEngineValidaotr : ICommandValidator<UpdateEngine>
    {
        public void Validate(UpdateEngine command)
        {
            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class UpdateEngineHandler : ICommandHandler<UpdateEngine>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateEngine> commandValidator;

        public UpdateEngineHandler(IUnitOfWork unitOfWork, ICommandValidator<UpdateEngine> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateEngine command)
        {
            commandValidator.Validate(command);

            var engineToUpdate = unitOfWork.Context.Engine.Single(e => e.Id == command.Id);

            if (engineToUpdate == null)
                throw new EngineNotFoundException(command.Id);

            engineToUpdate.Name = command.Name;
            engineToUpdate.Power = command.Power;
            engineToUpdate.Torque = command.Torque;
            engineToUpdate.Cylinders = command.Cylinders;
            engineToUpdate.Displacement = command.Displacement;

            unitOfWork.Context.Entry(engineToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}

