using System.ComponentModel.DataAnnotations;

namespace api.Validations
{
    public class JsonValidIf : ValidationAttribute
    {
        public JsonValidIf()
        {
            ErrorMessage = "{0}: Invalid.";
        }

        public override bool IsValid(object value)
        {
            if (value is bool valid)
            {
                return valid;
            }

            throw new ArgumentException("This property must to return a Boolean.");
        }
    }
}
