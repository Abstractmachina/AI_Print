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
        public async Task CheckIfServerIsOnline() {
            bool isOnline = await _client.CheckOnlineStatus();
            Assert.True(isOnline);
        }


    }
}