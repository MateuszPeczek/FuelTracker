using Common.Interfaces;
using CustomExceptions.User;
using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.UserStore;
using System;
using System.Net.Mail;
using System.Text;
<<<<<<< HEAD
=======
using System.Text.Encodings.Web;
>>>>>>> 762846a... Identity service

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
            catch (FormatException)
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
<<<<<<< HEAD
=======
        private readonly IEmailSendService emailSender;
>>>>>>> 762846a... Identity service
        private readonly UserManager<User> userManager;
        private readonly GuidSignInManager signInManager;
        private readonly IConfiguration config;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, 
<<<<<<< HEAD
                                     ICommandValidator<AddUser> commandValidator, 
=======
                                     ICommandValidator<AddUser> commandValidator,
                                     IEmailSendService emailSender,
>>>>>>> 762846a... Identity service
                                     UserManager<User> userManager,
                                     GuidSignInManager signInManager,
                                     IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
        }

        public void Handle(AddUser command)
        {
            commandValidator.Validate(command);

            var newUser = new User()
            {
                UserName = command.Email,
                Email = command.Email,
                Id = command.Id
            };

            var newSettings = new UserSettings()
            {
                Units = Domain.Common.Units.Metric,
                User = newUser,
                UserId = command.Id
            };

            var result = userManager.CreateAsync(newUser, command.Password);
<<<<<<< HEAD
            result.Wait();

            if (result.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
=======

            if (result.Result.Succeeded)
            {
                var code = userManager.GenerateEmailConfirmationTokenAsync(newUser);
                
                var callbackUrl = $"http://{command.Url}/api/auth/ConfirmEmail?userId={newUser.Id}&code={code.Result}";

                var mailSenderResult = emailSender.SendEmail(newUser.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
                
>>>>>>> 762846a... Identity service
                unitOfWork.Context.UserSettings.Add(newSettings);
            
<<<<<<< HEAD
            if (result.Status == System.Threading.Tasks.TaskStatus.Faulted)
=======
            if (!result.Result.Succeeded)
>>>>>>> 762846a... Identity service
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
