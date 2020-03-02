using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

namespace Aida64_Esp8266_DisplayControler
{
    public partial class Main : Form
    {
        public struct Packet
        {
           public byte cmd;
           public byte ver;
           public short len;
           public byte[] data;
           
            public Packet(byte c, byte[] d = null, short l = 0)
            {
                cmd = c;
                ver = 1;
                len = l;
                data = d;
            }

        };
        public Main()
        {
            InitializeComponent();
        }
        public List<string> id = new List<string>();
        public List<string> value = new List<string>();
        public List<string> selested = new List<string>();
        public UdpClient Udp;
        public Task recivesTask;
        public Task sendTask;
        public SynchronizationContext Sync = null;

        public List<string> clientList = new List<string>();

        const byte PACKET_ALIVE = 0X0;
        const byte PACKET_OK = 0X1;
        const byte PACKET_FAIL = 0X2;

        const byte PACKET_DISPLAY_IMG = 0XF;
        const byte PACKET_DISPLAY_INFO = 0X10;
        const byte PACKET_GET_INFO = 0X11;
        const byte PACKET_TOGGLE_LED = 0X12;
        const byte PACKET_TOGGLE_DISPLAY = 0X13;
        const byte PACKET_REBOOT = 0X14;

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
            catch (Exception)
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
            /*

            //Server

            if (sendTask.Status == null)
                return;
            if (sendTask.Status == TaskStatus.Running)
                return;

            IPEndPoint localIp = new IPEndPoint(IPAddress.Any, 8266);

            udpServer = new UdpClient(localIp);
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
            */

        }

        public void SetLogbox(object o)
        {
            this.logBox.AppendText(o as string + Environment.NewLine);
        }


        public void SetButtonText(object o)
        {
            var sa = o as string[];

            Type FormType = this.GetType();
            FieldInfo[] fi = FormType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo info in fi)
            {
                if (info.FieldType == typeof(Button))
                {
                    Button b = (info.GetValue(this)) as Button;
                    if (b.Name == sa[0])
                    {
                        b.Text = sa[1] == "0" ? "开灯" : "关灯";
                    }
                }

            }
        }

        public void AddClientBox(object o)
        {
            clientcbx.Items.Add(o as string);
        }

        public void AddClient(IPEndPoint addr)
        {
            string s = addr.ToString();

            if (clientcbx.Items.IndexOf(s) < 0)
            {
                Sync.Send(AddClientBox, s);
            }
        }

        public void QuerySelested()
        {
            if (cpuTmp.Checked)
                selested.Add("TCPU");
        }

        public byte[] BuildPacket(byte cmd, byte[] data = null)
        {
            
            int len = data == null ? 0 : data.Length;

            if (len > 65535)
                return null;
            MemoryStream mem = new MemoryStream();
            mem.WriteByte(cmd);
            mem.WriteByte(0x1);
            mem.Write(BitConverter.GetBytes((short)len), 0, 2);
            if (data != null)
                mem.Write(data, 0, len);
            return mem.ToArray();
        }


        public Packet ParsePacket(byte[] ba)
        {
            byte cmd = ba[0];
            short len = BitConverter.ToInt16(ba, 1);
            Packet p = new Packet(cmd);

            if (ba.Length != len + 4)
                return p;
            byte[] data = new byte[len];
            Array.Copy(ba, 4, data, 0, len);
            p.data = data;
            return p;
        }

 
        private void Main_Load(object sender, EventArgs e)
        {
            /*
            return Array.FindAll(assemblyArray, delegate (Type type)
            {
                return (type.BaseType.FullName == allType && type.FullName != mainType);
            });
            */
            Sync = SynchronizationContext.Current;
            IPEndPoint remoteAddr = new IPEndPoint(IPAddress.Any, 8266);
            Udp = new UdpClient(remoteAddr);
            recivesTask = new Task(() =>
            {
                while (true)
                {
                    byte[] pack = Udp.Receive(ref remoteAddr);

                    if (pack.Length > 2)
                    {
                        Sync.Send(SetLogbox, pack[0].ToString());
                        var p = ParsePacket(pack);
                        switch (p.cmd)
                        {
                            case PACKET_ALIVE:
                                AddClient(remoteAddr);
                                break;
                            case PACKET_GET_INFO:
                                Sync.Post(SetLogbox, p.data);
                                break;
                            case PACKET_TOGGLE_LED:
                                var i = 0;
                                break;
                            default:
                                
                                break;
                        }

                    }
                }
            });
            recivesTask.Start();
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

        private byte[] Convent2BMP(string file)
        {
            Bitmap source = new Bitmap(file);
            Bitmap bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format8bppIndexed);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms,ImageFormat.Bmp);
            return ms.GetBuffer();
        }
        //文件流转byte[]
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
                    byte[] img =BuildPacket(PACKET_DISPLAY_IMG, Convent2BMP(file));
                    Udp.Send(img, img.Length);
                }
            }

            if (baButton.Checked)
            {
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\bad apple"))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = BuildPacket(PACKET_DISPLAY_IMG, Convent2BMP(file));
                    Udp.Send(img, img.Length);
                }
            }

            if (asusButton.Checked)
            {
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\bilibili"))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = BuildPacket(PACKET_DISPLAY_IMG, Convent2BMP(file));
                    Udp.Send(img, img.Length);
                }
            }

            if (customButton.Checked)
            {
                if (customPath.Text == string.Empty)
                    return;
                foreach (var file in Directory.GetFiles(customPath.Text))
                {
                    pictureBox.ImageLocation = file;
                    byte[] img = BuildPacket(PACKET_DISPLAY_IMG, Convent2BMP(file));
                    Udp.Send(img, img.Length);
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

        private void BtnLed_Click(object sender, EventArgs e)
        {
            if (clientcbx.Text.IndexOf(":") < 0)
                return;
            string[] s = clientcbx.Text.Split(':');
            byte[] ba = BuildPacket(PACKET_TOGGLE_LED, Encoding.Default.GetBytes("test hello"));
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), Int32.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
            //
        }

        private void BtnDisplay_Click(object sender, EventArgs e)
        {
            //
        }

        private void BtnReboot_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
