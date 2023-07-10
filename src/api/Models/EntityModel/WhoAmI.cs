using api.Models.EntityModel.Users;

namespace api.Models.EntityModel
{
    public class WhoAmI
    {
        public User? User { get; set; }
        public bool AccessGranted { get; set; }

        public WhoAmI()
        {

        }
    }
}