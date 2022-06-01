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
        const string ipNumber = "127.0.0.1";

        TcpClient clienSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
