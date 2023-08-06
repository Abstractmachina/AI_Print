using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types
{
    public class ResponseObject
    {
        [JsonProperty("images")]
        public List<string> Images { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        public ResponseObject()
        {
        }

        public override string ToString()
        {
            string output = "ResponseObject: {\n";
            output += "\timages:{\n";
            if (Images == null || Images.Count == 0) 
            { 
                output += "\t\tno images"; 
            }
            else
            {
                foreach (var i in Images)
                {
                    if (i.Length > 20)
                    {
                        output += String.Format("\t\t{0}[...],", i.Substring(0, 20));
                    }
                    else
                    {
                        output += String.Format("\t\t{0},", i);
                    }

                }
            }
            output += "\t}\n"; // close images
            //output += String.Format("\tinfo: {\n\t\t{0}\n\t}", Info); // closed info
            output += "}"; // close ResponseObject
            return output;
        }
    }
}
