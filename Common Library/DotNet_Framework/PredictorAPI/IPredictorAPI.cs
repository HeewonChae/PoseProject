using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI
{
    public interface IPredictorAPI
    {
        void Init(string host_url, string line_notify_host, string line_notify_token);
    }
}