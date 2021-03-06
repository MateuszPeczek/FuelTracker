﻿using Common.Interfaces;
using CustomExceptions.User;
using Domain.UserDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.UserStore;
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
        private readonly IEmailSendService emailService;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly GuidSignInManager signInManager;

        public AddUserCommandHandler(IUnitOfWork unitOfWork,
                                     ICommandValidator<AddUser> commandValidator,
                                     IEmailSendService emailService,
                                     UserManager<User> userManager,
                                     GuidSignInManager signInManager,
                                     IConfiguration config,
                                     IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
            this.emailService = emailService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
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
            result.Wait();

            if (result.Result.Succeeded)
            {
                var code = userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var serverPath = httpContextAccessor.HttpContext.Request.Host.Value;

                var callbackUrl = $"http://{serverPath}/api/auth/ConfirmEmail?userId={newUser.Id}&code={code.Result}";

                var mailSenderResult = emailService.SendEmail(newUser.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

            }
            else if (result.Status == System.Threading.Tasks.TaskStatus.Faulted)
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
