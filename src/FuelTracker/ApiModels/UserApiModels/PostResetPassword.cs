using System.ComponentModel;

namespace FuelTracker.ApiModels.UserApiModels
{
    public class PostResetPassword
    {
        public string Code { get; set; }
        [DisplayName("Email")]
        public string Email{ get; set; }
        [DisplayName("New pmail")]
        public string Password { get; set; }
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
