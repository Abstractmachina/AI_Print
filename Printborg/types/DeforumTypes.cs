using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Printborg.Interfaces;
using Printborg.interfaces;
using Printborg.Utilities;

namespace Printborg.Types.Deforum {

    /// <summary>
    /// 
    /// </summary>
    public class DeforumJob : IJob {
        //[
        //    {
        //        "id": "batch(948023533)-0",
        //        "status": "SUCCEEDED",
        //        "phase": "DONE",
        //        "error_type": "NONE",
        //        "phase_progress": 1.0,
        //        "started_at": 1693060416.648067,
        //        "last_updated": 1693061418.838495,
        //        "execution_time": 1002.19042801857,
        //        "update_interval_time": 1.8368990421295166,
        //        "updates": 35,
        //        "message": null,
        //        "outdir": "D:\\Repos\\stable-diffusion-webui\\outputs\\img2img-images\\Deforum_01",
        //        "timestring": "20230826163336",
        //        "options_overrides": null
        //    }
        //]

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public Status Status { get; set; }
        [JsonProperty("phase")]
        public string Phase { get; set; }
        [JsonProperty("error_type")]
        public string ErrorType { get; set; }
        [JsonProperty("phase_progress")]
        public double Progress { get; set; }
        [JsonProperty("started_at")]
        public double StartedAt { get; set; }
        [JsonProperty("last_updated")]
        public double LastUpdated { get; set; }
        [JsonProperty("execution_time")]
        public double ExecutionTime { get; set; }
        [JsonProperty("update_interval_time")]
        public double UpdateIntervalTime { get; set; }
        [JsonProperty("updates")]
        public int Updates { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("outdir")]
        public string Outdir { get; set; }
        [JsonProperty("timestring")]
        public string Timestring { get; set; }

        public DeforumJob() { }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DeforumJobReceipt : IJobReceipt {
        // response format example
        //{
        //    "message": "Job(s) accepted",
        //    "batch_id": "batch(843362695)",
        //    "job_ids": [
        //        "batch(843362695)-0"
        //    ]
        //}
        [JsonProperty("message")]
        public Status Status { get; set; }
        [JsonProperty("batch_id")]
        public string Id { get; set; }
        [JsonProperty("job_ids")]
        public List<string> JobIds { get; set; }

        public override string ToString() {
            string output = $"{{ \nStatus: ({Status.ToString()})\nBatch Id: ({Id}) \n" +
            "Job IDs: ({ \n";
            foreach (var i in JobIds) {
                output += $"\t{i.ToString()}\n";
            }
            output += "}) }";
            return output;
        }
    }

    /// <summary>
    /// Testing to replace DeforumJobReceipt class. This way could simplify.
    /// </summary>
    public class DeforumBatchReceipt : IBatchReceipt {
        // note the extra colon in the property!! >:((
        [JsonProperty("message:")]
        public Status Status { get; set; }
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }
    }

    /// <summary>
    /// standardized status object to check job state.
    /// </summary>
    [JsonConverter(typeof(StatusConverter))]
    public enum Status {
        ACCEPTED, 
        SUCCESS,
        FAILURE,
        CANCELLED
    }
}
