using Common.DataTransferObjects;
using Common.Interfaces;
using Domain.UserDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.UserStore;
using Services.Email;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Auth
{
    public class AuthorisationService : IAuthorizationService
    {
        private readonly ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly GuidSignInManager signInManager;
        private readonly IConfiguration config;
        private readonly IEmailSendService emailService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorisationService(ApplicationContext context,
                              UserManager<User> userManager,
                              GuidSignInManager signInManager,
                              IConfiguration config,
                              IEmailSendService emailService,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.emailService = emailService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Token> AuthorizeUser(UserCredentials userCredentials)
        {
            var user = await userManager.FindByEmailAsync(userCredentials.Email);

            if (user == null || !user.EmailConfirmed)
                throw new Exception("Error on token generation");

            var result = await signInManager.CheckPasswordSignInAsync(user, userCredentials.Password, false);
            if (result.Succeeded)
            {
                return GenerateUserToken(user);
            }

            throw new Exception("Error on token generation");
        }

        public async Task<Token> RefreshToken(RefreshTokenCredentials refreshTokenCredentials)
        {
            var unauthorizedException = new UnauthorizedAccessException("Unauthorized token refresh action");
            var userRefreshToken = context.RefreshToken.SingleOrDefault(t => t.Token == refreshTokenCredentials.Token && t.UserId == refreshTokenCredentials.UserId);

            if (userRefreshToken == null)
                throw unauthorizedException;

            var tokenManager = new JwtSecurityTokenHandler();
            var token = tokenManager.ReadJwtToken(refreshTokenCredentials.Token);

            var tokenUserGuid = token.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).FirstOrDefault();
            if (tokenUserGuid == null || tokenUserGuid != refreshTokenCredentials.UserId.ToString())
                throw unauthorizedException;

            var user = await userManager.FindByIdAsync(refreshTokenCredentials.UserId.ToString());

            if (user == null)
                throw unauthorizedException;

            return GenerateUserToken(user);
        }

        public async Task<bool> RevokeToken(Guid userId)
        {
            var userRefreshToken = context.RefreshToken.SingleOrDefault(t => t.UserId == userId);

            if (userRefreshToken == null)
                return true;
            else
                await Task.Run(() => context.Remove(userRefreshToken));

            context.SaveChanges();

            return true;
        }

        public async Task<bool> RequestConfirmEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var code = userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = $"http://{GetCurrentServerName()}/api/auth/ConfirmEmail?userId={user.Id}&code={code.Result}";
            var mailSenderResult = await emailService.SendEmail(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

            return mailSenderResult;
        }

        public async Task<bool> ConfirmEmail(EmailConfirmationCredentials emailConfirmationCredentials)
        {
            if (emailConfirmationCredentials.UserId == null || emailConfirmationCredentials.Code == null)
                return false;

            var user = await userManager.FindByIdAsync(emailConfirmationCredentials.UserId.ToString());
            if (user == null)
                return false;

            emailConfirmationCredentials.Code = emailConfirmationCredentials.Code.Replace(" ", "+");

            var result = await userManager.ConfirmEmailAsync(user, emailConfirmationCredentials.Code);

            return result.Succeeded;
        }

        public async Task<bool> RequestForgotPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                return true;
            
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"http://{GetCurrentServerName()}/api/auth/ResetPassword?code={code}";
            var mailSenderResult = await emailService.SendEmail(user.Email, "Reset password request",
                $"Go to reset password form by <a href='{url}'>clicking here</a>.");

            return mailSenderResult;
        }

        public async Task<bool> ResetPassword(ResetPasswordData resetPasswordData)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordData.Email);

            if (user == null)
                return true;
            
            resetPasswordData.Code = resetPasswordData.Code.Replace(" ", "+");
            var result = await userManager.ResetPasswordAsync(user, resetPasswordData.Code, resetPasswordData.Password);

            return result.Succeeded;
        }

        public UserData GetCurrentUserData()
        {
            var data = httpContextAccessor.HttpContext.User.Claims;

            if (!data.Any())
                throw new UnauthorizedAccessException();

            var userData = new UserData()
            {
                Email = data.Where(c => c.Type == "Email").Select(c => c.Value).FirstOrDefault(),
                UserId = new Guid(data.Where(c => c.Type == "UserId").Select(c => c.Value).FirstOrDefault())
            };

            return userData;
        }

        #region "Private methods"

        private Token GenerateUserToken(User user)
        {
            var token = new Token();
            double expiryDays;
            int expiryMonths;

            if (!double.TryParse(config["Jwt:AccessTokenDaysValid"], out expiryDays) ||
                !int.TryParse(config["Jwt:RefreshTokenMonthsValid"], out expiryMonths))
                throw new Exception("Invalid token expriation settings");


            token.AccessToken = GenerateToken(new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserId",user.Id.ToString())
                    },
                expiryDate: DateTime.UtcNow.AddDays(expiryDays));


            token.RefreshToken = GenerateToken(new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserId",user.Id.ToString())
                    },
                expiryDate: DateTime.UtcNow.AddMonths(expiryMonths));
        
            var existingRefreshToken = context.RefreshToken.SingleOrDefault(t => t.UserId == user.Id);

            if (existingRefreshToken == null)
            {
                var newUserRefreshToken = new RefreshToken()
                {
                    Token = token.RefreshToken,
                    UserId = user.Id,
                    User = user
                };

                context.Add(newUserRefreshToken);
            }
            else
            {
                existingRefreshToken.Token = token.RefreshToken;
                context.Entry(existingRefreshToken).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            context.SaveChanges();

            return token;
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime expiryDate)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: expiryDate,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetCurrentServerName()
        {
            return httpContextAccessor.HttpContext.Request.Host.Value;
        }

#endregion
    }
}
