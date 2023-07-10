using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace api.Infrastructure.Mvc
{
    public class ModelErrorMessage
    {
        private ModelError _modelError;

        public ModelErrorMessage(ModelError modelError)
        {
            _modelError = modelError;
        }

        public override string ToString() 
        {
            if (!string.IsNullOrWhiteSpace(_modelError.ErrorMessage))
            {
                return _modelError.ErrorMessage;
            }

            if (_modelError.Exception is JsonReaderException readerException)
            {
                return readerException.Message;
            }

            if (_modelError.Exception is JsonSerializationException serializationException)
            {
                var matches = Regex.Matches(serializationException.Message, "Path.*");

                if (matches.Any())
                {
                    return $"Parse error: {matches.First().Value}";
                }
            }

            return "Could not to read the request body.";
        }
    }
}