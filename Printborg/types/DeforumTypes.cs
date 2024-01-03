using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types.Deforum {
    public struct JobResponse {
        public Status Status;
        public string BatchId;
        public List<string> JobIds;

        public override string ToString() {
            string output = $"{{ \nStatus: ({Status.ToString()})\nBatch Id: ({BatchId}) \n" +
            "Job IDs: ({ \n";
            foreach (var i in JobIds) {
                output += $"\t{i.ToString()}\n";
            }
            output += "}) }";
            return output;
        }
    }


    public enum Status {
        ACCEPTED,
        SUCCESS,
        FAILURE
    }
}
