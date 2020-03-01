using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Aida64_Esp8266_DisplayControler
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public List<string> id = new List<string>();
        public List<string> value = new List<string>();
        public List<string> selested = new List<string>();
        public UdpClient udpClient;
        public UdpClient udpServer;
        public Task recivesTask;
        public Task sendTask;
        public IPEndPoint removteip = new IPEndPoint(IPAddress.Any, 8266);
        public SynchronizationContext SyncContext = null;
        public void GetAidaInfo()
        {
            string tmp = string.Empty + "<AIDA>";
            try
            {
                MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting("AIDA64_SensorValues");
                MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor();
                tmp = tmp ?? "";
                for (int i = 0; i < accessor.Capacity; i++)
                {
                    tmp += ((char)accessor.ReadByte(i)).ToString();
                }
                tmp = tmp.Replace("\0", "");
                tmp = tmp ?? "";
                accessor.Dispose();
                mappedFile.Dispose();
                tmp += "</AIDA>";
                XDocument xmldoc = XDocument.Parse(tmp);
                IEnumerable<XElement> sysEnumerator = xmldoc.Element("AIDA").Elements("sys");
                InsertInfo(sysEnumerator);
                IEnumerable<XElement> tempEnumerator = xmldoc.Element("AIDA").Elements("temp");
                InsertInfo(tempEnumerator);
                IEnumerable<XElement> fanEnumerator = xmldoc.Element("AIDA").Elements("fan");
                InsertInfo(fanEnumerator);
                IEnumerable<XElement> voltEnumerator = xmldoc.Element("AIDA").Elements("volt");
                InsertInfo(voltEnumerator);
                IEnumerable<XElement> pwrEnumerator = xmldoc.Element("AIDA").Elements("pwr");
                InsertInfo(pwrEnumerator);
            }
            catch (Exception )
            {
                MessageBox.Show("请开启AIDA64内存共享功能,并保持AIDA64后台运行");
            }
        }
        public void InsertInfo(IEnumerable<XElement> xel)
        {
            foreach (var element in xel)
            {
                switch (element.Element("id").Value)
                {
                    case "SCPUCLK": //CPU频率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "SCPUUTI": //CPU使用率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "SMEMUTI": //内存使用率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "SGPU1CLK": //GPU频率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "SGPU1UTI": //GPU使用率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "SVMEMUSAGE": //显存使用率
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "TMOBO": //主板温度
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "TCPU": //CPU温度
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "TGPU1DIO": //GPU温度
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "FCPU": //CPU风扇转速
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "FGPU1": //GPU风扇转速
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "VCPU": //CPU电压
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "VGPU1": //GPU电压
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    case "PCPUPKG": //CPU Package功耗
                        id.Add(element.Element("id").Value);
                        value.Add(element.Element("value").Value);
                        break;
                    /*  备用代码   */
                        /*
                        case "":
                            id.Add(element.Element("id").Value);
                            value.Add(element.Element("value").Value);
                            break;
                         */
                }
            }
        }

        public void UdpCS()
        {
            //Client
            if (recivesTask == null)
                return;
            if (recivesTask.Status == TaskStatus.Running)
                return;
            udpClient = new UdpClient(removteip);
            int statuscode = -1;
            recivesTask = new Task(() =>
            {
                if (statuscode != -1)
                {
                    while (true)
                    {
                        byte[] pack = udpClient.Receive(ref removteip);
                        if (pack.Length > 2)
                        {
                            switch (pack[0].ToString())
                            {
                                /*
                                case "":
                                    break;
                                case "":
                                    break;
                                case "":
                                    break;
                                case "":
                                    break;
                                case "":
                                    break;
                                case "":
                                    break;
                                    */
                            }

                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        byte[] pack = udpClient.Receive(ref removteip);
                        if (pack.Length > 2)
                        {
                            byte[] cdata = new byte[4];
                            for (int i = 1; i < 5; i++)
                            {
                                cdata[i - 1] = pack[i];
                            }
                            switch (BitConverter.ToInt32(cdata, 0))
                            {
                                case 0x0:
                                    statuscode = 0;
                                    SyncContext.Send(SetLogbox, "Esp8266 is Online");
                                    break;
                                case 0x1:
                                    statuscode = 1;
                                    SyncContext.Send(SetLogbox, "Esp8266 Run Success");
                                    break;
                                case 0x2:
                                    statuscode = 2;
                                    SyncContext.Send(SetLogbox, "Esp8266 Run Faild");
                                    break; ;
                            }
                        }
                    }
                }
            });
            recivesTask.Start();

            //Server

            if (sendTask.Status == null)
                return;
            if (sendTask.Status == TaskStatus.Running)
                return;
            udpServer = new UdpClient(removteip);
            string sdata = null;
            lock (id)
            {
                lock (value)
                {
                    lock (selested)
                    {
                        foreach (var sel in selested)
                        {
                            if (sel == selested[selested.Count - 1])
                            {
                                sdata += id[id.IndexOf(sel)] + "=" + value[value.IndexOf(sel)];
                            }
                            else
                            {
                                sdata += id[id.IndexOf(sel)] + "=" + value[value.IndexOf(sel)] + "|";
                            }

                        }

                    }
                }
            }
            sendTask = new Task(() =>
            {
                udpServer.Send(Encoding.UTF8.GetBytes(sdata), Encoding.UTF8.GetBytes(sdata).Length);
            });
            sendTask.Start();

        }

        public void QuerySelested()
        {
            if (cpuTmp.Checked)
                selested.Add("TCPU");
        }
        public void SetLogbox(object o)
        {
            this.logBox.AppendText(o as string);
        }
        private void Main_Load(object sender, EventArgs e)
        {
            SyncContext = SynchronizationContext.Current;
        }

        private void Runserver_Click(object sender, EventArgs e)
        {
            getAidaData.Enabled = !getAidaData.Enabled;
            UdpCS();
        }

        private void GetAidaData_Tick(object sender, EventArgs e)
        {
            id.Clear();
            value.Clear();
            GetAidaInfo();
            QuerySelested();
        }

        private void 清空日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.ResetText();
        }

        private void SelButton_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            tmpBox.Enabled = !tmpBox.Enabled;
            utiBox.Enabled = !utiBox.Enabled;
            clkBox.Enabled = !clkBox.Enabled;
            sendData.Enabled = !sendData.Enabled;
            sendGif.Enabled = sendGif.Enabled;
            asusButton.Enabled = true;
            baButton.Enabled = true;
            biliButton.Enabled = true;
            customButton.Enabled = true;
        }
        protected byte[] AuthGetFileData(string fileUrl)
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffur = new byte[fs.Length];
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(buffur);
                    bw.Close();
                }
                return buffur;
            }
        }
        private void SendGif_Tick(object sender, EventArgs e)
        {
            if (biliButton.Checked)
            {
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\bilibili"))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = AuthGetFileData(file);
                    udpServer.Send(img,img.Length);
                }
            }

            if (baButton.Checked)
            {
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\bad apple"))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = AuthGetFileData(file);
                    udpServer.Send(img, img.Length);
                }
            }

            if (asusButton.Checked)
            {
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\bilibili"))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = AuthGetFileData(file);
                    udpServer.Send(img, img.Length);
                }
            }

            if (customButton.Checked)
            {
                if (customPath.Text == string.Empty)
                    return;
                foreach (var file in Directory.GetFiles(customPath.Text))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = AuthGetFileData(file);
                    udpServer.Send(img, img.Length);
                }
            }
        }

        private void BaButton_CheckedChanged(object sender, EventArgs e)
        {
            asusButton.Enabled = !asusButton.Enabled;
            biliButton.Enabled = !biliButton.Enabled;
            customButton.Enabled = !customButton.Enabled;
        }

        private void BiliButton_CheckedChanged(object sender, EventArgs e)
        {
            asusButton.Enabled = !asusButton.Enabled;
            baButton.Enabled = !baButton.Enabled;
            customButton.Enabled = !customButton.Enabled;
        }

        private void AsusButton_CheckedChanged(object sender, EventArgs e)
        {
            baButton.Enabled = !baButton.Enabled;
            biliButton.Enabled = !biliButton.Enabled;
            customButton.Enabled = !customButton.Enabled;
        }

        private void CustomButton_CheckedChanged(object sender, EventArgs e)
        {
            asusButton.Enabled = !asusButton.Enabled;
            baButton.Enabled = !baButton.Enabled;
            biliButton.Enabled = !biliButton.Enabled;
        }

        private void SendData_Tick(object sender, EventArgs e)
        {

        }
    }
}
