using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

using AI_Print.Json;
using AI_Print.Types;

namespace AI_Print {
	static internal class ImagePrompter {


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


        public static async Task<string?> POST_TextToImage(string? apiKey) {
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
        /// send request to stablediffusionapi.com. Not used, as I found out that it's a third party API.
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