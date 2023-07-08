// using api.Infrastructure.Security;
// using Newtonsoft.Json;

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

        // public UserHash(User user, long applicationId)
        // {
        //     UserId = user.PersonId;
        //     ApplicationId = applicationId;
        //     ExpirationDate = DateTime.Now.AddDays(DaysToExpire);
        //     Salt = user.Salt;
        // }

        // public override string ToString()
        // {
        //     var json = JsonConvert.SerializeObject(this);
        //     var hash = new UrlEncryption().Encrypt(json);

        //     return hash.Replace("+", "-").Replace("/", "_");
        // }

        // public static UserHash Decode(string hash)
        // {
        //     try
        //     {
        //         var hashReplaced = hash.Replace("-", "+").Replace("_", "/");
        //         var decrypted = new UrlEncryption().Decrypt(hashReplaced);

        //         return JsonConvert.DeserializeObject<UserHash>(decrypted);
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }
    }
}
