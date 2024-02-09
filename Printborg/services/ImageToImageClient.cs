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

        private readonly IApiController _controller = null;

        #region CONSTRUCTORS
        public ImageToImageClient() {

        }

        public ImageToImageClient(IApiController controller) {
            _controller = controller;
        }

        #endregion

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
            string response = await _controller.POST_Batch(payload);

            // response gets processed and returns a standardized return statement (accepted, failed)
            IJobReceipt processedResponse = processCreateJobResponse(response);
            return processedResponse;
        }


        /// <summary>
        /// Get all jobs in queue (including cancelled, finished and failed)
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, IJob>> GetAllJobs() {

            if (_controller.GetType() == typeof(DeforumController)) {
                var rawResponse = JsonConvert.DeserializeObject<Dictionary<string, DeforumJob>> (await _controller.GET_Jobs());

                var Idict = new Dictionary<string, IJob>();
                foreach (KeyValuePair<string, DeforumJob> entry in rawResponse) {
                    Idict.Add(entry.Key, entry.Value);
                }
                return Idict;
            }

            return null;
        }


        /// <summary>
        /// Get all jobs matching id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dictionary with id as key, and IJob as value</returns>
        public async Task<IJob[]> GetJob(string id) {
            var rawResponse = await _controller.GET_Batch(id);

            if (_controller.GetType() == typeof(DeforumController)) {
                return JsonConvert.DeserializeObject<DeforumJob[]>(rawResponse);
            }

            return null;
        }


        /// <summary>
        /// Get progress of specified job as a double (0 - 1). If invalid, will return a negative value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>progress as decimal value</returns>
        public async Task<double> GetProgress(string id) {
            double progress = await _controller.GET_Progress(id);

            try {
                return Convert.ToDouble(progress);

            } catch (Exception exception) {
                Console.WriteLine($"Error ImageToImageClient.GetProgress(id): {exception.Message}");
                return -1d;
            }
        }

        public async Task<IBatchReceipt> CancelJob(string id) {

            var response = await _controller.DELETE_Batch(id);
            if (_controller.GetType() == typeof (DeforumController)) {
                return JsonConvert.DeserializeObject<DeforumCancelBatchReceipt>(response);

            }
            return null;

        }
        public async Task<List<string>> CancelAllJobs() {
            //get all job ids

            //cancel jobs one by one?
            var result = await _controller.DELETE_Batches();
            return result;

        }


            //========================  UTILITY ===============================
            /// <summary>
            /// converts raw string response into a standardized object for ease of data passing.
            /// </summary>
            /// <param name="rawResponse"></param>
            /// <returns></returns>
            private IJobReceipt processCreateJobResponse(string rawResponse) {
           if (_controller.GetType() == typeof(DeforumController)) {
                var deforumReceipt = JsonConvert.DeserializeObject<DeforumBatchReceipt>(rawResponse);

                return deforumReceipt;
            }

            return null;

        }
    }
}
