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

        const string dbName = "logs";
        const string dbTable = "chat";

        private int userCount = 0;
        private string date;

        private object threadLock = new object();

        Queue<string> userToRemove = new Queue<string>();

        bool onReceiveFlag_Exit = false;
        bool onReceiveFlag_Join = false;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        TcpListener server = null;
        TcpClient clientSocket = null;

        ConnectDB db;

        DateTime time = DateTime.Today;
        
        #endregion

        public class JSON_Data
        {
            public string time { get; set; }
            public string user { get; set; }
            public string message { get; set; }
        }
        
        private string JsonParser(string date, string user, string message)
        {
            var serializedData = new JSON_Data
            {
                time = date,
                user = user,
                message = message
            };

            return JsonSerializer.Serialize(serializedData);

            //displayText(jsonData); // 동작 완료.
        }

        private void JsonDeparser(string jsonData)
        {
            var deSerializedData = new JSON_Data { };
            
            deSerializedData = JsonSerializer.Deserialize<JSON_Data>(jsonData);
        }

        public Main()
        {
            InitializeComponent();
            
            Thread thread = new Thread(InitSocket);
            thread.IsBackground = true;
            thread.Start();

            Thread thread_UIController = new Thread(onReceived_UIController);
            thread_UIController.IsBackground = true;
            thread_UIController.Start();

            Thread timerThread = new Thread(Timer);
            timerThread.IsBackground = true;
            timerThread.Start();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            db = new ConnectDB();
            db.Open();
        }

        #region TimerThread
        private void Timer()
        {
            // Save logs when this thread invoked.
            //db.SetData("insert into " + dbTable + "values()");
            
            // 데이터 나누는 것이 우선인 듯.
            Thread.Sleep(1000);
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
            clientSocket = default(TcpClient);
            server.Start();

            displayText(" >> Server Started.");

            while (true)
            {
                try
                {
                    clientSocket = server.AcceptTcpClient();
                    displayText(">> Connection Accepted");

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
                    sendListOfUsers(user_name);

                    handleClient h_client = new handleClient();
                    h_client.OnReceived += new handleClient.MessageDisplayHandler(onReceived);
                    h_client.OnDisconnected += new handleClient.DisconnectedHandler(OnDisconnected);
                    h_client.startClient(clientSocket, clientList);

                }
                catch (SocketException es)
                {
                    MessageBox.Show("es!");
                    break;
                }
                catch (Exception ex)
                {
                    displayText(ex.Message);
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
        private void onReceived_UIController()
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

        private void onReceived(string message, string user_name)
        {
            // 스레드 프로세스가 너무 길음. 컨트롤 내부로 Invoke 필요.
            if (message.Equals("/exit"))
            {
                string DisplayMessage = user_name + " leaves the chat.";

                // Thread Synchronization
                lock (threadLock)
                {
                    userCount = userCount - 1;
                    userToRemove.Enqueue(user_name);
                    onReceiveFlag_Exit = true;
                }

                // Async UserList Update.
                displayText(DisplayMessage); // 이 구문이 스레드 성능에 영향을 주는 것이 아닌지 검토 필요.
                sendMessagetoAll(DisplayMessage, user_name, true);
            }
            else
            {
                date = DateTime.Now.ToString("MM월dd일 HH:mm:ss");
                string DisplayMessage = "[" + date + "]" + user_name + " : " + message;
                displayText(DisplayMessage);
                sendMessagetoAll(message, user_name, true);
            }
        }

        private void sendMessagetoAll(string message, string user_name, bool flag)
        {
            foreach (var pair in clientList)
            {
                date = DateTime.Now.ToString("MM월dd일 HH:mm:ss");

                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();
                byte[] buffer = null;

                if (flag)
                {
                    if (message.Equals("/exit"))
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " leaves the chat.");
                    }
                    else
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " : " + message);
                        displayText(JsonParser(date, user_name, message));
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
        private void sendListOfUsers(string user_name)
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
                displayText(userList);
            }
        }
        #endregion

        #region WinformControl
        public void displayText(string text)
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
            String temp = db.GetDataSet("select * from " + dbName + ";");
            richTextBox1.AppendText("from Database! >> " + temp + Environment.NewLine);
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            // 데이터 베이스에 저장.
            db.Close();
            server.Stop();
            this.Close();
        }

        private void TextBox_Status_KeyUp(object sender, KeyEventArgs e)
        {
            sendMessagetoAll("ping", "", true);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (db.IsOpen())
                {
                    db.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("DB 가 제대로 닫히지 않았습니다.");
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            // JsonDeparser(); // test
        }
    }
}
