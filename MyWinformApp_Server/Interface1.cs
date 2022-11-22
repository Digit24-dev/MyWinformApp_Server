using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace MyWinformApp_Server
{
    interface IHTTP_Server
    {
        List<string> BindingAddressList { get; set; }
        HttpListener httpListener { get; set; }
        string rootPath { get; set; }

        Thread Thread { get; set; }

        List<Thread> responseThreadList { get; set; }
        bool isRunning { get; set; }
        bool isDisposed { get; set; }
        int BUFFER_SIZE { get; set; }

    }
}
