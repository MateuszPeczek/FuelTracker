namespace Common.Interfaces
{
    public interface IEmailSendService
    {
        bool SendEmail(string email, string subject, string message);
    }
}
