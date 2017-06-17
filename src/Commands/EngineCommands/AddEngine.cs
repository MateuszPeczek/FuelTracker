﻿using Common.Interfaces;
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
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddEngine> commandValidator;

        public AddEngineHandler(ApplicationContext context, ICommandValidator<AddEngine> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddEngine command)
        {
            commandValidator.Validate(command);
            var engineToAdd = new Engine() { Id = command.Id, FuelType = command.FuelType };

            context.Engine.Add(engineToAdd);
        }
    }
}