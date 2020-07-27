using System;
using System.IO;
using System.Net;
using System.Text;
using ImageMagick;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.Net.Sockets;
using IWshRuntimeLibrary;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Ionic.Zlib;
using Newtonsoft.Json;
using File = System.IO.File;

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
        }
        [Serializable]
        public class PackData
        {
            public List<MemoryStream> img;
        }
        [Serializable]
        public class ConfigJson
        {
            public int isMini = Convert.ToInt32(false);
            public int tcpu;
            public int tmon;
            public int tgpu;
            public int thdd;
            public int ucpu;
            public int uram;
            public int ugpu;
            public int uvram;
            public int ccpu;
            public int cgpu;
            public int rcpu;
            public int rgpu;
            public int pcpu;
            public int pgpu;
            public int width;
            public int height;
            public int fps;
            public int dataindex;
            public int clinetindex;
            public int serialindxe;
            public string path;
            public string ffpath;
            public int isPlayGIF = Convert.ToInt32(false);
            public int isSendData = Convert.ToInt32(false);
        }
        public Main()
        {
            InitializeComponent();
        }
        MemoryMappedFile mapFile;
        MemoryMappedViewAccessor Accessor;
        private CancellationToken token;
        public List<string> id = new List<string>();
        public List<string> value = new List<string>();
        public List<string> selested = new List<string>();
        public List<string> hddid = new List<string>();
        public List<string> hddvalue = new List<string>();
        public UdpClient Udp;
        public Task recivesTask;
        public Task sendBmpTask, sendInfoTask;
        public uint selectedUI;
        ManualResetEvent resetBmp = new ManualResetEvent(true), resetInfo = new ManualResetEvent(true);
        public SynchronizationContext Sync = null;
        public List<string> clientList = new List<string>();
        public int packIndex = -1;
        public List<string> packList = new List<string>();
        public int playPostion; //播放进度
        public Process httpProcess; //http服务器
        public Shell CMD;
        private string packfile;
        public ConfigJson cfgjson = new ConfigJson();
        public void GetAidaInfo()
        {
            StringBuilder tmp = new StringBuilder();
            try
            {
                MemoryStream ms = new MemoryStream();
                for (int i = 0; i < Accessor.Capacity; i++)
                {
                    byte c = Accessor.ReadByte(i);
                    if (c == '\0')
                        break;
                    ms.WriteByte(c);
                }
                tmp.Append("<AIDA>");
                tmp.Append(Encoding.Default.GetString(ms.ToArray()));
                tmp.Append("</AIDA>");
                XDocument xmldoc = XDocument.Parse(tmp.ToString());
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
                return;
            }
        }
        public void InsertInfo(IEnumerable<XElement> xel)
        {
            foreach (var element in xel)
            {
                for (int i = 1; i < 11; i++)
                {
                    if (element.Element("id").Value == "THDD" + i)
                    {
                        hddid.Add(element.Element("id").Value);
                        hddvalue.Add(element.Element("value").Value);
                    }
                }
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
                    case "PGPU1TDPP": //GPU TDP
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

        public void QuerySelested()
        {
            if ((selectedUI & UI_TEMP_CPU) > 0)
                selested.Add("TCPU"); //CPU温度
            if ((selectedUI & UI_TEMP_GPU) > 0)
                selested.Add("TGPU1DIO"); //GPU温度
            if ((selectedUI & UI_TEMP_BOARD) > 0)
                selested.Add("TMOBO"); //主板温度
            if ((selectedUI & UI_TEMP_HDD) > 0)
                selested.Add("HDD1"); //主板温度
            if ((selectedUI & UI_USE_CPU) > 0)
                selested.Add("SCPUUTI"); //CPU使用率
            if ((selectedUI & UI_USE_GPU) > 0)
                selested.Add("SGPU1UTI"); //GPU使用率
            if ((selectedUI & UI_USE_RAM) > 0)
                selested.Add("SMEMUTI"); //RAM使用率
            if ((selectedUI & UI_USE_VRAM) > 0)
                selested.Add("SVMEMUSAGE"); //显存使用率
            if ((selectedUI & UI_RATE_CPU) > 0)
                selested.Add("SCPUCLK"); //CPU频率
            if ((selectedUI & UI_RATE_GPU) > 0)
                selested.Add("SGPU1CLK"); //GPU频率
            if ((selectedUI & UI_SPEED_GPU) > 0)
                selested.Add("FCPU"); //CPU风扇转速
            if ((selectedUI & UI_SPEED_GPU) > 0)
                selested.Add("FGPU1"); //GPU风扇转速
            if ((selectedUI & UI_POWER_CPU) > 0)
            {
                selested.Add("VCPU");
                selested.Add("PCPUPKG");
            }
            if ((selectedUI & UI_POWER_GPU) > 0)
                selested.Add("VGPU1");
            selested.Add("PGPU1TDPP");
        }
        public void SetButtonText(object o)
        {
            var sa = o as string[];
            Type FormType = GetType();
            FieldInfo[] fi = FormType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (FieldInfo info in fi)
            {
                if (info.FieldType == typeof(Button))
                {
                    Button b = (info.GetValue(this)) as Button;

                    if (b.Name == sa[0])
                    {
                        if (b.Name == "btnLed")
                            b.Text = sa[1] == "0" ? "开灯" : "关灯";
                        else if (b.Name == "btnDisplay")
                            b.Text = sa[1] == "0" ? "开屏" : "关屏";
                    }
                }
            }
        }
        public void SetLogbox(object o)
        {
            logBox.AppendText((string)o + Environment.NewLine);
        }

        private void SetPlayInit(object o)
        {
            tbarPlay.Maximum = (int)o;
            btnStartPause.Text = "‖";
        }

        private void SetPlayStatus(object o)
        {
            var ia = (int[])o;
            lblPlay.Text = $"{ia[0]}/{ia[1]}";
            tbarPlay.Value = ia[0];

        }


        public void AddClientBox(object o)
        {
            lbxClient.Items.Add(o as string);

            lock (clientList)
            {
                clientList.Add(o as string);
            }
        }
        public void AddClient(IPEndPoint addr)
        {
            string s = addr.ToString();

            if (lbxClient.Items.IndexOf(s) < 0)
            {
                Sync.Send(AddClientBox, s);
            }
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
            short len = BitConverter.ToInt16(ba, 2);
            Packet p = new Packet(cmd);
            if (ba.Length != len + 4)
                return p;
            byte[] data = new byte[len];
            Array.Copy(ba, 4, data, 0, len);
            p.data = data;
            return p;
        }
        private void DataChange(object source, FileSystemEventArgs e)
        {
            Sync.Send(FlushPack, null);
        }

        public void FlushPack(object o)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/data", "*.dat");
            dataBox.Items.Clear();
            lock (packList)
            {
                packList.Clear();
                foreach (var f in files)
                {
                    dataBox.Items.Add(Path.GetFileName(f));
                    packList.Add(Path.GetFileName(f));
                }
            }
        }
        private bool AIDAQuery()
        {
            try
            {
                mapFile = MemoryMappedFile.OpenExisting("AIDA64_SensorValues");
                Accessor = mapFile.CreateViewAccessor();
                return true;
            }
            catch
            {
                MessageBox.Show("请启动AIDA64后再尝试发送！");
                return false;
            }
        }


        public void DoUpdate(string filename)
        {

            if (httpProcess != null && !httpProcess.HasExited)
                httpProcess.Kill();

            httpProcess = new Process();
            httpProcess.StartInfo.FileName = "python.exe";
            httpProcess.StartInfo.Arguments = "httpserver.py " + filename;
            httpProcess.StartInfo.UseShellExecute = false;
            httpProcess.StartInfo.RedirectStandardOutput = true;
            httpProcess.StartInfo.RedirectStandardInput = true;
            httpProcess.StartInfo.RedirectStandardError = true;
            httpProcess.StartInfo.CreateNoWindow = true;
            httpProcess.Start();
        }

        public bool CheckFileFromPath(string fname)
        {
            Process process = new Process();

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fname);
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                process.Close();
            }

            return true;
        }

        public void GetConfigValue()
        {
            cfgjson.tcpu = Convert.ToInt32(cpuTmp.Checked);
            cfgjson.tgpu = Convert.ToInt32(gpuTmp.Checked);
            cfgjson.thdd = Convert.ToInt32(hddTmp.Checked);
            cfgjson.tmon = Convert.ToInt32(mbTmp.Checked);
            cfgjson.ucpu = Convert.ToInt32(cpuUTI.Checked);
            cfgjson.ugpu = Convert.ToInt32(gpuUTI.Checked);
            cfgjson.uram = Convert.ToInt32(ramUTI.Checked);
            cfgjson.uvram = Convert.ToInt32(vramUTI.Checked);
            cfgjson.ccpu = Convert.ToInt32(cpuClk.Checked);
            cfgjson.cgpu = Convert.ToInt32(gpuClk.Checked);
            cfgjson.rcpu = Convert.ToInt32(cpuRpm.Checked);
            cfgjson.rgpu = Convert.ToInt32(gpuRpm.Checked);
            cfgjson.pcpu = Convert.ToInt32(cpuVol.Checked);
            cfgjson.pgpu = Convert.ToInt32(gpuVol.Checked);
            cfgjson.clinetindex = lbxClient.SelectedIndex;
            cfgjson.serialindxe = cbxSerial.SelectedIndex;
            cfgjson.dataindex = dataBox.SelectedIndex;
            cfgjson.fps = (int)nbxFPS.Value;
            cfgjson.width = (int)nbxWidth.Value;
            cfgjson.height = (int)nbxHeight.Value;
            cfgjson.path = binPath.Text;
        }
        public void SetConfigValue()
        {
            cpuTmp.Checked = Convert.ToBoolean(cfgjson.tcpu);
            gpuTmp.Checked = Convert.ToBoolean(cfgjson.tgpu);
            hddTmp.Checked = Convert.ToBoolean(cfgjson.thdd);
            mbTmp.Checked = Convert.ToBoolean(cfgjson.tmon);
            cpuUTI.Checked = Convert.ToBoolean(cfgjson.ucpu);
            gpuUTI.Checked = Convert.ToBoolean(cfgjson.ugpu);
            ramUTI.Checked = Convert.ToBoolean(cfgjson.uram);
            vramUTI.Checked = Convert.ToBoolean(cfgjson.uvram);
            cpuClk.Checked = Convert.ToBoolean(cfgjson.ccpu);
            gpuClk.Checked = Convert.ToBoolean(cfgjson.cgpu);
            cpuRpm.Checked = Convert.ToBoolean(cfgjson.rcpu);
            gpuRpm.Checked = Convert.ToBoolean(cfgjson.rgpu);
            cpuVol.Checked = Convert.ToBoolean(cfgjson.pcpu);
            gpuVol.Checked = Convert.ToBoolean(cfgjson.pgpu);
            dataBox.SelectedIndex = cfgjson.dataindex;
            nbxFPS.Value = cfgjson.fps;
            nbxWidth.Value = cfgjson.width;
            nbxHeight.Value = cfgjson.height;
            binPath.Text = cfgjson.path;
        }
        public void WriteConfig()
        {
            GetConfigValue();
            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(cfgjson));
                SetLogbox("写入成功");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void ReadConfig()
        {
            try
            {
                cfgjson = JsonConvert.DeserializeObject<ConfigJson>(File.ReadAllText("config.json"));
                SetLogbox("载入成功");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {

            if (!File.Exists("esptool.exe"))
                File.WriteAllBytes("esptool.exe", Properties.Resources.esptool);
            if (!File.Exists("httpserver.py"))
                File.WriteAllBytes("httpserver.py", Properties.Resources.httpserver);
            if (!File.Exists("说明.txt"))
                File.WriteAllBytes("说明.txt", Properties.Resources.说明);
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/data"))
                Directory.CreateDirectory("data");
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/firmware"))
                Directory.CreateDirectory("firmware");
            var initbin = Directory.GetCurrentDirectory() + "/firmware/init.bin";
            if (!File.Exists(initbin))
            {
                using (FileStream fs = new FileStream(initbin, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    byte[] ba = (byte[]) Properties.Resources.ResourceManager.GetObject("init", null);
                    fs.Write(ba, 0, ba.Length);
                }
            }

            FlushPack(null);
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Directory.GetCurrentDirectory() + "/data",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };
            watcher.Created += DataChange;
            watcher.Deleted += DataChange;
            watcher.EnableRaisingEvents = true;
            cbxSerial.Items.AddRange(SerialPort.GetPortNames());
            if (cbxSerial.Items.Count > 0)
                cbxSerial.SelectedIndex = 0;

            //lbxClient.SelectedIndex = cfgjson.clinetindex;
            if (cbxSerial.Items.Count > 0)
                cbxSerial.SelectedIndex = cfgjson.serialindxe;

            Sync = SynchronizationContext.Current;
            IPEndPoint remoteAddr = new IPEndPoint(IPAddress.Any, 8266);
            Udp = new UdpClient(remoteAddr);
            recivesTask = new Task(() =>
            {
                while (true)
                {
                    byte[] packet = Udp.Receive(ref remoteAddr);

                    if (packet.Length > 2)
                    {
                        var p = ParsePacket(packet);
                        switch (p.cmd)
                        {
                            case PACKET_ALIVE:
                                AddClient(remoteAddr);

                                if (File.Exists(binPath.Text))
                                {

                                    using (FileStream fs = new FileStream(binPath.Text, FileMode.Open))
                                    {
                                        var espmd5 = Encoding.Default.GetString(p.data).ToUpper();
                                        var binmd5 = MD5Helper.CalcMD5(fs);

                                        if (espmd5 != binmd5)
                                        {
                                            byte[] pack = BuildPacket(PACKET_UPDATE);
                                            Udp.Send(pack, pack.Length, remoteAddr);
                                            DoUpdate(binPath.Text);
                                        }
                                    }

                                }
                                break;
                            case PACKET_TOGGLE_LED:
                                Sync.Send(SetButtonText, new[] { "btnLed", p.data[0].ToString() });
                                break;
                            case PACKET_TOGGLE_DISPLAY:
                                Sync.Send(SetButtonText, new[] { "btnDisplay", p.data[0].ToString() });
                                break;

                        }
                    }
                }
            });
            recivesTask.Start();
            if (!File.Exists("config.json"))
            {
                File.Create("config.json").Close();
                WriteConfig();
            }
            else
            {
                ReadConfig();
                if (Convert.ToBoolean(cfgjson.isMini))
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    notifyIcon1.Visible = true;
                }
                SetConfigValue();
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        if (lbxClient.Items.Count > 0)
                        {
              
                            if (Convert.ToBoolean(cfgjson.isPlayGIF))
                            {
                                btnStartPause.PerformClick();
                            }

                            if (Convert.ToBoolean(cfgjson.isSendData))
                            {
                                btnSendData.PerformClick();
                            }
                            Sync.Send(SetLogbox,"成功连接到8266");
                            break;
                        }
                    }
                });
                task.Start();
            }
        }
        private void 清空日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.ResetText();
        }

        private byte[] ConvertXBM(string input)
        {
            string bytes = System.Text.RegularExpressions.Regex
                .Match(input, @"\{(.*)\}", System.Text.RegularExpressions.RegexOptions.Singleline).Groups[1].Value;
            string[] StringArray = bytes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            byte[] pixels = new byte[StringArray.Length - 1];
            for (int k = 0; k < StringArray.Length - 1; k++)
                if (byte.TryParse(StringArray[k].TrimStart().Substring(2, 2), NumberStyles.HexNumber,
                    CultureInfo.CurrentCulture, out byte result))
                    pixels[k] = result;
                else
                    throw new Exception();

            return pixels;
        }

        private void UdpSendXBM(byte[] data, int width, int height)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string[] s;

                lock (clientList)
                {
                    if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                        return;
                    s = clientList[0].Split(':');
                }

                IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
                ms.Write(new byte[] { Convert.ToByte(width), Convert.ToByte(height) }, 0, 2);
                ms.Write(data, 0, data.Length);
                byte[] packet = BuildPacket(PACKET_DISPLAY_IMG, ms.ToArray());
                Udp.Send(packet, packet.Length, addr);
            }
        }
        private void ProcPack(string file, int width, int height)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            MemoryStream ms = new MemoryStream();
            MemoryStream oms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Dispose();
            ms.Seek(0, SeekOrigin.Begin);
            var data = GZipStream.UncompressBuffer(ms.ToArray());
            ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Seek(0, SeekOrigin.Begin);
            var formatter = new BinaryFormatter();
            PackData pack = (PackData)formatter.Deserialize(ms);
            Sync.Send(SetPlayInit, pack.img.Count);
            var imgList = pack.img.ToArray();
            for (playPostion = 0; playPostion < imgList.Length; playPostion++)
            {
                resetBmp.WaitOne();

                lock (packList)
                {
                    if (packList[packIndex] != Path.GetFileName(file))
                    {
                        file = Directory.GetCurrentDirectory() + "\\data\\" + packList[packIndex];
                        return;
                    }

                }
                var buf = ConvertXBM(Encoding.Default.GetString(imgList[playPostion].ToArray()));
                UdpSendXBM(buf, width, height);
                MagickImage img = new MagickImage(imgList[playPostion].ToArray()) { Format = MagickFormat.Xbm };
                img.Format = MagickFormat.Bmp;
                buf = img.ToByteArray();
                ms = new MemoryStream();
                ms.Write(buf, 0, buf.Length);
                pictureBox.Image = Image.FromStream(ms);
                Sync.Send(SetPlayStatus, new int[] { playPostion, pack.img.Count });
                playPostion++;

                ms.Dispose();
                oms.Dispose();
                Thread.Sleep((int)Math.Round(1000 / nbxFPS.Value));
            }



        }

        private void BtnLed_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_TOGGLE_LED);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
        }
        private void BtnReboot_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_REBOOT);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
        }

        private void Erase_flash_Click(object sender, EventArgs e)
        {
            if (cbxSerial.SelectedIndex < 0)
            {
                MessageBox.Show("请选择串口!");
                return;
            }
            erase_flash.Enabled = false;
            var sname = cbxSerial.Text;
            tsLbl.Text = "擦除Flash...";
            Process esp = new Process();
            esp.StartInfo.FileName = "esptool.exe";
            esp.StartInfo.Arguments = $"--port { sname} erase_flash";
            esp.Start();
            esp.WaitForExit();
            tsLbl.Text = "擦除完毕";
            erase_flash.Enabled = true;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;

            if (MessageBox.Show("复位将会丢失全部设置信息，是否确定？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_RESET);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
        }
        private void SelectAll_Click(object sender, EventArgs e)
        {
            vramUTI.Checked = true;
            cpuClk.Checked = true;
            cpuRpm.Checked = true;
            cpuTmp.Checked = true;
            cpuUTI.Checked = true;
            cpuVol.Checked = true;
            gpuClk.Checked = true;
            gpuRpm.Checked = true;
            gpuTmp.Checked = true;
            gpuUTI.Checked = true;
            gpuVol.Checked = true;
            ramUTI.Checked = true;
            vramUTI.Checked = true;
            hddTmp.Checked = true;
            mbTmp.Checked = true;
        }
        private void UnSelectAll_Click(object sender, EventArgs e)
        {
            vramUTI.Checked = false;
            cpuClk.Checked = false;
            cpuRpm.Checked = false;
            cpuTmp.Checked = false;
            cpuUTI.Checked = false;
            cpuVol.Checked = false;
            gpuClk.Checked = false;
            gpuRpm.Checked = false;
            gpuTmp.Checked = false;
            gpuUTI.Checked = false;
            gpuVol.Checked = false;
            ramUTI.Checked = false;
            vramUTI.Checked = false;
            hddTmp.Checked = false;
            mbTmp.Checked = false;
        }
        private void CpuTmp_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_TEMP_CPU;
            else
                selectedUI ^= UI_TEMP_CPU;
        }
        private void MbTmp_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_TEMP_BOARD;
            else
                selectedUI ^= UI_TEMP_BOARD;
        }
        private void GpuTmp_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_TEMP_GPU;
            else
                selectedUI ^= UI_TEMP_GPU;
        }
        private void HddTmp_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_TEMP_HDD;
            else
                selectedUI ^= UI_TEMP_HDD;
        }
        private void CpuUTI_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_USE_CPU;
            else
                selectedUI ^= UI_USE_CPU;
        }
        private void RamUTI_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_USE_RAM;
            else
                selectedUI ^= UI_USE_RAM;
        }
        private void GpuUTI_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_USE_GPU;
            else
                selectedUI ^= UI_USE_GPU;
        }
        private void VramUTI_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_USE_VRAM;
            else
                selectedUI ^= UI_USE_VRAM;
        }
        private void CpuClk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_RATE_CPU;
            else
                selectedUI ^= UI_RATE_CPU;
        }
        private void GpuClk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_RATE_GPU;
            else
                selectedUI ^= UI_RATE_GPU;
        }
        private void CpuRpm_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_SPEED_CPU;
            else
                selectedUI ^= UI_SPEED_CPU;
        }
        private void GpuRpm_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_SPEED_GPU;
            else
                selectedUI ^= UI_SPEED_GPU;
        }

        private void CpuVol_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_POWER_CPU;
            else
                selectedUI ^= UI_POWER_CPU;
        }
        private void GpuVol_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                selectedUI |= UI_POWER_GPU;
            else
                selectedUI ^= UI_POWER_GPU;
        }

        private void 制作动画包ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackForm p = new PackForm(this);
            p.ShowDialog(this);
        }

        private void DataBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            packIndex = (sender as ComboBox).SelectedIndex;
            packfile = Directory.GetCurrentDirectory() + "/data/" + dataBox.Text;
        }

        private void 开源地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FantasyGmm/Aida64_Esp8266_DisplayControler");
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                Activate();
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void 创建桌面快捷方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Aida64_DisplayControler", Process.GetCurrentProcess().MainModule.FileName, iconLocation: Process.GetCurrentProcess().MainModule.FileName);
            }
            catch (Exception)
            {
                MessageBox.Show("创建桌面快捷方式失败");                
            }
        }
        public static void CreateShortcut(string directory, string shortcutName, string targetPath,
            string description = null, string iconLocation = null)
        {
            string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortcutName));
            if (!System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);//创建快捷方式对象
                shortcut.TargetPath = targetPath;//指定目标路径
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);//设置起始位置
                shortcut.WindowStyle = 1;//设置运行方式，默认为常规窗口
                shortcut.Description = description;//设置备注
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;//设置图标路径
                shortcut.Save();//保存快捷方式
            }
        }

        private void 开机启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string systemStartPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string appPath = Process.GetCurrentProcess().MainModule.FileName;
            try
            {
                CreateShortcut(systemStartPath, "Aida64_DisplayControler", appPath, iconLocation: Process.GetCurrentProcess().MainModule.FileName);
            }
            catch (Exception)
            {
                MessageBox.Show("创建开启启动失败");
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                cfgjson.isMini = Convert.ToInt32(true);
            }
            else
            {
                cfgjson.isMini = Convert.ToInt32(false);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                Activate();
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void TbarPlay_Scroll(object sender, EventArgs e)
        {
            var bar = sender as TrackBar;
            playPostion = bar.Value;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            resetBmp.Reset();
            btnStartPause.Text = "▶";
            playPostion = 0;
            SetPlayStatus(new [] { 0, 0 });
            tbarPlay.Value = 0;
            cfgjson.isPlayGIF = Convert.ToInt32(false);
            SetLogbox("停止发送动画");
        }

        private void SelBin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog
            {
                Filter = "固件文件(*.bin)|*.bin"
            };
            fd.ShowDialog(); binPath.Text = fd.FileName;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (httpProcess != null && !httpProcess.HasExited)
                httpProcess.Kill();
            WriteConfig();
        }

        private void BtnSerial_Click(object sender, EventArgs e)
        {
            if (cbxSerial.SelectedIndex < 0)
            {
                MessageBox.Show("请选择串口!");
                return;
            }
            btnSerial.Enabled = false;
            tsLbl.Text = "正在上传固件...";
            string sname = cbxSerial.Text;
            string firmware = "\"" + Directory.GetCurrentDirectory() + "\\firmware\\init.bin" + "\"";
            if (!string.IsNullOrEmpty(binPath.Text))
            {
                firmware = "\"" + binPath.Text + "\"";
            }
            var outdataHandler = new DataReceivedEventHandler((object o, DataReceivedEventArgs ee) =>
            {
                var s = ee.Data;

                if (s != null)
                {
                    var m = Regex.Match(s, @"(\b\d{1,3}\b)(\s)+%");

                    if (m.Success)
                    {
                        var progress = int.Parse(m.Groups[1].Value);

                        Invoke(new MethodInvoker(() =>
                        {
                            tsProgress.Value = progress;
                        }));

                    }
                }
            });
            var exitHandler = new EventHandler((object o, EventArgs ee) =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    tsLbl.Text = "固件上传完毕";
                    tsProgress.Value = 100;
                    btnSerial.Enabled = true;
                }));
            });
            CMD = new Shell("esptool.exe", $"--port {sname} -b 256200  write_flash --flash_mode qio --flash_freq 80m 0x00000 {firmware}", Directory.GetCurrentDirectory(), outdataHandler, exitHandler);
            CMD.Start();
        }

        private void CleanConfig_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_RESET);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/ctrget");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/FantasyGmm");
        }

        private void 保存配置文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetConfigValue();
            SetLogbox("保存中...");
            WriteConfig();
        }

        private void 载入配置文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadConfig();
            SetLogbox("载入中...");
            SetConfigValue();
        }

        private void BtnStartPause_Click(object sender, EventArgs e)
        {
            if (btnStartPause.Text == "‖")
            {
                SetLogbox("暂停发送动画");
                resetBmp.Reset();
                btnStartPause.Text = "▶";
                cfgjson.isPlayGIF = Convert.ToInt32(false);
            }
            else
            {
                SetLogbox("开始发送动画");
                if (dataBox.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择动画文件!");
                    return;
                }
                packfile = Directory.GetCurrentDirectory() + "\\data\\" + dataBox.Text;
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\data\\" + dataBox.Text))
                {
                    MessageBox.Show("动画文件不存在!");
                    return;
                }
                btnStartPause.Text = "‖";
                cfgjson.isPlayGIF = Convert.ToInt32(true);
                if (sendBmpTask == null)
                {
                    sendBmpTask = new Task(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            var width = Convert.ToInt32(nbxWidth.Value);
                            var height = Convert.ToInt32(nbxHeight.Value);
                            ProcPack(packfile, width, height);
                        }
                    }, token);
                    sendBmpTask.Start();
                }
                else
                {
                    resetBmp.Set();
                }
            }
        }

        private void BtnSendData_Click(object sender, EventArgs e)
        {
            if (btnSendData.Text == "停止发送数据")
            {
                resetInfo.Reset();
                btnSendData.Text = "发送监测数据";
                cfgjson.isSendData = Convert.ToInt32(false);
            }
            else
            {
                if (!AIDAQuery())
                    return;
                string[] s;
                if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                    return;
                cfgjson.isSendData = Convert.ToInt32(true);
                s = clientList[0].Split(':');
                IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
                btnSendData.Text = "停止发送数据";
                if (sendInfoTask == null)
                {
                    sendInfoTask = new Task(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            resetInfo.WaitOne();
                            if (selectedUI == 0)
                                continue;
                            id.Clear();
                            value.Clear();
                            selested.Clear();
                            hddid.Clear();
                            hddvalue.Clear();
                            GetAidaInfo();
                            QuerySelested();
                            JObject jsobj = new JObject
                            {
                                {"l", selested.Count.ToString()},
                                {"hl", hddid.Count.ToString()},
                            };

                            for (int i = 0; i < id.Count; i++)
                            {
                                foreach (var sel in selested)
                                {
                                    if (id[i] == sel)
                                    {
                                        jsobj.Add(id[i], value[i]);
                                    }
                                }
                            }
                            for (int i = 0; i < hddid.Count; i++)
                            {
                                jsobj.Add(hddid[i], hddvalue[i]);
                            }
                            byte[] pack = BuildPacket(PACKET_DISPLAY_INFO, Encoding.UTF8.GetBytes(jsobj.ToString()));
                            Udp.Send(pack, pack.Length, addr);
                            Thread.Sleep((int)nbxFPS.Value);
                        }
                    }, token);
                    sendInfoTask.Start();
                }
                else
                {
                    resetInfo.Set();
                }
            }
        }
    }
}