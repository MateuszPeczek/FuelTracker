using System.ComponentModel.DataAnnotations;

namespace FuelTracker.ApiModels.UserApiModels
{
    public class PutUser
    {
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
    }
}
