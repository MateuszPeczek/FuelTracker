using Common.Interfaces;
using CustomExceptions.User;
using Persistence;
using System;
using System.Linq;

namespace Commands.UserCommands
{
    public class UpdateUser : ICommand
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UpdateUser(Guid userId, string firstName, string lastName)
        {
            Id = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class UpdateUserValidator : ICommandValidator<UpdateUser>
    {
        public void Validate(UpdateUser command)
        {
            if (command.Id == new Guid())
                throw new InvalidUserIdException();

            if (string.IsNullOrWhiteSpace(command.FirstName))
                throw new EmptyUserFirstNameException();
        }
    }

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<UpdateUser> commandValidator;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork,
                                        ICommandValidator<UpdateUser> commandValidator)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateUser command)
        {
            commandValidator.Validate(command);

            var dbUSer = unitOfWork.Context.Users.FirstOrDefault(u => u.Id == command.Id);

            if (dbUSer == null)
                throw new UserNotFoundException(command.Id);

            dbUSer.FirstName = command.FirstName;
            dbUSer.LastName = command.LastName;

            unitOfWork.Context.Entry(dbUSer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
