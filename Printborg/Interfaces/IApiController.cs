using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.Interfaces
{
    public interface IApiController
    {
        //Task<string> Get(string baseAddress, string endpoint, Action<string, double> ReportProgress, int timeout = 30);

        Task<string> POST_Job(string payload);
        Task DELETE_Job(string id);
        Task GET_Job(string id);
        Task GET_allJobs();
        Task DELETE_allJobs();
        Task<string> GET_Batch(string id);

    }
}
