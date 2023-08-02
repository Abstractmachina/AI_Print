using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Json {
	internal class CustomContractResolver : DefaultContractResolver {
		protected override string ResolvePropertyName(string propertyName) {
			return propertyName.ToLower();
		}

	}

	internal class T2IJsonConverter : JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var prompt = (TextToImagePrompt)value;
			var jsonObject = new JObject();
			var contractResolver = serializer.ContractResolver as DefaultContractResolver;

			// Serialize properties
			foreach (var property in value.GetType().GetProperties()) {
				var propertyName = contractResolver != null ? contractResolver.GetResolvedPropertyName(property.Name) : property.Name;
				var propertyValue = property.GetValue(value);
				if (propertyValue == null) {
					jsonObject.Add(propertyName.ToLower(), JValue.CreateNull());
				}
				else if (propertyValue is bool) {
					jsonObject.Add(propertyName.ToLower(),
					((bool)propertyValue == true) ? "yes" : "no");

				}
				else {
					jsonObject.Add(propertyName.ToLower(), JToken.FromObject(propertyValue));
				}
			}
			jsonObject.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			throw new NotImplementedException();
		}

		public override bool CanRead => false;

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(TextToImagePrompt);
		}
	}
}
