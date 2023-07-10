using System.ComponentModel.DataAnnotations;

namespace api.Validations
{
    public class JsonMaxLength : MaxLengthAttribute
    {
        public JsonMaxLength(int length) : base(length)
        {
            ErrorMessage = "{0}: Maximum {1} characters.";
        }
    }
}