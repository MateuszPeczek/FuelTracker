using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.ModelCommands
{
    public class UpdateModel : ICommand
    {
        public UpdateModel(Guid manufacturerId, Guid modelId, string name)
        {
            ManufacturerId = manufacturerId;
            Id = modelId;
            Name = name;
        }

        public Guid ManufacturerId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateModelValidaotr : ICommandValidator<UpdateModel>
    {
        public void Validate(UpdateModel command)
        {
            if (command.Id == new Guid())
                throw new InvalidModelIdException();
        }
    }

    public class UpdateModelHandler : ICommandHandler<UpdateModel>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<UpdateModel> commandValidator;

        public UpdateModelHandler(ApplicationContext context, ICommandValidator<UpdateModel> commandValidator)
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
                    if (!context.Manufacturer.Any(m => m.Id == command.Id))
                        throw new ManufacturerNotFoundException(command.ManufacturerId);

                    var modelToUpdate = context.ModelName.Single(m => m.Id == command.Id);

                    modelToUpdate.Name = command.Name;
                    modelToUpdate.ManufacturerId = command.ManufacturerId;

                    context.Entry(modelToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
