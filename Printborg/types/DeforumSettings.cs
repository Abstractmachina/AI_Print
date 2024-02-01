using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Printborg.Interfaces;
using Printborg.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types.Deforum {
    public class DeforumSettings {

        public RunSettings Run { get; set; }
        public KeyFramesSettings KeyFrames { get; set; }
        public GuidedImagesSettings GuidedImages { get; set; }
        public PromptSettings Prompt { get; set; }
        public StrengthSettings Strength { get; set; }
        public CfgSettings Cfg { get; set; }
        public SeedSettings Seed { get; set; }
        public MotionSettings Motion { get; set; }

        public DeforumSettings() {
            Run = new RunSettings();
            KeyFrames = new KeyFramesSettings();
            GuidedImages = new GuidedImagesSettings();
            Prompt = new PromptSettings();
            Strength = new StrengthSettings();
            Cfg = new CfgSettings();
            Seed = new SeedSettings();
            Motion = new MotionSettings();

        }
        public DeforumSettings(RunSettings run, KeyFramesSettings keyFrames, GuidedImagesSettings guidedImages, PromptSettings prompt, StrengthSettings strength, CfgSettings cfg, SeedSettings seed, MotionSettings motion) {
            Run = run;
            KeyFrames = keyFrames;
            GuidedImages = guidedImages;
            Prompt = prompt;
            Strength = strength;
            Cfg = cfg;
            Seed = seed;
            Motion = motion;
        }


        public string ToJson() {

            var run = JObject.Parse(JsonConvert.SerializeObject(Run));
            var keyframes = JObject.Parse(JsonConvert.SerializeObject(KeyFrames));
            var guidedImages = JObject.Parse(JsonConvert.SerializeObject(GuidedImages));
            var prompt = JObject.Parse(JsonConvert.SerializeObject(Prompt));
            var strength = JObject.Parse(JsonConvert.SerializeObject(Strength));
            var cfg = JObject.Parse(JsonConvert.SerializeObject(Cfg));
            var seed = JObject.Parse(JsonConvert.SerializeObject(Seed));
            var motion = JObject.Parse(JsonConvert.SerializeObject(Motion));

            var result = new JObject();

            result.Merge(run);
            result.Merge(keyframes);
            result.Merge(guidedImages);
            result.Merge(prompt);
            result.Merge(strength);
            result.Merge(cfg);
            result.Merge(seed);
            result.Merge(motion);

            // combine with rest of the settings
            string filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Config\\config_deforum_hiddenSettings.json"
            );
            //string configFile = Path.Combine(Environment.CurrentDirectory, "/Config/config_deforum_hiddenSettings.json");
            var hiddenSettings = File.ReadAllText(filePath);
            JObject defaultSettings = JObject.Parse(hiddenSettings);
            result.Merge(defaultSettings);

            return result.ToString();
        }

    }



    public class RunSettings : IDefaultable {
        [JsonProperty("sampler")]
        public string Sampler;
        [JsonProperty("steps")]
        public int Steps;
        [JsonProperty("W")]
        public int W;
        [JsonProperty("H")]
        public int H;
        [JsonProperty("seeds")]
        public int Seed;
        [JsonProperty("batch_name")]
        public string BatchName;

        /// <summary>
        /// Create an instance with default values
        /// </summary>
        public RunSettings() {
            AssignDefaultValues();
        }

        public RunSettings(string sampler, int steps, int w, int h, int seed, string batchname) {
            Sampler = sampler;
            Steps = steps;
            W = w;
            H = h;
            Seed = seed;
            BatchName = batchname;
        }

        public void AssignDefaultValues() {
            Sampler = "Euler a";
            Steps = 12;
            W = 256;
            H = 256;
            var rand = new Random();
            Seed = rand.Next();
            var currentDateTime = DateTime.Now.ToString("yymmdd_hhmmss");
            BatchName = $"Deforum_{currentDateTime}";
        }
    }

    public class KeyFramesSettings : IDefaultable {
        [JsonProperty("animation_mode")]
        public string AnimationMode;
        [JsonProperty("diffusion_cadence")]
        public int DiffusionCadence;
        [JsonProperty("max_frames")]
        public int MaxFrames;
        [JsonProperty("fps")]
        public int Fps;

        public KeyFramesSettings() {
            AssignDefaultValues();
        }

        public KeyFramesSettings(string animationMode, int diffusionCadence, int maxFrames, int fps) {
            AnimationMode = animationMode;
            DiffusionCadence = diffusionCadence;
            MaxFrames = maxFrames;
            Fps = fps;
        }

        public void AssignDefaultValues() {
            AnimationMode = "2D";
            DiffusionCadence = 2;
            MaxFrames = 10;
            Fps = 30;
        }
    }

    public class GuidedImagesSettings : IDefaultable {
        /// <summary>
        /// syntax: first frame - "0": "path", 
        /// then all following frames - 
        /// "max_f{int-int}" : "path" or 
        /// "max_f{int}" : "path"
        /// </summary>
        [JsonProperty("init_images")]
        public Dictionary<string, string> InitImages;
        // TODO find out correct structure
        [JsonProperty("image_strength_schedule")]
        public string ImageStrengthSchedule;
        [JsonProperty("blendFactorMax")]
        public string BlendFactorMax;
        [JsonProperty("blendFactorSlope")]
        public string BlendFactorSlope;
        [JsonProperty("tweening_frames_schedule")]
        public string TweeningFramesSchedule;


        public GuidedImagesSettings() {
            AssignDefaultValues();
        }

        public GuidedImagesSettings(Dictionary<string, string> initImages, string imageStrengthSchedule, string blendFactorMax, string blendFactorSlope, string tweeningFramesSchedule) {
            InitImages = initImages;
            ImageStrengthSchedule = imageStrengthSchedule;
            BlendFactorMax = blendFactorMax;
            BlendFactorSlope = blendFactorSlope;
            TweeningFramesSchedule = tweeningFramesSchedule;
        }

        public void AssignDefaultValues() {

            var initImages = new Dictionary<string, string> {
                { "0", "https://deforum.github.io/a1/Gi1.png" },
             { "max_f/4-5", "https://deforum.github.io/a1/Gi2.png"},
             {"max_f/2-10", "https://deforum.github.io/a1/Gi3.png"},
             {"3*max_f/4-15", "https://deforum.github.io/a1/Gi4.jpg" },
             {"max_f-20", "https://deforum.github.io/a1/Gi1.png" }
            };
            InitImages = initImages;
            ImageStrengthSchedule = "0:(0.75)";
            BlendFactorMax = "0:(0.35)";
            BlendFactorSlope = "0:(0.25)";
            TweeningFramesSchedule = "0:(20)";
        }
    }

    public class PromptSettings : IDefaultable {
        [JsonProperty("prompts")]
        public Dictionary<int, string> Prompts;
        [JsonProperty("positive_prompts")]
        public Dictionary<int, string> PositivePrompts;
        [JsonProperty("negative_prompts")]
        public Dictionary<int, string> NegativePrompt;

        public PromptSettings() {
            AssignDefaultValues();
        }
        public PromptSettings(Dictionary<int, string> prompts, Dictionary<int, string> positivePrompts,  Dictionary<int, string> negativePrompts) {
            Prompts = prompts;
            PositivePrompts = positivePrompts;
            NegativePrompt = negativePrompts;
        }

        public void AssignDefaultValues() {
            Prompts = new Dictionary<int, string> {
                { 0, "None None  tiny cute bunny, vibrant diffraction, highly detailed, intricate, ultra hd, sharp photo, crepuscular rays, in focus --neg nsfw, nude    " },
                { 30, "None None  anthropomorphic clean cat, surrounded by fractals, epic angle and pose, symmetrical, 3d, depth of field --neg nsfw, nude    " },
                { 60, "None None  a beautiful coconut --neg photo, realistic  nsfw, nude    " },
                { 90, "None None  a beautiful durian, award winning photography --neg nsfw, nude    " }
            };
            PositivePrompts = null;
            NegativePrompt = null;
        }
    }

    public class StrengthSettings : IDefaultable {
        [JsonProperty("strength_schedule")]
        public string StrengthSchedule;

        public StrengthSettings() {
            AssignDefaultValues();
        }
        public StrengthSettings(string strengthSchedule) {
            StrengthSchedule = strengthSchedule;
        }

        public void AssignDefaultValues() {
            StrengthSchedule = "0: (0.65)";
        }
    }

    public class CfgSettings : IDefaultable {
        [JsonProperty("cfg_scale_schedule")]
        public string CfgScaleSchedule;

        public CfgSettings() {
            AssignDefaultValues();
        }
        public CfgSettings(string cfgScaleSchedule) {
            CfgScaleSchedule = cfgScaleSchedule;
        }

        public void AssignDefaultValues() {
            CfgScaleSchedule = "0: (7)";
        }
    }

    public class SeedSettings : IDefaultable {
        [JsonProperty("seed_behavior")]
        public string SeedBehaviour;
        [JsonProperty("seed_schedule")]
        public string SeedSchedule;
        
        public SeedSettings() {
            AssignDefaultValues();
        }
        public SeedSettings(string seedBehaviour, string seedSchedule) {
            SeedBehaviour = seedBehaviour;
            SeedSchedule = seedSchedule;
        }

        public void AssignDefaultValues() {
            SeedBehaviour = "iter";
            SeedSchedule = "0:(s), 1:(-1), \"max_f-2\":(-1), \"max_f-1\":(s)";
        }
    }

    public class MotionSettings : IDefaultable {
        [JsonProperty("angle")]
        public string Angle;
        [JsonProperty("zoom")]
        public string Zoom;
        [JsonProperty("translation_x")]
        public string TranslationX;
        [JsonProperty("translation_y")]
        public string TranslationY;
        [JsonProperty("translation_z")]
        public string TranslationZ;
        [JsonProperty("transform_center_x")]
        public string TransformCenterX;
        [JsonProperty("transform_center_y")]
        public string TransformCenterY;
        [JsonProperty("rotation_3d_x")]
        public string Rotation3dX;
        [JsonProperty("rotation_3d_y")]
        public string Rotation3dY;
        [JsonProperty("rotation_3d_z")]
        public string Rotation3dZ;

        public MotionSettings() {
            AssignDefaultValues();
        }
        public MotionSettings(
            string angle,string zoom,
            string translationX, string translationY,string translationZ,
            string transformCenterX, string transformCenterY,
            string rotation3dX,string rotation3dY,string  rotation3dZ) {
            Angle = angle;
            Zoom = zoom;
            TranslationX = translationX;
            TranslationY = translationY;
            TranslationZ = translationZ;
            TransformCenterX = transformCenterX;
            TransformCenterY = transformCenterY;
            Rotation3dX = rotation3dX;
            Rotation3dY = rotation3dY;
            Rotation3dZ = rotation3dZ;
        }

        public void AssignDefaultValues() {
            Angle = "0: (0)";
            Zoom = "0: (1.0025+0.002*sin(1.25*3.14*t/30))";
            TranslationX = "0: (0)";
            TranslationY = "0: (0)";
            TranslationZ = "0: (1.75)";
            TransformCenterX = "0: (0.5)";
            TransformCenterY = "0: (0.5)";
            Rotation3dX = "0: (0)";
            Rotation3dY = "0: (0)";
            Rotation3dZ = "0: (0)"; 
        }
    }

}
