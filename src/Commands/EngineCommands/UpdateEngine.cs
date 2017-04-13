using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class UpdateModel : ICommand
    {
        public UpdateModel(Guid id, string name, int? power, int? torque, int? cylinders, float? displacement)
        {
            Id = id;
            Name = name;
            Power = power;
            Torque = torque;
            Cylinders = cylinders;
            Displacement = Displacement;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public int? Cylinders { get; set; }
        public float? Displacement { get; set; }
    }

    public class UpdateEngineValidaotr : ICommandValidator<UpdateModel>
    {
        public void Validate(UpdateModel command)
        {
            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class UpdateEngineHandler : ICommandHandler<UpdateModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<UpdateModel> commandValidator;

        public UpdateEngineHandler(ApplicationContext context, ICommandValidator<UpdateModel> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateModel command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToUpdate = context.Engine.Single(e => e.Id == command.Id);

                    engineToUpdate.Name = command.Name;
                    engineToUpdate.Power = command.Power;
                    engineToUpdate.Torque = command.Torque;
                    engineToUpdate.Cylinders = command.Cylinders;
                    engineToUpdate.Displacement = command.Displacement;

                    context.Entry(engineToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
