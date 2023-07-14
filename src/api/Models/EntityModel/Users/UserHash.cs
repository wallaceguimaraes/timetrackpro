
namespace api.Models.EntityModel.Users
{
    public class UserHash
    {
        public const int DaysToExpire = 2;

        public long UserId { get; set; }
        public string Salt { get; set; }
        public long ApplicationId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Expired => DateTime.Now >= ExpirationDate;

        public UserHash() { }


    }
}
