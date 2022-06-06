﻿using System;
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
        
        static int userCount = 12;
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
                    displayText("Connection Accepted");

                    NetworkStream stream = clientSocket.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$"));

                    clientList.Add(clientSocket, user_name);

                    sendMessagetoAll(user_name + " has entered the chat.", "", false);

                    handleClient h_client = new handleClient();
                    h_client.OnReceived += new handleClient.MessageDisplayHandler(onReceived);
                    h_client.OnDisconnected += new handleClient.DisconnectedHandler(OnDisconnected);
                }
                catch (SocketException es)
                {
                    break;
                }
                catch (Exception ex)
                {
                    break;
                }

                clientSocket.Close();
                server.Stop();
            }
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
                displayText(DisplayMessage);

            }else
            {
                string DisplayMessage = user_name + " leaves the chat.";
                displayText(DisplayMessage);

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
                    if (message.Equals("/exit "))
                    {
                        buffer = Encoding.Unicode.GetBytes(user_name + " leaves the chat.");
                    }
                    else
                    {
                        buffer = Encoding.Unicode.GetBytes("[" + date + "]" + user_name + " : " + message);
                    }
                }
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        private void displayText(string text)
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
    }
}
