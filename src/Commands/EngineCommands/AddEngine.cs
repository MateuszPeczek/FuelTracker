using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class AddEngineModel : ICommand
    {
        public Guid Id { get; set; }
        public FuelType FuelType { get; set; }

        public AddEngineModel(FuelType fuelType)
        {
            Id = Guid.NewGuid();
            this.FuelType = fuelType;
        }
    }

    public class AddEngineValidator : ICommandValidator<AddEngineModel>
    {
        public void Validate(AddEngineModel command)
        {
            if (command.FuelType > Enum.GetValues(typeof(FuelType)).Cast<FuelType>().Last())
                throw new FuelTypeOutOfRangeException();

            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class AddEngineHandler : ICommandHandler<AddEngineModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddEngineModel> commandValidator;

        public AddEngineHandler(ApplicationContext context, ICommandValidator<AddEngineModel> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddEngineModel command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToAdd = new Engine() { Id = command.Id, FuelType = command.FuelType };

                    context.Engine.Add(engineToAdd);
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
