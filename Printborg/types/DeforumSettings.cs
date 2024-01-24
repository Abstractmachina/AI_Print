using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types {
    public class DeforumSettings {

        public RunSettings Run { get; set; }
        public KeyFramesSettings KeyFrames { get; set; }
        public GuidedImagesSettings GuidedImages { get; set; }
        public PromptSettings Prompt { get; set; }
        public StrengthSettings Strength { get; set; }
        public CfgSettings Cfg { get; set; }
        public SeedSettings Seed { get; set; }
        public MotionSettings Motion { get; set; }

        public DeforumSettings() { }
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

    }

    public struct RunSettings {
        public string Sampler;
        public int Steps;
        public int W;
        public int H;
        public int Seed;
        public string BatchName;

        public RunSettings(string sampler, int steps, int w, int h, int seed, string batchname) {
            Sampler = sampler;
            Steps = steps;
            W = w;
            H = h;
            Seed = seed;
            BatchName = batchname;
    }
    }

    public struct KeyFramesSettings {
        public string AnimationMode;
        public int DiffusionCadence;
        public int MaxFrames;
        public int Fps;

        public KeyFramesSettings(string animationMode, int diffusionCadence, int maxFrames, int fps) {
            AnimationMode = animationMode;
            DiffusionCadence = diffusionCadence;
            MaxFrames = maxFrames;
            Fps = fps;
    }
    }

    public struct GuidedImagesSettings {
        /// <summary>
        /// syntax: first frame - "0": "path", 
        /// then all following frames - 
        /// "max_f{int-int}" : "path" or 
        /// "max_f{int}" : "path"
        /// </summary>
        public Dictionary<string, string> InitImages;
        // TODO find out correct structure
        public string ImageStrengthSchedule;
        public string BlendFactorMax;
        public string BlendFactorSlope;
        public string TweeningFramesSchedule;

        public GuidedImagesSettings(Dictionary<string, string> initImages, string imageStrengthSchedule, string blendFactorMax, string blendFactorSlope, string tweeningFramesSchedule) {
            InitImages = initImages;
            ImageStrengthSchedule = imageStrengthSchedule;
            BlendFactorMax = blendFactorMax;
            BlendFactorSlope = blendFactorSlope;
            TweeningFramesSchedule = tweeningFramesSchedule;
        }
    }

    public struct PromptSettings {
        public Dictionary<int, string> Prompts;
        public Dictionary<int, string> PositivePrompts;
        public Dictionary<int, string> NegativePrompt;
    }

    public struct StrengthSettings {
        public List<string> StrengthSchedule;
    }

    public struct  CfgSettings
    {
        public List<string> CfgScaleSchedule;
    }

    public struct SeedSettings {
        public string SeedBehaviour;
        public string SeedSchedule;
    }

    public struct MotionSettings {
        public string Angle;      
        public string Zoom;
        public string TranslationX;
        public string TranslationY;
        public string TranslationZ;
        public string TransformCenterX;
        public string TransformCenterY;
        public string Rotation3dX;
        public string Rotation3dY;
        public string Rotation3dZ;

    }

}
