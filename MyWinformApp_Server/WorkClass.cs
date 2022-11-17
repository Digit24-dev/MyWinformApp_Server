using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinformApp_Server
{
    public delegate void WorkClassCallBack(string dummy);

    class WorkClass
    {
        private string Dummy;
        private WorkClassCallBack callBack;

        public WorkClass(string dummy, WorkClassCallBack callBack)
        {
            this.Dummy = dummy;
            this.callBack = callBack;
        }

        public void ThreadProc()
        {
            
            callBack?.Invoke(Dummy);
        }
    }
}
