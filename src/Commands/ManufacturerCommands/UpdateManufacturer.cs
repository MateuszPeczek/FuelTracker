using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.ManufacturerCommands
{
    public class UpdateManufacturer : ICommand
    {
        public UpdateManufacturer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateManufacturerValidaotr : ICommandValidator<UpdateManufacturer>
    {
        public void Validate(UpdateManufacturer command)
        {
            if (command.Id == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class UpdateManufacturerHandler : ICommandHandler<UpdateManufacturer>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateManufacturer> commandValidator;

        public UpdateManufacturerHandler(IUnitOfWork unitOfWork, ICommandValidator<UpdateManufacturer> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateManufacturer command)
        {
            commandValidator.Validate(command);

            var manufacturerToUpdate = unitOfWork.Context.Manufacturer.Single(e => e.Id == command.Id);

            manufacturerToUpdate.Name = command.Name;

            unitOfWork.Context.Entry(manufacturerToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}