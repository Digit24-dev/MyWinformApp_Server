using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

/// <summary>
/// To do List.
/// - 서버 종료되면 해당 날짜와 채팅의 로그를 데이터로 남기기. (데이터 형식은 미정.)
/// - 귓속말
/// - 파일 전송
/// - 
/// </summary>


namespace MyWinformApp_Server
{
    public partial class Main : Form
    {
        const int portNumber = 8000;
        
        private int userCount = 0;
        private string date;

        private object threadLock = new object();

        Queue<string> userToRemove = new Queue<string>();

        bool onReceiveFlag_Exit = false;
        bool onReceiveFlag_Join = false;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        TcpListener server = null;
        TcpClient clientSocket = null;

        connectDB db;

        public Main()
        {
            InitializeComponent();

            Thread thread = new Thread(InitSocket);
            thread.IsBackground = true;
            thread.Start();

            Thread thread_UIController = new Thread(onReceived_UIController);
            thread_UIController.IsBackground = true;
            thread_UIController.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            db = new connectDB();
            db.Open();
        }

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

        /// <summary>
        /// 새로운 스레드 분기를 시켜 사용자 목록에서 제거할 대상을 큐에 삽입하고 해당 큐에 값이 있으면 UI 컨트롤을 하는 메소드.
        /// 서버에 스레드 하나가 추가 필요.
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
                else if(onReceiveFlag_Join)
                {
                    if(this.InvokeRequired)
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
            if(message.Equals("/exit"))
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
            }else
            {
                string DisplayMessage = "[From client]" + user_name + " : " + message;
                displayText(DisplayMessage);
                sendMessagetoAll(message, user_name, true);
            }
        }

        private void sendMessagetoAll(string message, string user_name, bool flag)
        {
            foreach(var pair in clientList)
            {
                date = DateTime.Now.ToString("yyyy.MM.dd. HH.mm.ss");

                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();
                byte[] buffer = null;

                if (flag)
                {
                    if (message.Equals("/exit"))
                    {
                        buffer = Encoding.Unicode.GetBytes(user_name + " leaves the chat.");
                    }
                    else
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " : " + message);
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

        // Method Overload -> 메소드 오버로드로 하지말고 새로운 메소드로 선언
        // 새로운 유저가 입장할 시에 모든 유저에게 유저 리스트를 전송한다.
        // 컨트롤 내용을 어떻게 전달할지가 중요하다.
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

        public void displayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                }));
            }else
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
            String temp = db.GetDataSet("select * from logs;");
            richTextBox1.AppendText("from Database! >> " + temp);
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            server.Stop();
            this.Close();
        }

        private void TextBox_Status_KeyUp(object sender, KeyEventArgs e)
        {
            sendMessagetoAll("ping", "", true);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.Close();
        }
    }
}
