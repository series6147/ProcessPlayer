using Newtonsoft.Json;
using ProcessPlayer.Content.Converters;
using System;

namespace ProcessPlayer.Content.Models
{
    public class Field
    {
        #region properties

        [JsonConverter(typeof(JsonTypeConverter))]
        public Type DataType { get; set; }

        public string Expression { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
