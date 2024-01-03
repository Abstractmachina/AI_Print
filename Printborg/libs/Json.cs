using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Printborg.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Utilities
{
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
