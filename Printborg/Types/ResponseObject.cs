using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types
{
    internal class ResponseObject
    {
        [JsonProperty("images")]
        public List<string> Images { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        public ResponseObject()
        {
        }
    }
}
