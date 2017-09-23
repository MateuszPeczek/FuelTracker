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
        public string Name { get; set; }
        public int Power { get; set; }
        public int Torque { get; set; }
        public int Cylinders { get; set; }
        public float Displacement { get; set; }
        public FuelType FuelType { get; set; }

        public AddEngine(string name, 
                         int power, 
                         int torque, 
                         int cylinders, 
                         float displacement, 
                         FuelType fuelType)
        {
            Id = Guid.NewGuid();
            Name = name;
            Power = power;
            Torque = torque;
            Cylinders = cylinders;
            Displacement = displacement;
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
            engineToAdd.Name = command.Name;
            engineToAdd.Power = command.Power;
            engineToAdd.Torque = command.Torque;
            engineToAdd.Cylinders = command.Cylinders;
            engineToAdd.Displacement = command.Displacement;
            engineToAdd.FuelType = command.FuelType;            

            unitOfWork.Context.Add(engineToAdd);
        }
    }
}