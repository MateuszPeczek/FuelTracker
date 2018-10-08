using Common.DataTransferObjects;
using Infrastructure.ExceptionHandling;
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
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await authService.AuthorizeUser(new UserCredentials() { Email = model.Email, Password = model.Password });
                    return Ok(token);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.GetMessageIncludingInnerExceptions());
                }
            }

            return BadRequest("Could not create token");
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken", Name = "RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenCredentials refreshTokenCredentials)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await authService.RefreshToken(refreshTokenCredentials);
                    return Ok(token);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.GetMessageIncludingInnerExceptions());
                }
            }

            return BadRequest("Could not create token");
        }

        [Authorize]
        [HttpPost("RevokeToken", Name = "RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody]Guid userId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await authService.RevokeToken(userId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.GetMessageIncludingInnerExceptions());
                }
            }

            return BadRequest("Could not revoke token");
        }

        [AllowAnonymous]
        [HttpPost("RequestConfirmEmail", Name = "RequestConfirmEmail")]
        public async Task<IActionResult> RequestConfirmEmail([FromBody]PostRequestConfirmEmail model)
        {
            try
            {
                var mailSenderResult = await authService.RequestConfirmEmail(model.Email);

                if (mailSenderResult)
                    return Ok();

                return BadRequest("Invalid token or user id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetMessageIncludingInnerExceptions());
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                var result = await authService.ConfirmEmail(new EmailConfirmationCredentials() { Code = code, UserId = new Guid(userId) });

                if (result)
                    return View("EmailConfirmed");

                return BadRequest("Invalid token or user id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetMessageIncludingInnerExceptions());
            }
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]PostForgotPassword model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await authService.RequestForgotPassword(model.Email);

                    if (result)
                        return View("ForgotPassword");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetMessageIncludingInnerExceptions());
            }
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword", Name = "ResetPassword")]
        public IActionResult ResetPassword(string code = null)
        {
            try
            {
                if (code == null)
                {
                    throw new ApplicationException("A code must be supplied for password reset.");
                }
                code = code.Replace(" ", "+");

                var model = new Models.Auth.ResetPassword { Code = code };
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetMessageIncludingInnerExceptions());
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(PostResetPassword model)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.GetMessageIncludingInnerExceptions());
            }
        }
    }
}
