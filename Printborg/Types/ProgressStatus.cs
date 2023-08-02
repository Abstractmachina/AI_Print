using Newtonsoft.Json;

namespace Printborg.Types
{
    public class ProgressStatus
    {
        // EXAMPLE RESPONSE
        //   {
        //   "progress": 0,
        //   "eta_relative": 0,
        //   "state": {
        //     "skipped": false,
        //     "interrupted": false,
        //     "job": "",
        //     "job_count": 0,
        //     "job_timestamp": "20230731113349",
        //     "job_no": 1,
        //     "sampling_step": 0,
        //     "sampling_steps": 20
        //   },
        //   "current_image": null,
        //   "textinfo": null
        //   }

        [JsonProperty("progress")]
        public float Progress { get; set; }
        [JsonProperty("eta_relative")]
        public float Eta { get; set; }

        [JsonProperty("current_image")]
        public string? CurrentImage { get; set; }
        [JsonProperty("textinfo")]
        public string? TextInfo { get; set; }

        [JsonProperty("state")]
        public Stateobject stateObject { get; set; }

        public ProgressStatus() { }

        public class Stateobject
        {
            [JsonProperty("skipped")]
            public bool Skipped { get; set; }
            [JsonProperty("interrupted")]
            public bool Interrupted { get; set; }
            [JsonProperty("job")]
            public string Job { get; set; }
            [JsonProperty("job_count")]
            public int JobCount { get; set; }
            [JsonProperty("job_timestamp")]
            public string JobTimeStamp { get; set; }
            [JsonProperty("job_no")]
            public int JobNumber { get; set; }
            [JsonProperty("sampling_step")]
            public int SamplingStep { get; set; }
            [JsonProperty("sampling_steps")]
            public int SamplingSteps { get; set; }

            public Stateobject() { }

        }
    }
}
