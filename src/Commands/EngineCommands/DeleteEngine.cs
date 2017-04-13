﻿using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.EngineCommands
{
    public class DeleteEngine : ICommand
    {
        public Guid Id { get; set; }

        public DeleteEngine(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteEngineValidator : ICommandValidator<DeleteEngine>
    {
        public void Validate(DeleteEngine command)
        {
            if (command.Id == new Guid())
                throw new InvalidEngineIdException();
        }
    }

    public class DeleteEngineHandler : ICommandHandler<DeleteEngine>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteEngine> commandValidator;

        public DeleteEngineHandler(ApplicationContext context, ICommandValidator<DeleteEngine> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteEngine command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var engineToDelte = context.Engine.Single(e => e.Id == command.Id);

                    context.Entry(engineToDelte).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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
