using Common.Interfaces;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleToUpdate = context.Vehicle.Single(v => v.Id == command.Id);
                    var selectedEngine = context.Engine.FirstOrDefault(e => e.Id == command.EngineId);

                    vehicleToUpdate.ProductionYear = command.ProductionYear;

                    if (selectedEngine != null)
                        vehicleToUpdate.Engine = selectedEngine;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
