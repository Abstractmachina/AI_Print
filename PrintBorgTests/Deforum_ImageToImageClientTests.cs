using Printborg.Controllers;
using Printborg.Services;

namespace PrintBorgTests {

    [Collection("Sequential")]

    public class Deforum_ImageToImageClientTests {

        private readonly ImageToImageClient _client;

        public Deforum_ImageToImageClientTests() {
            var controller = new DeforumController();
            _client = new ImageToImageClient(controller);
        }

        [Fact]
        public async Task CheckIfServerIsOnline() {
            bool isOnline = await _client.CheckOnlineStatus();
            Assert.True(isOnline);
        }


    }
}