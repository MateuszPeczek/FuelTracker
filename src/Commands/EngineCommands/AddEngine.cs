using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

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
    }

    public class AddEngineValidator : ICommandValidator<AddEngine>
    {
        public void Validate(AddEngine command)
        {
            throw new NotImplementedException();
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

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {



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
