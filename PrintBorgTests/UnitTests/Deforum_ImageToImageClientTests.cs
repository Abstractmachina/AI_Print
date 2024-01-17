using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.interfaces;
using Printborg.Interfaces;
using Printborg.Services;
using Printborg.Types.Deforum;
using Xunit.Abstractions;

namespace PrintBorgTests.UnitTests
{

    [Collection("Sequential")]

    public class Deforum_ImageToImageClientTests
    {

        private readonly ITestOutputHelper _output;
        private readonly ImageToImageClient _client;
        private readonly string _payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");

        public Deforum_ImageToImageClientTests(ITestOutputHelper output)
        {
            var controller = new DeforumController();
            _client = new ImageToImageClient(controller);
            _output = output;
        }

        [Fact]
        private async Task CheckIfServerIsOnline()
        {
            bool isOnline = await _client.CheckOnlineStatus();
            Assert.True(isOnline);
        }


        [Fact]
        private async Task SubmittingJobReturnsValidIJobReceipt()
        {

            string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
            var response = await _client.SubmitJob(payload);

            Assert.IsAssignableFrom<IJobReceipt>(response);
            Assert.NotNull(response.Id);
            Assert.NotEqual("", response.Id);
        }


        [Fact]
        private async Task SubmittingJobAddsToJobQueue()
        {
            string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
            var response = await _client.SubmitJob(payload);

            string currentId = response.Id;

            // var response = await controller.POST_Job(payload);

            _output.WriteLine($"Submitted JobId: {currentId}");
            _output.WriteLine("... getting queued jobs ...");

            var jobsJson = await _client.GetAllJobs();
            var jobs = JsonConvert.DeserializeObject<Dictionary<string, DeforumJob>>(jobsJson);

            bool containsId = false;
            foreach (KeyValuePair<string, DeforumJob> entry in jobs)
            {
                _output.WriteLine($"Checking {entry.Key}");
                if (entry.Key.Contains(currentId))
                {
                    _output.WriteLine("Id is in queue!");
                    containsId = true;
                    break;
                }
            }

            Assert.True(containsId);
        }

        [Fact]
        private async Task ClientCanCheckProgress()
        {

            string payload = File.ReadAllText(@"D:\Repos\csstablediffusiontest\assets\deforum_testSettings.txt");
            var receipt = await _client.SubmitJob(payload);
            string currentId = receipt.Id;

            double progress = await _client.GetProgress(receipt.Id);

            _output.WriteLine($"{currentId} progress: {progress}");
            Assert.True(progress >= 0d);

        }

        [Fact]
        private async Task ClientCanCancelJob()
        {
            _output.WriteLine("... Submitting test payload ...");
            var receipt = await _client.SubmitJob(_payload);
            _output.WriteLine($"Job Receipt: {receipt.Id}, {receipt.Status}");
            string currentId = receipt.Id;
            Thread.Sleep(1000);

            _output.WriteLine("... Canceling Job ...");
            var cancelReceipt = await _client.CancelJob(currentId);
            _output.WriteLine($"Job Receipt: {cancelReceipt.Ids}, {cancelReceipt.Status}");
            Assert.True(cancelReceipt.Status == Status.CANCELLED);
        }

        [Fact]
        private async Task ClientCanCancelAllJobs()
        {
            Assert.True(false);
        }

    }
}