using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MyWinformApp_Server
{
    class DAO
    {
        string dbName = "chatlog";
        private void DataParser(string time, string user, string message)
        {
            ConnectDB conn = new ConnectDB();
            
            if (conn.IsOpen())
            {
                conn.SetData("insert into " + dbName + "values(" + time + user + message + ")");
            }
        }
        private string JsonParser(string time, string user, string message)
        {
            var serializedData = new JsonChat
            {
                Time = time,
                User = user,
                Message = message
            };

            string data = JsonSerializer.Serialize(serializedData);

            return data;
        }
    }

    public class JsonChat 
    { 
        public string Time { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
    }

}
