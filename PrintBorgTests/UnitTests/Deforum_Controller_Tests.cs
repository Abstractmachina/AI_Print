using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace PrintBorgTests.UnitTests {
    public class Deforum_Controller_Tests {
        private readonly ITestOutputHelper _output;
        private readonly string _payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
        private readonly DeforumController _controller;

        public Deforum_Controller_Tests(ITestOutputHelper output) {
            _output = output;
            _controller = new DeforumController();
        }

        [Fact]
        private async Task CanDeleteOneBatchById() {

            try {
                _output.WriteLine("... Submitting test payload ...");
                var PostResponse = await _controller.POST_Batch(_payload);
                _output.WriteLine(PostResponse);
                var convertedReceipt = JsonConvert.DeserializeObject<DeforumBatchReceipt>(PostResponse);
                string currentId = convertedReceipt.Id;
                _output.WriteLine($"Job Receipt: {convertedReceipt.Id}; {convertedReceipt.Status}");
                _output.WriteLine("Job Ids:");
                foreach (var id in convertedReceipt.JobIds) {
                    _output.WriteLine($"{id}");
                }

                Thread.Sleep(1000);

                _output.WriteLine("... Canceling Job ...");
                var cancelResponse = await _controller.DELETE_Batch(currentId);
                _output.WriteLine($"raw string: {cancelResponse}");
                var receipt = JsonConvert.DeserializeObject<DeforumCancelBatchReceipt>(cancelResponse);
                _output.WriteLine($"converted receipt: {receipt.Id}; {receipt.Status}");
                Assert.True(receipt.Status == Status.CANCELLED);

            }
            catch(Exception e) {
                _output.WriteLine($"Deleting batch failed: {e.Message}");
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        private async Task CanDeleteAllBatches() {
            try {

                // submit jobs
                _output.WriteLine("... Submitting test payloads ...");

                var postResponseList = new List<DeforumBatchReceipt>();

                for (int i = 0; i < 3; i++) {
                    var r = await _controller.POST_Batch(_payload);
                    _output.WriteLine(r);

                    postResponseList.Add(JsonConvert.DeserializeObject<DeforumBatchReceipt>(r));
                }

                Thread.Sleep(2000);

                _output.WriteLine("... Getting all batches ...");
                var getBatchesRes = await _controller.GET_Batches();
                _output.WriteLine(getBatchesRes);
                var batchesResObjects = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(getBatchesRes);
                _output.WriteLine("Converted response objects:");
                foreach (KeyValuePair<string, string[]> entry in batchesResObjects) {
                    _output.WriteLine(entry.Key);
                    foreach (string value in entry.Value) _output.WriteLine($"\t{value}");
                }

                _output.WriteLine("... Cancelling Jobs ...");

                var cancelResponseList = new List<DeforumCancelBatchReceipt>();
                _output.WriteLine("raw cancel responses");
                foreach (KeyValuePair<string, string[]> entry in batchesResObjects) {
                    var res = await _controller.DELETE_Batch(entry.Key);
                    _output.WriteLine(res);
                    cancelResponseList.Add(JsonConvert.DeserializeObject<DeforumCancelBatchReceipt>(res));
                }

                foreach (var r in cancelResponseList) {
                    _output.WriteLine($"converted receipt: {r.Id}; {r.Status}");
                    Assert.True(r.Status == Status.CANCELLED);
                }


            }
            catch (Exception e) {
                _output.WriteLine($"Deleting batches failed: {e.Message}");
                Assert.Fail(e.Message);
            }
        } 
    }
}
