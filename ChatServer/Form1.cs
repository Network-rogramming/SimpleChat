using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        int port;
        IPAddress addr;
        IPEndPoint ep;

        Socket listener;
        Thread workThread;
        ManualResetEvent serverEvent;

        byte[] buff1 = new byte[2048];
        byte[] buff2 = new byte[2048];

        string clientMessage = String.Empty;
        string serverMessage = String.Empty;

        int receiveBytes = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                serverEvent = new ManualResetEvent(false);

                addr = IPAddress.Parse(ipField.Text);
                port = int.Parse(portField.Text);
                ep = new IPEndPoint(addr, port);

                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                listener.Bind(ep);
                listener.Listen(100);

                workThread = new Thread(new ThreadStart(ThreadMethod));
                workThread.IsBackground = true;
                workThread.Start();

                startButton.Enabled = false;
                statucLabel.ForeColor = Color.Green;
                statucLabel.Text = "Состояние сервера: включен";
                chatField.Text += DateTime.Now.ToString() + " => Сервер успешно запущен \r\n";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка потока", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        // 1
        void ThreadMethod()
        {
            try
            {
                while (true)
                {
                    AsyncCallback delegate1 = new AsyncCallback(AcceptCallback);
                    listener.BeginAccept(delegate1, listener);
                    serverEvent.WaitOne();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка потока", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2
        public void AcceptCallback(IAsyncResult state)
        {

        }
    }
}
