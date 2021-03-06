﻿using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Linq;

namespace Commands.VehicleCommands
{
    public class DeleteVehicle : ICommand
    {
        public DeleteVehicle(Guid guid)
        {
            Id = guid;
        }

        public Guid Id { get; set; }
    }

    public class DeleteVehicleValidator : ICommandValidator<DeleteVehicle>
    {
        public void Validate(DeleteVehicle command)
        {
            if (command.Id == Guid.Empty)
                throw new InvalidVehicleIdException();
        }
    }

    public class DeleteVehicleHandler : ICommandHandler<DeleteVehicle>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<DeleteVehicle> commandValidator;

        public DeleteVehicleHandler(IUnitOfWork unitOfWork, ICommandValidator<DeleteVehicle> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteVehicle command)
        {
            commandValidator.Validate(command);
            var vehicleToDelete = unitOfWork.Context.Vehicle.Single(s => s.Id == command.Id);

            var consumptionReportsToDelete = unitOfWork.Context.ConsumptionReport.Where(c => c.VehicleId == vehicleToDelete.Id);
            unitOfWork.Context.ConsumptionReport.RemoveRange(consumptionReportsToDelete);
            
            var fuelSummaryToDelete = unitOfWork.Context.FuelSummary.Where(f => f.VehicleId == vehicleToDelete.Id).SingleOrDefault();
            if (fuelSummaryToDelete != null)
                unitOfWork.Context.FuelSummary.Remove(fuelSummaryToDelete);

            if (vehicleToDelete != null)
                unitOfWork.Context.Vehicle.Remove(vehicleToDelete);
            else
                throw new VehicleNotFoundException(command.Id);
        }
    }
}