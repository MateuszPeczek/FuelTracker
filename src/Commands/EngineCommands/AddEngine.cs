using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Linq;

namespace Commands.EngineCommands
{
    public class AddEngine : ICommand
    {
        public Guid Id { get; set; }
        public FuelType FuelType { get; set; }

        public AddEngine(FuelType fuelType)
        {
            Id = Guid.NewGuid();
            FuelType = fuelType;
        }
    }

    public class AddEngineValidator : ICommandValidator<AddEngine>
    {
        public void Validate(AddEngine command)
        {
            if (command.FuelType > Enum.GetValues(typeof(FuelType)).Cast<FuelType>().Last())
                throw new FuelTypeOutOfRangeException();

            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class AddEngineHandler : ICommandHandler<AddEngine>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<AddEngine> commandValidator;

        public AddEngineHandler(IUnitOfWork unitOfWork, ICommandValidator<AddEngine> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddEngine command)
        {
            commandValidator.Validate(command);
            var engineToAdd = new Engine();

            engineToAdd.Id = command.Id;
            

            unitOfWork.Context.Add(engineToAdd);
        }
    }
}