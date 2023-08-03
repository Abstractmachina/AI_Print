using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types {
    public class ResponseArtefacts {
		[JsonProperty("artifacts")]
        public List<ResponseObject_old> Artefacts { get; set; }
    }

    public class ResponseObject_old {
        [JsonProperty("base64")]
        public string Base64 { get; set; }
        [JsonProperty("finishReason")]
        public string FinishReason { get; set; }
        [JsonProperty("seed")]
        public Int64? Seed { get; set; }
    }
}
