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
            if (command.EngineId.HasValue && command.EngineId.Value == Guid.Empty)
                throw new InvalidEngineIdException();
        }
    }

    public class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<UpdateVehicle> commandValidator;

        public UpdateVehicleHandler(ApplicationContext context, ICommandValidator<UpdateVehicle> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateVehicle command)
        {
            commandValidator.Validate(command);

            var vehicleToUpdate = context.Vehicle.Single(v => v.Id == command.Id);
            if (vehicleToUpdate == null)
                throw new VehicleNotFoundException(command.Id);

            if (command.EngineId.HasValue)
            {
                var selectedEngine = context.Engine.FirstOrDefault(e => e.Id == command.EngineId);
                if (selectedEngine == null)
                    throw new EngineNotFoundException(command.EngineId.Value);

                vehicleToUpdate.EngineId = command.EngineId;
            }

            vehicleToUpdate.ProductionYear = command.ProductionYear;
        }
    }
}