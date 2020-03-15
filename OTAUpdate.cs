using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aida64_Esp8266_DisplayControler
{
    public partial class OTAUpdate : Form
    {
        private CancellationToken token;
        ManualResetEvent reset= new ManualResetEvent(true);
        public OTAUpdate()
        {
            InitializeComponent();
        }

        private void SelBin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog
            {
                Filter = "固件文件(*.bin)|*.bin"
            };
            fd.ShowDialog();
            binPath.Text = fd.FileName;
        }

        private void StartUpdate_Click(object sender, EventArgs e)
        {
            Task httpServer = null;
            Main main = new Main();
            byte[] pack = main.BuildPacket(0x3);
            string[] s;
            if (main.clientList.Count == 0 || main.clientList[0].IndexOf(":") < 0)
                return;
            s = main.clientList[0].Split(':');
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
            main.Udp.Send(pack, pack.Length, addr);
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:81/");
            httpListener.Start();
            if (httpServer == null)
            {
                httpServer = new Task(() =>
                {
                    while(!token.IsCancellationRequested)
                    {
                        reset.WaitOne();
                        HttpListenerContext hlc = httpListener.GetContext();
                        HttpListenerRequest hlrequst = hlc.Request;
                        hlc.Response.StatusCode = 200;
                        using (StreamWriter sw = new StreamWriter(hlc.Response.OutputStream))
                        {
                            try
                            {
                                FileStream fs = new FileStream(binPath.Text, FileMode.Open);
                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);
                                sw.Write(buffer);
                                fs.Dispose();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("固件不存在");
                                throw;
                            }
                        }
                        string input = new StreamReader(hlrequst.InputStream).ReadToEnd();
                        if (input == "end")
                        {
                            hlc.Response.Abort();
                            httpListener.Close();
                            reset.Reset();
                        }
                    }
                });
                httpServer.Start();
            }
        }
    }
}