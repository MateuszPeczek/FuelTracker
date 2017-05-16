using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class UpdateEngine : ICommand
    {
        public UpdateEngine(Guid id, string name, int? power, int? torque, int? cylinders, float? displacement)
        {
            ModelId = id;
            Name = name;
            Power = power;
            Torque = torque;
            Cylinders = cylinders;
            Displacement = Displacement;
        }

        public Guid ModelId { get; set; }
        public string Name { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public int? Cylinders { get; set; }
        public float? Displacement { get; set; }
    }

    public class UpdateEngineValidaotr : ICommandValidator<UpdateEngine>
    {
        public void Validate(UpdateEngine command)
        {
            if (command.ModelId == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class UpdateEngineHandler : ICommandHandler<UpdateEngine>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<UpdateEngine> commandValidator;

        public UpdateEngineHandler(ApplicationContext context, ICommandValidator<UpdateEngine> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateEngine command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToUpdate = context.Engine.Single(e => e.Id == command.ModelId);

                    if (engineToUpdate == null)
                        throw new EngineNotFoundException(command.ModelId);

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
