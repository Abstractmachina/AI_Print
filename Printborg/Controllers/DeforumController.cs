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

namespace Printborg.Controllers
{
    public class DeforumController : IApiController
    {


        private string _baseAddress = "";
        private int _timeout = 0;


        #region CONSTRUCTORS
        /// <summary>
        /// Instantiates controller with default stable diffusion local address http://127.0.0.1:7860/
        /// </summary>
        public DeforumController()
        {
            _baseAddress = "http://127.0.0.1:7860/";
        }
        public DeforumController(string baseAddress)
        {
            _baseAddress = baseAddress;
        }
        public DeforumController(int timeout) : this()
        {
            _timeout = timeout;
        }
        public DeforumController(string baseAddress, int timeout) : this(baseAddress)
        {
            _timeout = timeout;

        }

        
        #endregion

        public Task DELETE_Job(string id)
        {
            throw new NotImplementedException();
        }

        public Task DELETE_allJobs() {
            throw new NotImplementedException();
        }

        public Task GET_allJobs()
        {
            throw new NotImplementedException();
        }

        public Task GET_Job(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> POST_Job(string payload)
        {

            string endpoint = "deforum_api/batches";

            if (_baseAddress == null || _baseAddress == "") throw new ArgumentException(_baseAddress, "Invalid baseAddress");


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                if (_timeout == 0) client.Timeout = Timeout.InfiniteTimeSpan;
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
    }

    
}
