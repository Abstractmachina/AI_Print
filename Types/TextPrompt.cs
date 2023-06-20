using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Print.Types {
    public class TextPrompt {
		[JsonProperty("text")]
        public string Text { get; set; }
		[JsonProperty("weight")]
        public float Weight { get; set; }

        public TextPrompt(string text, float weight) {
            Text = text;
            Weight = weight;
        }
    }

}
