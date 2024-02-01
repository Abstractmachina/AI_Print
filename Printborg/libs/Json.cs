using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Printborg.Types;
using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Printborg.Types.ProgressStatus;

namespace Printborg.Utilities {

    public class DeforumJobResponseConverter : JsonConverter {
        public override bool CanConvert(Type objectType) => objectType == typeof(Status);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }

    public class StatusConverter : JsonConverter {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(Status);

        // False because we don't need to use this converter for serialisation
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var raw = reader.Value as string;

            if (raw != "") {
                raw = raw.ToLower();
                if (raw.Contains( "succeeded")) {
                    return Status.SUCCESS;
                }
                else if (raw.Contains("accepted")) {
                    return Status.ACCEPTED;
                }
                else if (raw.Contains("cancelled")) {
                    return Status.CANCELLED;
                }
                else {
                    return Status.FAILURE;
                }
            }
            return Status.FAILURE;



            //get json object
            //var jsonObject = JObject.Load(reader);
            //var accountType = jsonObject.GetValue("accountType", StringComparison.OrdinalIgnoreCase)?.ToObject<AccountType>();

                //Console.WriteLine(jsonObject.ToString());
                //var statusObject = jsonObject.GetValue("status", StringComparison.OrdinalIgnoreCase);

                //if (statusObject != null) {
                //    string statusString = statusObject.Value<string>().ToLower();
                //    if (statusString == "succeeded") {
                //        return Status.SUCCESS;
                //    }
                //    else if (statusString == "accepted") {
                //        return Status.ACCEPTED;
                //    } else {
                //        return Status.FAILURE;
                //    }

                //}
                //return Status.FAILURE;
            }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            // This won't be used because we don't need to use this converter for serialisation
            throw new NotImplementedException();
        }
    }

    public class IntStringDictConverter : JsonConverter {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(Dictionary<int, string>);

        public override bool CanWrite => true;
        public override bool CanRead => false;

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
            if (value is null) return;

            writer.WriteStartObject();
            foreach (KeyValuePair<int, string> entry in (Dictionary<int, string>) value) {
                writer.WritePropertyName(entry.Key.ToString());
                writer.WriteValue(entry.Value);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }


        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        //}

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        //    if (reader.TokenType == JsonToken.Null)
        //        return null;

        //    var raw = reader.Value as string;

        //    if (raw != "") {
        //        raw = raw.ToLower();
        //        if (raw.Contains("succeeded")) {
        //            return Status.SUCCESS;
        //        }
        //        else if (raw.Contains("accepted")) {
        //            return Status.ACCEPTED;
        //        }
        //        else if (raw.Contains("cancelled")) {
        //            return Status.CANCELLED;
        //        }
        //        else {
        //            return Status.FAILURE;
        //        }
        //    }
        //    return Status.FAILURE;



        //    //get json object
        //    //var jsonObject = JObject.Load(reader);
        //    //var accountType = jsonObject.GetValue("accountType", StringComparison.OrdinalIgnoreCase)?.ToObject<AccountType>();

        //    //Console.WriteLine(jsonObject.ToString());
        //    //var statusObject = jsonObject.GetValue("status", StringComparison.OrdinalIgnoreCase);

        //    //if (statusObject != null) {
        //    //    string statusString = statusObject.Value<string>().ToLower();
        //    //    if (statusString == "succeeded") {
        //    //        return Status.SUCCESS;
        //    //    }
        //    //    else if (statusString == "accepted") {
        //    //        return Status.ACCEPTED;
        //    //    } else {
        //    //        return Status.FAILURE;
        //    //    }

        //    //}
        //    //return Status.FAILURE;
        //}
    }
}
