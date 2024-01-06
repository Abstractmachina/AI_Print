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
    /// <summary>
    /// Conversion object for response messages received from stable diffusion deforum
    /// </summary>
    public class DeforumJobResponseConverter {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("batch_id")]
        public string BatchId { get; set; }
        [JsonProperty("job_ids")]
        public List<string> JobIds { get; set; }
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
            if (raw == "succeeded") {
                return Status.SUCCESS;
            }
            else if (raw == "accepted") {
                return Status.ACCEPTED;
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