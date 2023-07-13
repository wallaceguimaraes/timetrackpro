using api.Models.EntityModel.Users;
using api.Validations;
using Newtonsoft.Json;

namespace api.Models.ViewModel.Users
{
    public class UserModel
    {
        [JsonProperty("name"), JsonRequiredValidate, JsonMaxLength(45)]
        public string? Name { get; set; }

        [JsonProperty("email"), JsonRequiredValidate, JsonEmail]
        public string? Email { get; set; }

        [JsonProperty("login"), JsonRequiredValidate, JsonMaxLength(30)]
        public string? Login { get; set; }

        [JsonProperty("password"), JsonRequiredValidate]
        public string? Password { get; set; }

        public User Map()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Login = Login,
                Password = Password
            };
        }

        public User Map(User user)
        {
            user.Name = Name ?? user.Name;
            user.Email = Email ?? user.Email;
            user.Login = Login ?? user.Login;
            user.Password = Password ?? user.Password;
            user.LastUpdateAt = DateTime.Now;

            return user;
        }
    }
}