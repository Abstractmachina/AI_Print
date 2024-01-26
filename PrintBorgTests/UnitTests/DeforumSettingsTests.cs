using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintBorgTests.UnitTests {
    public class DeforumSettingsTests {


        [Fact]
        public static void RunSettingsCanConvertToJSon() {
            var settings = new RunSettings("samplerTest", 10, 120, 120, 0, "batchNametest");

            var json = JsonConvert.SerializeObject(settings);
            Console.WriteLine(json);

            Assert.True(false);
        }

        [Fact]
        public static void GuidedImagesSettingsConvertToJSonCorrectly() {

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
        public static void PromptSettingsConvertToJSonCorrectly() {
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
        public static void CanMergeJsonObjectsCorrectly() {
            var settings1 = new RunSettings("samplerTest", 10, 120, 120, 0, "batchNametest");
            var initImages = new Dictionary<string, string>();
            initImages.Add("0", "test0");
            initImages.Add("1", "test1");
            initImages.Add("2", "test2");
            var settings2 = new GuidedImagesSettings(initImages, "imageStrengthScheduleTest", "0:0", "0:0", "0:0");

            var jObject1 = JObject.Parse(JsonConvert.SerializeObject(settings1));
            var jObject2 = JObject.Parse(JsonConvert.SerializeObject(settings2));

            var result = new JObject();

            result.Merge(jObject1);
            result.Merge(jObject2);

            Console.WriteLine(result);
            Assert.True(false);

        }
    }
}
