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
        static string ipNumber = "127.0.0.1";
        
        static int userCount = 12;
        string data;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        TcpListener server = null;
        TcpClient clientSocket = null;

        public Main()
        {
            InitializeComponent();
            //Thread thread = new Thread();
            ThreadPool.QueueUserWorkItem(InitSocket);
            
        }


        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void InitSocket(object state)
        {
            server = new TcpListener(IPAddress.Any, 8000);
            clientSocket = default(TcpClient);
            server.Start();
            // DisplayText();

            while (true)
            {
                userCount++;
                clientSocket = server.AcceptTcpClient();
                // DIsplayText( Accept connection );

                NetworkStream stream = clientSocket.GetStream();
                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                user_name = user_name.Substring(0, user_name.IndexOf("$"));

                clientList.Add(clientSocket, user_name);

                // send msg to all

                handleClient h_client = new handleClient();
                
                
            }
        }


        private void onReceived(string message, string user_name)
        {
            if(message.Equals("/exit"))
            {
                string DisplayMessage = user_name + " leaves the chat.";
                DisplayText(DisplayMessage);

            }else
            {
                string DisplayMessage = user_name + " leaves the chat.";
                DisplayText(DisplayMessage);

            }    
        }

        private void DisplayText(string text)
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
    }
}
