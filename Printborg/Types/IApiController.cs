using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Types
{
    internal interface IApiController
    {
        Task<string> Get(string baseAddress, string endpoint, Action<string, double> ReportProgress, int timeout = 30);
    }
}
