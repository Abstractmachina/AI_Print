using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.interfaces;
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
    /// The ImageToImageClient ensures a unified interface with user-defined API controllers. 
    /// </summary>
    public class ImageToImageClient {

        private IApiController _controller = null;

        public ImageToImageClient() {

        }

        public ImageToImageClient(IApiController controller) {
            _controller = controller;
        }

        public async Task<bool> CheckOnlineStatus() {
            try {
                if (_controller.GetType() == typeof(DeforumController)) {
                    var id = await ((DeforumController)_controller).GetAppId();
                    Console.WriteLine(id);
                    if (id != null || id != "") return true;
                }
            }
            catch (Exception exception) {
                return false;
            }
            

            return false;
        }


        /// <summary>
        /// Sends job via API POST endpoint to server. Note that the payload has to match the requirements of the API, it is currently not validated. 
        /// </summary>
        /// <returns>Job receipt informing user of status and job id</returns>
        public async Task<IJobReceipt> SubmitJob(string payload) {
            string response = await _controller.POST_Job(payload);

            // response gets processed and returns a standardized return statement (accepted, failed)
            IJobReceipt processedResponse = processCreateJobResponse(response);
            return processedResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetJobs() {
            var rawResponse = await _controller.GET_Jobs();
            return rawResponse;
        }


        /// <summary>
        /// Get all jobs matching id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dictionary with id as key, and IJob as value</returns>
        public async Task<Dictionary<string, IJob>> GetJob(string id) {
            var rawResponse = await _controller.GET_Batch(id);

            var jobs = JsonConvert.DeserializeObject<Dictionary<string, IJob>>(rawResponse);

            return jobs;
        }



        /// <summary>
        /// converts raw string response into a standardized object for ease of data passing.
        /// </summary>
        /// <param name="rawResponse"></param>
        /// <returns></returns>
        private IJobReceipt processCreateJobResponse(string rawResponse) {
           if (_controller.GetType() == typeof(DeforumController)) {
                var deforumReceipt = JsonConvert.DeserializeObject<DeforumJobReceipt>(rawResponse);

                return deforumReceipt;
            }

            return null;

        }
    }
}
