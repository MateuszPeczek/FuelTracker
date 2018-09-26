using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IEmailSendService
    {
        Task<bool> SendEmail(string email, string subject, string message);
    }
}
