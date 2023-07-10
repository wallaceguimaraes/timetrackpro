using api.Validations;
using Newtonsoft.Json;

namespace api.Models.ViewModel
{
    public class CredentialModel
    {
        [JsonProperty("login"), JsonRequiredValidate]
        public string? Login { get; set; }

        [JsonProperty("password"), JsonRequiredValidate]
        public string? Password { get; set; }
    }
}