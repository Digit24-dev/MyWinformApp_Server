using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace MyWinformApp_Server
{
    class handleClient
    {
        TcpClient clientSocket; int clientNo; public void startClient(TcpClient ClientSocket, int clientNo) { this.clientSocket = ClientSocket; this.clientNo = clientNo; Thread t_hanlder = new Thread(doChat); t_hanlder.IsBackground = true; t_hanlder.Start(); }
        public delegate void MessageDisplayHandler(string text); public event MessageDisplayHandler OnReceived; public delegate void CalculateClientCounter(); public event CalculateClientCounter OnCalculated; private void doChat() { NetworkStream stream = null; try { byte[] buffer = new byte[1024]; string msg = string.Empty; int bytes = 0; int MessageCount = 0; while (true) { MessageCount++; stream = clientSocket.GetStream(); bytes = stream.Read(buffer, 0, buffer.Length); msg = Encoding.Unicode.GetString(buffer, 0, bytes); msg = msg.Substring(0, msg.IndexOf("$")); msg = "Data Received : " + msg; if (OnReceived != null) OnReceived(msg); msg = "Server to client(" + clientNo.ToString() + ") " + MessageCount.ToString(); if (OnReceived != null) OnReceived(msg); byte[] sbuffer = Encoding.Unicode.GetBytes(msg); stream.Write(sbuffer, 0, sbuffer.Length); stream.Flush(); msg = " >> " + msg; if (OnReceived != null) { OnReceived(msg); OnReceived(""); } } } catch (SocketException se) { Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message)); if (clientSocket != null) { clientSocket.Close(); stream.Close(); } if (OnCalculated != null) OnCalculated(); } catch (Exception ex) { Trace.WriteLine(string.Format("doChat - Exception : {0}", ex.Message)); if (clientSocket != null) { clientSocket.Close(); stream.Close(); } if (OnCalculated != null) OnCalculated(); } }
    }
}
