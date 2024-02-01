using Newtonsoft.Json;
using Printborg.Types.Deforum;
using System.Reflection;
using Xunit.Abstractions;

namespace PrintBorgTests.UnitTests {

    public class DeforumSettingsTests {

        private readonly ITestOutputHelper _output;

        public DeforumSettingsTests(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void RunSettingsCanConvertToJSon() {
            var settings = new RunSettings("samplerTest", 10, 120, 120, 0, "batchNametest");

            var json = JsonConvert.SerializeObject(settings);
            Console.WriteLine(json);

            Assert.True(false);
        }

        [Fact]
        public void GuidedImagesSettingsConvertToJSonCorrectly() {

            var initImages = new Dictionary<string, string>();
            initImages.Add("0", "test0");
            initImages.Add("1", "test1");
            initImages.Add("2", "test2");
            var settings = new GuidedImagesSettings(initImages, "imageStrengthScheduleTest", "0:0", "0:0", "0:0");

            var json = JsonConvert.SerializeObject(settings);
            Console.WriteLine(json);
            Assert.True(false);

        }

        [Fact]
        public void PromptSettingsConvertToJSonCorrectly() {
            var prompts = new Dictionary<int, string>();
            prompts.Add(0, "test0");
            prompts.Add(1, "test1");
            prompts.Add(2, "test2");
            var settings = new PromptSettings(prompts, prompts, prompts);

            var json = JsonConvert.SerializeObject(settings);
            Console.WriteLine(json);
            Assert.True(false);

        }

        [Fact]
        private void OutputsToJsonCorrectly() {

            var settings = new DeforumSettings();
            _output.WriteLine(settings.ToJson());


            string filePath = Path.Combine(
               Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "\\\\deforum_config_standard.json"
           );

            _output.WriteLine(filePath);
            //string configFile = Path.Combine(Environment.CurrentDirectory, "/Config/config_deforum_hiddenSettings.json");
            var hiddenSettings = File.ReadAllText(filePath);



            Assert.True(false);

        }
    }
}
