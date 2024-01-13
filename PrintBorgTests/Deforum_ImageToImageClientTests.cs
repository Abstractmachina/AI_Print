using Printborg.Controllers;
using Printborg.Services;

namespace PrintBorgTests {

    [Collection("Sequential")]

    public class Deforum_ImageToImageClientTests {

        private readonly ITestOutputHelper _output;
        private readonly ImageToImageClient _client;

        public Deforum_ImageToImageClientTests(ITestOutputHelper output) {
            var controller = new DeforumController();
            _client = new ImageToImageClient(controller);
            _output = output;
        }

        [Fact]
        private async Task CheckIfServerIsOnline() {
            bool isOnline = await _client.CheckOnlineStatus();
            Assert.True(isOnline);
        }


        [Fact]
        private async Task SubmittingJobReturnsValidIJobReceipt() {

            string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
            var response = await _client.SubmitJob(payload);

            Assert.IsAssignableFrom<IJobReceipt>(response);
            Assert.NotNull(response.Id);
            Assert.NotEqual("", response.Id);
        }


        [Fact]
        private async Task SubmittingJobAddsToJobQueue() {
            string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
            var response = await _client.SubmitJob(payload);

            string currentId = response.Id;

            // var response = await controller.POST_Job(payload);

            _output.WriteLine($"Submitted JobId: {currentId}");
            _output.WriteLine("... getting queued jobs ...");

            var jobsJson = await _client.GetAllJobs();
            var jobs = JsonConvert.DeserializeObject<Dictionary<string, DeforumJob>>(jobsJson);

            bool containsId = false;
            foreach (KeyValuePair<string, DeforumJob> entry in jobs) {
                _output.WriteLine( $"Checking {entry.Key}" );
                if (entry.Key.Contains(currentId)) {
                    _output.WriteLine("Id is in queue!");
                    containsId = true; 
                    break;
                }
            }

            Assert.True(containsId);
        }

        //[Fact]
        //private async Task SubmitJobWithProgressReporting() {

        //    var client = new ImageToImageClient(new DeforumController());

        //    IApiController controller = new DeforumController();

        //    string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");


        //    // string response = await controller.POST_Job(payload);


        //    var response = await client.SubmitJob(payload);
        //    Console.WriteLine(response);

        //    string currentId = response.Id;
        //    Console.WriteLine("Current Id is: " + currentId);

        //    bool _isFinished = false;

        //    var rawResponse = await controller.GET_Batch(currentId);

        //    var job2 = JsonConvert.DeserializeObject<DeforumJob[]>(rawResponse);
        //    _output.WriteLine("/n/nnumber of batches in job: " + job2.Length);
        //    foreach (var j in job2) {
        //        Console.WriteLine($"Id: {j.Id}, Status: {j.Status}, Progress: {j.Progress}, Phase: {j.Phase}, message: {j.Message}");
        //    }
        //    return;
        //    while (!_isFinished) {
        //        var job = await client.GetJob(currentId);
        //        //check status. if status is SUCCESS, it means job is finished. 
        //        var batch = job.First().Value;
        //        if (batch.Status == Status.SUCCESS) {
        //            Console.WriteLine($"Done");
        //            _isFinished = true;
        //            break;
        //        }
        //        //get progress
        //        Console.WriteLine($"Generating ... [{batch.Progress}]");
        //        Thread.Sleep(1000); // check every 1sec
        //    }

        //    Console.WriteLine("Job Complete. Exiting.");
        //}

    }
}