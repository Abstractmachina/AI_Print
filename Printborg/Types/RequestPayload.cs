using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types {
    /// <summary>
    /// contains the content sent to Stability.ai via POST request
    /// </summary>
    public class RequestPayload {
        public int Height { get; set; }
        public int Width { get; set; }
        public List<TextPrompt> Text_prompts { get; set; }
        // public float Cfg_scale {get;set;}
        // public string Clip_guidance_preset {get;set;}
        // public string Sampler {get;set;}
        // public int Samples {get;set;}
        // public int Seed {get;set;}
        // public int Steps {get;set;}
        // public string Style_presets {get;set;}

        public RequestPayload(List<TextPrompt> textPrompts, int height, int width) {
            Text_prompts = textPrompts;
            Height = height;
            Width = width;
        }

        public override string ToString() {
            string output = "ReqBody Object:\n";
            output += $"\tHeight: {Height}\n";
            output += $"\tWidth: {Width}\n";
            output += "\tText Prompts:\n";
            foreach (var tp in Text_prompts) {
                output += $"\t\tText: '{tp.Text}', Weight: {tp.Weight}\n";
            }
            return output;
        }
    }
}
