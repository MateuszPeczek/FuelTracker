using Common.DataTransferObjects;
using FuelTracker.ApiModels.UserApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FuelTracker.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly Common.Interfaces.IAuthorizationService authService;

        public AuthController(Common.Interfaces.IAuthorizationService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost(Name = "GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody]PostUser model)
        {
            if (ModelState.IsValid)
            {
                var token = await authService.GenerateToken(new UserCredentials() { Email = model.Email, Password = model.Password });

                return Ok(new { token });
            }

            return BadRequest("Could not create token");
        }

        [AllowAnonymous]
        [HttpPost("RequestConfirmEmail", Name = "RequestConfirmEmail")]
        public async Task<IActionResult> RequestConfirmEmail([FromBody]PostRequestConfirmEmail model)
        {

            var mailSenderResult = await authService.RequestConfirmEmail(model.Email);

            if (mailSenderResult)
                return Ok();

            return BadRequest("Invalid token or user id");
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await authService.ConfirmEmail(new EmailConfirmationCredentials() { Code = code, UserId = new Guid(userId) });

            if (result)
                return View("EmailConfirmed");

            return BadRequest("Invalid token or user id");
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]PostForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.RequestForgotPassword(model.Email);

                if (result)
                    return View("ForgotPassword");
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
            code = code.Replace(" ", "+");

            var model = new Models.Auth.ResetPassword { Code = code };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(PostResetPassword model)
        {
            if (model.Password != model.ConfirmPassword)
                ModelState.AddModelError("Password", "Password and confirm password fields are not the same");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.ResetPassword(new ResetPasswordData()
            {
                Code = model.Code,
                ConfirmPassword = model.ConfirmPassword,
                Email = model.Email,
                Password = model.Password
            });

            if (result)
                return View("PasswordUpdated");

            return Ok();
        }
    }
}
