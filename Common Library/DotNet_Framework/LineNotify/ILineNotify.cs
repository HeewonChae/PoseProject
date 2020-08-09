using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineNotify
{
    public interface ILineNotify
    {
        void Init(string host_url, string token);

        void SendMessageAsync(string msg);

        void SendMessage(string msg);
    }
}