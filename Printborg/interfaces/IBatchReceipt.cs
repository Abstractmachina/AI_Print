using Printborg.Types.Deforum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.interfaces {
    public interface IBatchReceipt { 
        Status Status { get; set; }
        List< string> Ids { get; set; }
    }
}
