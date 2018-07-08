using Common.Interfaces;
using CustomExceptions.User;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Commands.UserCommands
{
    public class AddUser : ICommand
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public AddUser(Guid userId, string email, string password)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
        }
    }

    public class AddUserValidator : ICommandValidator<AddUser>
    {
        public void Validate(AddUser command)
        {
            if (command.Id == new Guid())
                throw new InvalidUserIdException();

            if (string.IsNullOrWhiteSpace(command.Email))
                throw new EmptyUserEmailException();

            try
            {
                var email = new MailAddress(command.Email);
            }
            catch (FormatException ex)
            {
                throw new InvalidUserEmailException();
            }

            if (string.IsNullOrWhiteSpace(command.Password))
                throw new EmptyUserPasswordException();
        }
    }

    public class AddUserCommandHandler : ICommandHandler<AddUser>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandValidator<AddUser> commandValidator;

        public void Handle(AddUser command)
        {
            commandValidator.Validate(command);

            //create user logic
        }
    }
}
