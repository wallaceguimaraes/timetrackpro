using System.ComponentModel.DataAnnotations;

namespace api.Validations
{
    public class JsonRequiredValidate : RequiredAttribute
    {
        public JsonRequiredValidate()
        {
            ErrorMessage = "{0}: Required.";
        }
    }
}