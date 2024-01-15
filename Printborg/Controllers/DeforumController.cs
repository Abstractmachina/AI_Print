using Printborg.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using Printborg.Types;
using Printborg.Types.Deforum;

namespace Printborg.Controllers {
    public class DeforumController : IApiController {

        // MEMBERS
        private string _baseAddress = "";
        private int _timeout = 0;

        #region PROPERTIES
        public string BaseAddress {
            get { return _baseAddress; }
            set { _baseAddress = value; }
        }

        public int Timeout {
            get { return _timeout; }
            set { _timeout = value; }
        }
        #endregion //PROPERTIES

        #region CONSTRUCTORS
        /// <summary>
        /// Instantiates controller with default stable diffusion local address http://127.0.0.1:7860/
        /// </summary>
        public DeforumController() {
            _baseAddress = "http://127.0.0.1:7860/";
        }
        public DeforumController(string baseAddress) {
            _baseAddress = baseAddress;
        }
        public DeforumController(int timeout) : this() {
            _timeout = timeout;
        }
        public DeforumController(string baseAddress, int timeout) : this(baseAddress) {
            _timeout = timeout;

        }


        #endregion


        public async Task<string> GetAppId() {
            string endpoint = $"/app_id";

            using (var client = new HttpClient()) {
                Setup(client);
                var response = await client.GetAsync(endpoint);
                return await response.Content.ReadAsStringAsync();
            }
        }
        public async Task<string> DELETE_Job(string id) {
            string endpoint = $"deforum_api/jobs/{id}";
            using (var client = new HttpClient()) {
                Setup(client);

                var response = await client.DeleteAsync(endpoint);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<List<string>> DELETE_Jobs() {
            var jobIds = JsonConvert.DeserializeObject<Dictionary<string, DeforumJob>>(await this.GET_Jobs()).Select(j => j.Value.Id);

            var outList = new List<string>();
            foreach (var id in jobIds) {
                var response = await this.DELETE_Job(id);
                var status = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                outList.Add($"{status["id"]} : {status["message"]}");
            }
            return outList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GET_Jobs() {
            string endpoint = "deforum_api/jobs";

            using (var client = new HttpClient()) {
                Setup(client);

                var response = await client.GetAsync(endpoint);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public Task<string> GET_Job(string id) {
            throw new NotImplementedException();
        }

        public async Task<string> POST_Job(string payload) {
            const string endpoint = "deforum_api/batches";

            ValidateBaseAddress();


            using (HttpClient client = new HttpClient()) {
                client.BaseAddress = new Uri(_baseAddress);
                if (_timeout == 0) client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                else client.Timeout = TimeSpan.FromSeconds(_timeout);

                // prep payload
                //var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                // send post request
                var rawResponse = await client.PostAsync(endpoint, content);
                return await rawResponse.Content.ReadAsStringAsync();

                //var myDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(await rawResponse.Content.ReadAsStringAsync());
                // response format example
                //{
                //    "message": "Job(s) accepted",
                //    "batch_id": "batch(843362695)",
                //    "job_ids": [
                //        "batch(843362695)-0"
                //    ]
                //}
            }
        }

        public async Task<string> GET_Batch(string id) {
            // example response:
            // [
            //    {
            //      "id": "batch(948023533)-0",
            //      "status": "SUCCEEDED",
            //      "phase": "DONE",
            //      "error_type": "NONE",
            //      "phase_progress": 1.0,
            //      "started_at": 1693060416.648067,
            //      "last_updated": 1693061418.838495,
            //      "execution_time": 1002.19042801857,
            //      "update_interval_time": 1.8368990421295166,
            //      "updates": 35,
            //      "message": null,
            //      "outdir": "D:\\Repos\\stable-diffusion-webui\\outputs\\img2img-images\\Deforum_01",
            //      "timestring": "20230826163336",
            //      "deforum_settings": {
            //      ...
            //      },
            //      "options_overrides": null
            //    }
            // ]

            string endpoint = $"deforum_api/batches/{id}";

            ValidateBaseAddress();

            using (HttpClient client = new HttpClient()) {
                Setup(client);

                // send post request
                var rawResponse = await client.GetAsync(endpoint);
                return await rawResponse.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Not implemented in the deforum API currently
        /// </summary>
        /// <returns></returns>
        public async Task<string> GET_Batches() {
            string endpoint = "deforum_api/batches";

            using (var client = new HttpClient()) {
                Setup(client);

                var response = await client.GetAsync(endpoint);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> DELETE_Batch(string id) {
            ValidateBaseAddress();
            string endpoint = $"deforum_api/batches/{id}";

            using (var client = new HttpClient()) {
                Setup(client);

                var response = await client.DeleteAsync(endpoint);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<double> GET_Progress(string id) {

            //get batch based on id
            var job = await this.GET_Batch(id);

            //deserialize json string to a generic dictionary
            var dic = JsonConvert.DeserializeObject<DeforumJob[]>(job); 
            double progress = dic[0].Progress;

            return progress;
        }


        //==============    UTILITY ========================

        /// <summary>
        /// Set up client with default settings (base address, timeout, etc.)
        /// </summary>
        /// <param name="client"></param>
        private void Setup(HttpClient client) {
            client.BaseAddress = new Uri(_baseAddress);
            if (_timeout == 0) client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
            else client.Timeout = TimeSpan.FromSeconds(_timeout);
        }

        private void ValidateBaseAddress() {
            if (_baseAddress == null || _baseAddress == "") throw new ArgumentException(_baseAddress, "Invalid baseAddress");
        }
    }



}
