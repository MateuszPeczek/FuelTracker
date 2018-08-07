namespace FuelTracker.ApiModels.UserApiModels
{
    public class PostResetPassword
    {
        public string Code { get; set; }
        public string Email{ get; set; }
        public string Password { get; set; }
        public string ResetPassword { get; set; }
    }
}
