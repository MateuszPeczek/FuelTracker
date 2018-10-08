using Common.DataTransferObjects;
using System;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<Token> AuthorizeUser(UserCredentials userCredentials);
        Task<Token> RefreshToken(RefreshTokenCredentials refreshTokenCredentials);
        Task<bool> RevokeToken(Guid userId);
        Task<bool> RequestConfirmEmail(string email);
        Task<bool> ConfirmEmail(EmailConfirmationCredentials emailConfirmationCredentials);
        Task<bool> RequestForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordData resetPasswordData);
        UserData GetCurrentUserData();
    }
}
