using Common.DataTransferObjects;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<string> GenerateToken(UserCredentials userCredentials);
        Task<bool> RequestConfirmEmail(string email);
        Task<bool> ConfirmEmail(EmailConfirmationCredentials emailConfirmationCredentials);
        Task<bool> RequestForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordData resetPasswordData);
        UserData GetCurrentUserData();
    }
}
