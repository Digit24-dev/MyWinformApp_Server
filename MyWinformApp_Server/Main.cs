using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyWinformApp_Server
{
    public partial class Main : Form
    {
        #region ClassVariables
        const int portNumber = 8000;

        const string tableName = DAO.tableName;
        //const string dbTable = "chat";

        private int userCount = 0;
        private string date;

        private string bigSerializedJSON_Chatlogs = "";
        private string temporaryForked_Chatlogs;

        private readonly object threadLock = new object();
        private Queue<string> userToRemove = new Queue<string>();

        bool onReceiveFlag_Exit = false;
        bool onReceiveFlag_Join = false;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        TcpListener server = null;
        TcpClient clientSocket = null;

        DAO userDAO;

        HttpConnect httpConnect;
        #endregion

        public class JSON_Data
        {
            public string Time { get; set; }
            public string User { get; set; }
            public string Message { get; set; }
        }
        /*
         *         채팅 들어올 때, JSON으로 저장 -> DB에 넣을 때 Flush
         */

        private string JsonParser(string date, string user, string message)
        {
            var serializedData = new JSON_Data
            {
                Time = date,
                User = user,
                Message = message
            };

            return JsonSerializer.Serialize(serializedData);

            //displayText(jsonData); // 동작 완료.
        }

        private JSON_Data JsonDeparser(string jsonData)
        {
            JSON_Data deSerializedData;
            
            deSerializedData = JsonSerializer.Deserialize<JSON_Data>(jsonData);

            return deSerializedData;
        }

        public Main()
        {
            InitializeComponent();

            // Scoket Thread
            Thread thread = new Thread(InitSocket)
            {
                IsBackground = true
            };
            thread.Start();

            // Controller Thread
            Thread thread_UIController = new Thread(OnReceived_UIController)
            {
                IsBackground = true
            };
            thread_UIController.Start();

/*            // HTTP Thread
            httpConnect = new HttpConnect();
            //httpConnect.ServerInit();
            Thread httpThread = new Thread(httpConnect.ServerInit)
            {
                IsBackground = true
            };
            httpThread.Start();*/

            /*// Timer Thread
            // 스레드에 파라미터를 전달하는 방법에 대한 연구 필요.
            WorkClass wc = new WorkClass(temporaryForked_Chatlogs, new WorkClassCallBack(TimerISR));
            Thread timer = new Thread(new ThreadStart(wc.ThreadProc))
            {
                IsBackground = true
            };
            timer.Start();*/
        }

        private void Main_Load(object sender, EventArgs e)
        {
            userDAO = new DAO();
            userDAO.Open();
        }

        #region TimerRegion
        // 타이머가 동작하면 temporaryForked_Chatlogs를 넘겨야 함.
        private void TimerISR(object parameter)
        {
            temporaryForked_Chatlogs = bigSerializedJSON_Chatlogs;

            // JSON은 XML 노드 트리 구조 + Hash 의 느낌이 든다. JSON 구조에 대해서 더 공부하고 DB에 저장하는 방법을 고려해보자.

            JSON_Data temporaryDeSerJSON_Bucket;
            
            temporaryDeSerJSON_Bucket = JsonDeparser(temporaryForked_Chatlogs);
            var indexJSON_Bucket = temporaryDeSerJSON_Bucket.Time;

            
            foreach (var item in indexJSON_Bucket)
            {
                userDAO.SetData("insert into " + tableName + "values(" + temporaryDeSerJSON_Bucket.Time + temporaryDeSerJSON_Bucket.User + temporaryDeSerJSON_Bucket.Message + ")");
            }
            
            bigSerializedJSON_Chatlogs = "";
        }
        #endregion

        #region NetworkConnection

        /// <summary>
        /// Main Socket Thread
        /// 
        /// </summary>
        private void InitSocket()
        {
            server = new TcpListener(IPAddress.Any, portNumber);
            clientSocket = default;
            server.Start();

            DisplayText(" >> Server Started.");

            while (true)
            {
                try
                {
                    clientSocket = server.AcceptTcpClient();
                    DisplayText(">> Connection Accepted");

                    NetworkStream stream = clientSocket.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$"));
                    clientList.Add(clientSocket, user_name);

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            userCount++;
                            textBox_UserCount.Text = userCount.ToString();
                            textBox_UserCount.Show();
                            ListBox_Users.Items.Add(user_name);
                        }));
                    }

                    // Async UserList Update.
                    onReceiveFlag_Join = true;
                    SendListOfUsers(user_name);
                    
                    HandleClient h_client = new HandleClient();
                    h_client.OnReceived += new HandleClient.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new HandleClient.DisconnectedHandler(OnDisconnected);
                    h_client.StartClient(clientSocket, clientList);

                }
                catch (SocketException es)
                {
                    MessageBox.Show(es.Message);
                    break;
                }
                catch (Exception ex)
                {
                    DisplayText(ex.Message);
                    MessageBox.Show("ex!");
                    break;
                }
            }
            clientSocket.Close();
            server.Stop();
        }
        private void OnDisconnected(TcpClient clientSocket)
        {
            if (clientList.ContainsKey(clientSocket))
                clientList.Remove(clientSocket);
        }
        #endregion

        #region OnConnectionMethods
        /// <summary>
        /// 새로운 스레드 분기를 시켜 사용자 목록에서 제거할 대상을 큐에 삽입하고 해당 큐에 값이 있으면 UI 컨트롤을 하는 메소드.
        /// 그 밖에 UI 컨트롤을 전체적으로 담당하는 역할.
        /// </summary>
        /// <param name="user_name"></param>
        private void OnReceived_UIController()
        {
            while (true)
            {
                if (onReceiveFlag_Exit)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            lock (threadLock)
                            {
                                textBox_UserCount.Text = userCount.ToString();
                                textBox_UserCount.Show();

                                string tempValue = userToRemove.Dequeue();
                                ListBox_Users.Items.Remove(tempValue);
                                onReceiveFlag_Exit = false;
                            }
                        }));
                    }
                }
                else if (onReceiveFlag_Join)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            lock (threadLock)
                            {
                                onReceiveFlag_Join = false;
                            }
                        }));
                    }
                }
            }
        }

        private void OnReceived(string message, string user_name)
        {
            // 스레드 프로세스가 너무 길음. 컨트롤 내부로 Invoke 필요.
            if (message.Equals("/exit"))
            {
                string DisplayMessage = user_name + " leaves the chat.";

                // Thread Synchronization
                lock (threadLock)
                {
                    userCount--;
                    userToRemove.Enqueue(user_name);
                    onReceiveFlag_Exit = true;
                }

                // Async UserList Update.
                DisplayText(DisplayMessage); // 이 구문이 스레드 성능에 영향을 주는 것이 아닌지 검토 필요.
                SendMessageToAll(DisplayMessage, user_name, true);
            }
            else
            {
                date = DateTime.Now.ToString("MM월dd일 HH:mm:ss");
                string DisplayMessage = "[" + date + "]" + user_name + " : " + message;
                // 분리 방법 : JSON 형태로 데이터를 분리 -> 분리한 데이터를 직렬화하여 String 타입으로 저장하다가 Timer마다 DB에 저장하고 Flush
                bigSerializedJSON_Chatlogs += JsonParser(date, user_name, message);
                DisplayText(DisplayMessage);
                SendMessageToAll(message, user_name, true);
            }
        }

        private void SendMessageToAll(string message, string user_name, bool flag)
        {
            foreach (var pair in clientList)
            {
                date = DateTime.Now.ToString("MM월dd일 HH:mm:ss");

                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();
                DAO userDAO = new DAO();

                byte[] buffer;

                if (flag)
                {
                    if (message.Equals("/exit"))
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " leaves the chat.");
                        
                        //this.userDAO.SetData("insert into " + tableName + "values(" + date + user_name + message + ")");
                    }
                    else
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " : " + message);
                        userDAO.DataParser(date, user_name, message);
                        //db.SetData("insert into " + dbName + "values(" + time + user_name + message + ")");
                    }
                }
                else
                {
                    buffer = Encoding.Unicode.GetBytes(message);
                }

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        /// <summary>
        /// 모든 유저에게 현재 접속 중인 유저 리스트를 문자열로 전송하는 메소드입니다. 문자열은 '$'로 구분되어 전송됩니다.
        /// </summary>
        /// <param name="user_name"></param>
        private void SendListOfUsers(string user_name)
        {
            string userList = "";

            foreach (var pair in clientList)
            {
                if (pair.Value.Equals(""))
                    break;
                userList += pair.Value + "$";
            }
            foreach (var pair in clientList)
            {
                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();

                byte[] buffer = Encoding.Unicode.GetBytes(userList);

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                DisplayText(userList);
            }
        }
        #endregion

        #region WinformControl
        public void DisplayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
            {
                richTextBox1.AppendText(text + Environment.NewLine);
            }
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            textBox_UserCount.Text = userCount.ToString();
            textBox_UserCount.Show();
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            String temp = userDAO.GetDataSet("select * from " + tableName + ";");
            richTextBox1.AppendText("from Database! >> " + temp + Environment.NewLine);
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            // 데이터 베이스에 저장.
            userDAO.Close();
            server.Stop();
            this.Close();
        }

        private void TextBox_Status_KeyUp(object sender, KeyEventArgs e)
        {
            SendMessageToAll("ping", "", true);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (userDAO.IsOpen())
                {
                    userDAO.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("DB 가 제대로 닫히지 않았습니다.");
            }
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            // JsonDeparser(); // test
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
    }
}
