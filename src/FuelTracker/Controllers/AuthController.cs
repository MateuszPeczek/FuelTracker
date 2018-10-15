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
        [HttpPost("AuhorizeUser", Name = "AuthorizeUser")]
        public async Task<IActionResult> AuhorizeUser([FromBody]PostUser model)
        {
                var token = await authService.AuthorizeUser(new UserCredentials() { Email = model.Email, Password = model.Password });
                return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken", Name = "RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenCredentials refreshTokenCredentials)
        {
                var token = await authService.RefreshToken(refreshTokenCredentials);
                return Ok(token);
        }

        [Authorize]
        [HttpPost("RevokeToken", Name = "RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody]Guid userId)
        {
                var result = await authService.RevokeToken(userId);
                return Ok();
        }

        [AllowAnonymous]
        [HttpPost("RequestConfirmEmail", Name = "RequestConfirmEmail")]
        public async Task<IActionResult> RequestConfirmEmail([FromBody]PostRequestConfirmEmail model)
        {
            var mailSenderResult = await authService.RequestConfirmEmail(model.Email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await authService.ConfirmEmail(new EmailConfirmationCredentials() { Code = code, UserId = new Guid(userId) });
            return View("EmailConfirmed");
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
