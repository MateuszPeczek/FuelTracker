using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Linq;

namespace Commands.VehicleCommands
{
    public class UpdateVehicle : ICommand
    {
        public Guid Id { get; set; }
        public int? ProductionYear { get; set; }
        public Guid? EngineId { get; set; }

        public UpdateVehicle(Guid guid, int? productionYear, Guid? engineId)
        {
            Id = guid;
            ProductionYear = productionYear;
            EngineId = engineId;
        }
    }

    public class UpdateVehicleValidator : ICommandValidator<UpdateVehicle>
    {
        public void Validate(UpdateVehicle command)
        {
            if (command.Id == new Guid())
                throw new InvalidVehicleIdException();

            if (command.EngineId.HasValue && command.EngineId.Value == Guid.Empty)
                throw new InvalidEngineIdException();
        }
    }

    public class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateVehicle> commandValidator;

        public UpdateVehicleHandler(IUnitOfWork unitOfWork, ICommandValidator<UpdateVehicle> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateVehicle command)
        {
            commandValidator.Validate(command);

            var vehicleToUpdate = unitOfWork.Context.Vehicle.Single(v => v.Id == command.Id);
            if (vehicleToUpdate == null)
                throw new VehicleNotFoundException(command.Id);

            if (command.EngineId.HasValue)
            {
                var selectedEngine = unitOfWork.Context.Engine.FirstOrDefault(e => e.Id == command.EngineId);
                if (selectedEngine == null)
                    throw new EngineNotFoundException(command.EngineId.Value);

                vehicleToUpdate.ProductionYear = command.ProductionYear;
                vehicleToUpdate.EngineId = command.EngineId;
            }

            vehicleToUpdate.ProductionYear = command.ProductionYear;
        }
    }
}