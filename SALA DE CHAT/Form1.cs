using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net.Sockets;
using System.IO;
using Transitions;

namespace SALA_DE_CHAT
{
    public partial class Form1 : Form
    {
        static private NetworkStream stream;
        static private StreamWriter streamW;
        static private StreamReader streamR;
        static private TcpClient client = new TcpClient();
        static private string David = "unknown";

        private delegate void DaddItem(string s);

        private void AddItem(string s)
        {
            listBox1.Items.Add(s);
        }

        public Form1()
        {
            InitializeComponent();
        }

        void Listen()
        {
            while (client.Connected)
            {
                try
                {
                    this.Invoke(new DaddItem(AddItem), streamR.ReadLine());
                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        void Conectar()
        {
            try
            {
                client.Connect("127.0.0.1", 8000);
                if (client.Connected)
                {
                    Thread t = new Thread(Listen);

                    stream = client.GetStream();
                    streamW = new StreamWriter(stream);
                    streamR = new StreamReader(stream);

                    streamW.WriteLine(David);
                    streamW.Flush();

                    t.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no Disponible");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Servidor no Disponible");
                Application.Exit();
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            David = txtUsuario.Text;
            Conectar();

            Transition t = new Transition(new TransitionType_EaseInEaseOut(900));
            t.add(lblTitulo, "Left", 555);
            t.add(txtUsuario, "Left", 555);
            t.add(btnConectar, "Left", 555);
            t.add(listBox1, "Left", 26);
            t.add(txtMensaje, "Left", 26);
            t.add(btnEnviar, "Left", 283);
            t.add(lblUsurio, "Left", 555);
            t.run();

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            streamW.WriteLine(txtMensaje.Text);
            streamW.Flush();
            txtMensaje.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnviar.Location = new Point(-239, 250);
            txtMensaje.Location = new Point(-239, 250);
            listBox1.Location = new Point(-329, 23);
        }

        private void lblUsurio_Click(object sender, EventArgs e)
        {

        }
    }
}
