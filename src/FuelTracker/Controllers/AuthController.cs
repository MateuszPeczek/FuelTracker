using Common.Interfaces;
using Domain.UserDomain;
using FuelTracker.ApiModels.UserApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.UserStore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FuelTracker.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        public AuthController(UserManager<User> userManager,
                              GuidSignInManager signInManager,
                              IConfiguration config,
                              IEmailSendService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.emailService = emailService;
        }

        private readonly UserManager<User> userManager;
        private readonly GuidSignInManager signInManager;
        private readonly IConfiguration config;
        private readonly IEmailSendService emailService;

        [AllowAnonymous]
        [HttpPost(Name = "GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] PostUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null || !user.EmailConfirmed)
                    return StatusCode(403);

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {

                    var claims = new[]
                    {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("UserId",user.Id.ToString())
                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(config["Jwt:Issuer"],
                      config["Jwt:Issuer"],
                      claims,
                      expires: DateTime.Now.AddMinutes(30),
                      signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return BadRequest("Could not create token");
        }


        [AllowAnonymous]
        [HttpPost("RequestConfirmEmail", Name = "RequestConfirmEmail")]
        public async Task<IActionResult> RequestConfirmEmail([FromBody]PostRequestConfirmEmail model)
        {
        
            var user = await userManager.FindByEmailAsync(model.Email);
            //if (user != null && user.EmailConfirmed)
            //    return Ok();

            var code = userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = $"http://{Request.Host.Value}/api/auth/ConfirmEmail?userId={user.Id}&code={code.Result}";
            var mailSenderResult = emailService.SendEmail(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

            if (mailSenderResult)
                return Ok();

            return BadRequest("Invalid token or user id");
        }


        [AllowAnonymous]
        [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return Ok();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Ok();

            code = code.Replace(" ", "+");

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View("EmailConfirmed");

            return BadRequest("Invalid token or user id");
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]PostForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return Ok();
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var url = $"http://{Request.Host.Value}/api/auth/ResetPassword?code={code}";
                var mailSenderResult = emailService.SendEmail(user.Email, "Reset password request",
                    $"Go to reset password form by <a href='{url}'>clicking here</a>.");
                return Ok();
            }
            
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword", Name = "ResetPassword")]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new PostResetPassword { Code = code };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(PostResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok();
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("PasswordUpdated");
            }
            return Ok();
        }
    }
}
