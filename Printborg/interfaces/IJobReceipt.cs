using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Interfaces {
    
    /// <summary>
    /// Simple object that informs user whether their submission was successful or not.
    /// </summary>
    public interface IJobReceipt {
        Status Status { get; set; }
        string Id { get; set; }
    }
}
