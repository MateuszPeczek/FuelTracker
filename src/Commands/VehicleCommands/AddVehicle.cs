using Common.Interfaces;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using Persistence;
using System;

namespace Commands.VehicleCommands
{
    public class AddVehicle : ICommand
    {
        public AddVehicle(Guid modelId)
        {
            Id = Guid.NewGuid();
            ModelId = modelId;
        }

        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
    }

    public class AddVehicleValidator : ICommandValidator<AddVehicle>
    {
        public void Validate(AddVehicle command)
        {
            if (command.ModelId == Guid.Empty)
                throw new InvalidModelIdException();
        }
    }

    public class AddVehicleHandler : ICommandHandler<AddVehicle>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddVehicle> commandValidator;

        public AddVehicleHandler(ApplicationContext context, ICommandValidator<AddVehicle> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddVehicle command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var newVehicle = new Vehicle()
                    {
                        Id = command.Id,
                        ModelNameId = command.ModelId,
                    };

                    context.Vehicle.Add(newVehicle);

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
