using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MyWinformApp_Server
{
    interface IDataBase_Connection
    {
        void Open();
        void Close();
        DataSet GetDataSet(string sql);
    }
}
