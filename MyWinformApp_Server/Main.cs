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


namespace MyWinformApp_Server
{
    public partial class Main : Form
    {
        string message = string.Empty;

        const int portNumber = 9000;
        
        static int userCount = 0;
        string date;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        TcpListener server = null;
        TcpClient clientSocket = null;

        public Main()
        {
            InitializeComponent();
            Thread thread = new Thread(InitSocket);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        private void InitSocket()
        {
            server = new TcpListener(IPAddress.Any, 8000);
            clientSocket = default(TcpClient);
            server.Start();
            
            displayText(" >> Server Started.");
            
            while (true)
            {
                try
                {
                    userCount++;
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
                            ListBox_Users.Items.Add(user_name);
                        }));
                    }

                    sendMessagetoAll(user_name);

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

        private void onReceived(string message, string user_name)
        {
            if(message.Equals("/exit"))
            {
                string DisplayMessage = user_name + " leaves the chat.";
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        ListBox_Users.Items.Remove(user_name);
                    }));
                }
                displayText(DisplayMessage);
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

        // Method Overload - new member
        private void sendMessagetoAll(string user_name)
        {
            string userList = "";

            foreach (var pair in clientList)
            {
                userList += pair.Value + "$";
            }
            foreach (var pair in clientList)
            {
                date = DateTime.Now.ToString("yyyy.MM.dd. HH.mm.ss");

                TcpClient client = pair.Key as TcpClient;
                NetworkStream stream = client.GetStream();

                byte[] buffer = Encoding.Unicode.GetBytes(userList);

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        private void sendUserList()
        {

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
            
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            
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
    }
}
