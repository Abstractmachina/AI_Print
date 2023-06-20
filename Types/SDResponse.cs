using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Print {
    internal class SDResponse {
        public string? status { get; set; }
        public string? generationTime { get; set; }
        public List<string?>? output { get; set; }

    }
}
