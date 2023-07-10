using System.ComponentModel.DataAnnotations;

namespace api.Validations
{
    public class JsonEmail : RegularExpressionAttribute
    {
        public JsonEmail() : base(@"^[\.\w-]+@([\w-]+\.)+[\w-]+$")
        {
            ErrorMessage = "{0}: Invalid.";
        }
    }
}