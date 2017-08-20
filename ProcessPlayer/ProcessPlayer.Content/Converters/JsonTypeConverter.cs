using Newtonsoft.Json;
using System;

namespace ProcessPlayer.Content.Converters
{
    public class JsonTypeConverter : JsonConverter
    {
        #region overriden methods

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((reader.Value ?? string.Empty).ToString())
            {
                case "DateTime":
                    return typeof(DateTime);
                case "Number":
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value as Type;
            string typeName = null;

            if (type != null)
            {
                if (type == typeof(DateTime))
                    typeName = "DateTime";
                else if (type == typeof(byte)
                    || type == typeof(decimal)
                    || type == typeof(double)
                    || type == typeof(float)
                    || type == typeof(int)
                    || type == typeof(long)
                    || type == typeof(short)
                    || type == typeof(uint)
                    || type == typeof(ulong)
                    || type == typeof(ushort))
                    typeName = "Number";
            }

            serializer.Serialize(writer, typeName);
        }

        #endregion
    }
}
