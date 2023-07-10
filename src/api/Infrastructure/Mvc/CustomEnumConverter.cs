using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace api.Infrastructure.Mvc
{
    public class CustomEnumConverter : StringEnumConverter
    {
        private const int InvalidEnumValue = int.MinValue;

        public override object ReadJson(
            JsonReader reader
            , Type objectType
            , object existingValue
            , JsonSerializer serializer)
        {
            var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;

            if (reader.Value is string stringValue)
            {
                if (!Enum.TryParse(enumType, stringValue, true, out object validEnumValue))
                {
                    return Enum.ToObject(enumType, InvalidEnumValue);
                }

                return validEnumValue;
            }

            return Enum.ToObject(enumType, InvalidEnumValue);
        }
    }
}
