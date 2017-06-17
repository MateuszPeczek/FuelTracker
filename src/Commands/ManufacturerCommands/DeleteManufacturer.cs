﻿using Common.Interfaces;
using CustomExceptions.Manufacturer;
using Persistence;
using System;
using System.Linq;

namespace Commands.ManufacturerCommands
{
    public class DeleteManufacturer : ICommand
    {
        public Guid Id { get; set; }

        public DeleteManufacturer(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteManufacturerValidator : ICommandValidator<DeleteManufacturer>
    {
        public void Validate(DeleteManufacturer command)
        {
            if (command.Id == new Guid())
                throw new InvalidManufacturerIdException();
        }
    }

    public class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteManufacturer> commandValidator;

        public DeleteManufacturerHandler(ApplicationContext context, ICommandValidator<DeleteManufacturer> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteManufacturer command)
        {
            commandValidator.Validate(command);

            var manufacturerToDelete = context.Manufacturer.Single(m => m.Id == command.Id);
            var modelsToDelete = context.ModelName.Where(m => m.ManufacturerId == manufacturerToDelete.Id);

            context.Entry(manufacturerToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            foreach (var model in modelsToDelete)
            {
                context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
        }
    }
}
