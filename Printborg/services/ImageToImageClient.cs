using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Interfaces;
using Printborg.Types.Deforum;
using Printborg.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Services {

    /// <summary>
    /// The ImageToImageClient ensures that responses received from APIs are processed into standardized objects
    /// </summary>
    public class ImageToImageClient {

        private IApiController _controller = null;

        public ImageToImageClient() {

        }

        public ImageToImageClient(IApiController controller) {
            _controller = controller;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<JobResponse?> CreateJob(string payload) {
            string response = await _controller.POST_Job(payload);

            // response gets processed and returns a standardized return statement (accepted, failed)
            JobResponse? processedResponse = processCreateJobResponse(response);
            return processedResponse;
        }

        private JobResponse? processCreateJobResponse(string rawResponse) {
           if (_controller.GetType() == typeof(DeforumController)) {
                // response format example
                //{
                //    "message": "Job(s) accepted",
                //    "batch_id": "batch(843362695)",
                //    "job_ids": [
                //        "batch(843362695)-0"
                //    ]
                //}
                JobResponse response = new JobResponse();
                var json = JsonConvert.DeserializeObject<DeforumJobResponseConverter>(rawResponse);
                if (json.Message.Contains("accepted")) {
                    response.Status = Status.ACCEPTED;
                    response.BatchId = json.BatchId;
                    response.JobIds = json.JobIds;
                }

                return response;
            }

            return null;

        }
    }
}
