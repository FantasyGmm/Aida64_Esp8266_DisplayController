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
                    default:
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

        public void UdpClInit()
        {
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
                            switch (pack[1].ToString())
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
                            switch (pack[0])
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
        }

        public void UdpServer()
        {
            if (sendTask.Status == null)
                return;
            if (sendTask.Status == TaskStatus.Running)
                return;
            udpServer = new UdpClient(removteip);
            string data = null;
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
                                data += id[id.IndexOf(sel)] + "=" + value[value.IndexOf(sel)];
                            }
                            else
                            {
                                data += id[id.IndexOf(sel)] + "=" + value[value.IndexOf(sel)] + "|";
                            }

                        }

                    }
                }
            }
            sendTask = new Task(() =>
                {
                    udpServer.Send(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(data).Length);
                });
            sendTask.Start();
        }
        public void Queryselested()
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

        private void button1_Click(object sender, EventArgs e)
        {
            getData.Enabled = !getData.Enabled;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            id.Clear();
            value.Clear();
            GetAidaInfo();
            Queryselested();
            UdpClInit();
            UdpServer();
        }

        private void 清空日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.ResetText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getData.Enabled = false;
        }
    }
}
