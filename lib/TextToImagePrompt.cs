using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Print {
	internal class TextToImagePrompt {
		public string Key { get; set; }
		public string Prompt { get; set; }
		public string? Negative_prompt { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public int Num_inference_steps { get; set; }
		public bool Safety_checker { get; set; }
		public bool Enhance_prompt { get; set; }
		public int? Seed { get; set; }
		public float Guidance_scale { get; set; }
		public bool Multi_lingual { get; set; }
		public bool Panorama { get; set; }
		public bool Self_attention { get; set; }
		public bool Upscale { get; set; }
		public string Embedding_model { get; set; }
		public string? Webhook { get; set; }
		public string? Track_id { get; set; }

		public TextToImagePrompt(string key) : this(key, "a round, red apple") { }

		public TextToImagePrompt(string key, string prompt) : this(key, prompt, null, 512, 512) { }

		public TextToImagePrompt(string key, string prompt, string? negative_prompt, int width, int height) : this(key, prompt, negative_prompt, width, height, 1, 20) { }

		public TextToImagePrompt(
			string key, string prompt, string negative_prompt, int width, int height, int samples, int num_inference_steps
		) : this(
			key, prompt, negative_prompt, width, height, samples, num_inference_steps, false, true, null, 7.5f, false, false, false, false, "embeddings_model_id", null, null
		) {
		}

		public TextToImagePrompt(string key, string prompt, string negative_prompt, int width, int height, int samples, int num_inference_steps, bool safety_checker, bool enhance_prompt, int? seed, float guidance_scale, bool multi_lingual, bool panorama, bool self_attention, bool upscale, string embeddings_model, string? webhook, string? track_id) {
			Key = key;
			Prompt = prompt;
			Negative_prompt = negative_prompt;
			Width = width;
			Height = height;
			Samples = samples;
			Num_inference_steps = num_inference_steps;
			Safety_checker = safety_checker;
			Enhance_prompt = enhance_prompt;
			Seed = seed;
			Guidance_scale = guidance_scale;
			Multi_lingual = multi_lingual;
			Panorama = panorama;
			Self_attention = self_attention;
			Upscale = upscale;
			Embedding_model = embeddings_model;
			Webhook = webhook;
			Track_id = track_id;
		}
	}

}
