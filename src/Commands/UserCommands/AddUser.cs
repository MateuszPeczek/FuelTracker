using Common.Interfaces;
using CustomExceptions.User;
using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Net.Mail;
using System.Text;

namespace Commands.UserCommands
{
    public class AddUser : ICommand
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public AddUser(string email, string password)
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
        private readonly UserManager<User> userManager;
        
        public AddUserCommandHandler(IUnitOfWork unitOfWork, ICommandValidator<AddUser> commandValidator, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
            this.userManager = userManager;
        }

        public void Handle(AddUser command)
        {
            commandValidator.Validate(command);

            var newUser = new User()
            {
                UserName = "Default",
                Email = command.Email,
                Id = command.Id
            };
            
            var result = userManager.CreateAsync(newUser, command.Password);
            result.Wait();

            if (result.Status == System.Threading.Tasks.TaskStatus.Faulted)
            {
                var sb = new StringBuilder();

                foreach (var error in result.Result.Errors)
                {
                    sb.AppendLine(error.Description);
                }

                throw new Exception(sb.ToString());
            }
        }
    }
}
