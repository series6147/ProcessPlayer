using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessPlayer.Content.Converters
{
    public class JsonProcessContentsConverter : JsonConverter
    {
        #region private variables

        private static Dictionary<string, Type> _contentMapping;

        #endregion

        #region public methods

        public static void SetMappingTypes(IEnumerable<Type> types)
        {
            _contentMapping = types.ToDictionary(t => t.Name.ToLower(), t => t);
        }

        #endregion

        #region JsonConverter Members

        public override bool CanConvert(Type objectType)
        {
            return objectType != null && objectType.IsSubclassOf(typeof(IEnumerable<ProcessContent>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string property;
            var res = new List<ProcessContent>();

            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                if (reader.TokenType == JsonToken.StartObject)
                {
                    reader.Read();

                    property = (reader.Value ?? string.Empty).ToString().ToLower();

                    reader.Read();

                    if (_contentMapping.ContainsKey(property))
                    {
                        var content = serializer.Deserialize(reader, _contentMapping[property]) as ProcessContent;

                        res.Add(content);
                    }
                    else
                        throw new Exception(string.Format("Content '{0}' is not registered in system.", property));
                }

            var incoming =
                (from rq in res.Where(rq => rq.OutgoingIDs != null)
                 from id in rq.OutgoingIDs
                 group rq.ID by id into gs
                 select gs).ToDictionary(g => g.Key, g => g);
            var outgoing = res.Where(r => !string.IsNullOrEmpty(r.ID)).ToDictionary(r => r.ID, r => r);

            foreach (var r in res)
            {
                r.IncomingLinks = incoming.ContainsKey(r.ID)
                    ? incoming[r.ID].Where(id => outgoing.ContainsKey(id)).Select(id => outgoing[id])
                    : null;
                r.OutgoingLinks = r.OutgoingIDs == null
                    ? null
                    : r.OutgoingIDs.Where(id => !string.IsNullOrEmpty(id) && outgoing.ContainsKey(id)).Select(id => outgoing[id]);
            }

            return res;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
