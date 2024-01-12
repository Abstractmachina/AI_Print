using Newtonsoft.Json;
using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Interfaces {
    public interface IJob {
        string Id { get; set; }
        Status Status { get; set; }
        string Phase { get; set; }
        string ErrorType { get; set; }
        double Progress { get; set; }
        double StartedAt { get; set; }
        double LastUpdated { get; set; }
        double ExecutionTime { get; set; }
        double UpdateIntervalTime { get; set; }
        int Updates { get; set; }
        string Message { get; set; }
        string Outdir { get; set; }
    }
}
