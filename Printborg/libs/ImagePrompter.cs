using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using System.Threading;
using Printborg.Types;
using Printborg.Json;

namespace Printborg {
	static public class ImagePrompter {

        private static async Task<string> Skip(string baseAddress, HttpClient client)
        {
            string endpoint = "/sdapi/v1/skip";

            var response = await client.PostAsync(endpoint, null);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<ResponseObject> Auto1111TextToImage(string baseAddress, Auto1111Payload payload)
        {

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = Timeout.InfiniteTimeSpan;

                // IApiHandler handler = new Auto1111Handler(client);

                // skip any jobs that are running from before
                var interruptStatus = await Skip(baseAddress, client);
                Console.WriteLine(interruptStatus);
                string uri = baseAddress + "/sdapi/v1/txt2img";

                var requestMsg = new HttpRequestMessage();
                var json = JsonConvert.SerializeObject(payload);

                // requestMsg.Content = new StringContent(json);
                // requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine("test");
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = client.PostAsync("/sdapi/v1/txt2img", content);


                // continuously poll api for task progress until completed
                while (!response.IsCompleted)
                {
                    Console.WriteLine("still in progress ...");
                    await pollProgess(baseAddress, client);

                    await Task.Delay(2000);
                }

                Console.WriteLine("Image generation finished ...");

                var result = await response.Result.Content.ReadAsStringAsync();

                var resultObject = JsonConvert.DeserializeObject<ResponseObject>(result);
                return resultObject;
            }
        }

        private static async Task pollProgess(string baseAddress, HttpClient client)
        {
            string progressUri = baseAddress + "/sdapi/v1/progress";

            var progressResponse = await client.GetAsync(progressUri);
            progressResponse.EnsureSuccessStatusCode();

            string responseContent = await progressResponse.Content.ReadAsStringAsync();
            var status = JsonConvert.DeserializeObject<ProgressStatus>(responseContent);
            if (status != null)
            {
                Console.WriteLine("Progress: " + status.Progress);
                Console.WriteLine("estimated eta: " + status.Eta);
                Console.WriteLine("Job: " + status.stateObject.Job);
            }
        }

        public static async Task<string> Auto1111_T2I(string address, string username, string password, Auto1111Payload payload) {
            using (HttpClient client = new HttpClient()) {
                Console.WriteLine("... Attempting to send POST request /sdapi/v1/txt2img");
                string endpoint = address + "/sdapi/v1/txt2img";

                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

                var json = JsonConvert.SerializeObject(payload);
                request.Content = new StringContent(json);

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.SendAsync(request);
                Console.WriteLine(response.StatusCode);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("... results coming in ....");


                return result;
            }
        }

         

        /// <summary>
        /// OBSOLETE Text to Image for Stability AIs official API. requires account and credits. 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static async Task<string> POST_TextToImage(string apiKey) {
            using (HttpClient client = new HttpClient()) {
                try {
                    if (apiKey == null) throw new Exception("Missing Stability API key.");

                    // set up test request object
                    var payload = new RequestPayload(
                        new List<TextPrompt>{
                            new TextPrompt("two red apples fighting", 1.0f)
                        },
                        512, 512
                    );

                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.stability.ai/v1/generation/stable-diffusion-v1-5/text-to-image");
                    request.Headers.Add("Authorization", apiKey);
                    request.Content = new StringContent(JsonConvert.SerializeObject(payload));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // send request and receive response, which is converted to a json string
                    var response = await client.SendAsync(request);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                    //var responseObject = JsonConvert.DeserializeObject<ResponseArtefacts>(responseContent);
                    //if (responseObject == null) throw new Exception("API request failed. ResponseObject is null");
                    // var rr = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseObject.artifacts[0]);
                    // foreach(KeyValuePair<string, object>pair in rr) {
                    //     Console.WriteLine(pair.Key);
                    //     Console.WriteLine(pair.Value);
                    // }
                    // Console.WriteLine(responseObject.artifacts.Count);
                    // foreach (var v in responseObject.artifacts)
                    // Console.WriteLine(v);
                    // foreach(KeyValuePair<string, object>item in responseObject) Console.WriteLine(item.Key.GetType());
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
        }



        /// <summary>
        /// OBSOLETE send request to stablediffusionapi.com. Not used, as I found out that it's a third party API.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static async Task<string> SendTextToImagePromptRequest (string apiKey, string prompt) {
			var client = new HttpClient();
			try {
				var newPrompt = new TextToImagePrompt(apiKey, prompt);

				var settings = new JsonSerializerSettings {
					Converters = { new T2IJsonConverter() }
				};

				StringContent body = new StringContent(JsonConvert.SerializeObject(newPrompt, settings));
				body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				Console.WriteLine(await body.ReadAsStringAsync());

				HttpResponseMessage response = await client.PostAsync("https://stablediffusionapi.com/api/v3/text2img", body);
				if (!response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync();
					throw new Exception(content);
				}
				var result = await response.Content.ReadAsStringAsync();
				Console.WriteLine(result);
				var json = JsonConvert.DeserializeObject(result);

				return result;
			}
			catch (Exception err) {
				Console.WriteLine(err.ToString());
				return err.ToString();
			}
		}
	}
}