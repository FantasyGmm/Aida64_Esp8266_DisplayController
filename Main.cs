﻿using System;
using System.IO;
using System.Net;
using Ionic.Zlib;
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
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;

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
        public Main()
        {
            InitializeComponent();
        }
        private ResourceManager rm = new ResourceManager("Resources", Assembly.GetExecutingAssembly());
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
            catch (Exception ex)
            {
                Sync.Send(SetLogbox, ex.Message);
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
        public void SetLogbox(object o)
        {
            try
            {
                logBox.AppendText(o as string + Environment.NewLine);
            }
            catch
            {
                return;
            }
            
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
            lock(packList)
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
        private void Main_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/data"))
                Directory.CreateDirectory("data");
            FlushPack(null);
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Directory.GetCurrentDirectory() + "/data",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };
            watcher.Created += DataChange;
            watcher.Deleted += DataChange;
            watcher.EnableRaisingEvents = true;
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
                        var p = ParsePacket(pack);
                        switch (p.cmd)
                        {
                            case PACKET_ALIVE:
                                AddClient(remoteAddr);
                                break;
                            case PACKET_GET_INFO:
                                Sync.Send(SetLogbox, p.data);
                                break;
                            case PACKET_TOGGLE_LED:
                                Sync.Send(SetButtonText, new string[] { "btnLed", p.data[0].ToString() });
                                break;
                            case PACKET_TOGGLE_DISPLAY:
                                Sync.Send(SetButtonText, new string[] { "btnDisplay", p.data[0].ToString() });
                                break;
                        }
                    }
                }
            });
            recivesTask.Start();
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
            using(MemoryStream ms = new MemoryStream())
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
            Pack_Start:

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                var data = GZipStream.UncompressBuffer(ms.ToArray());
                ms = new MemoryStream();
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var formatter = new BinaryFormatter();
                PackData pack = (PackData)formatter.Deserialize(ms);
                foreach (var m in pack.img)
                {
                    resetBmp.WaitOne();

                    lock (packList)
                    {
                        if (packList[packIndex] != Path.GetFileName(file))
                        {
                            file = Directory.GetCurrentDirectory() + "/data/" + packList[packIndex];
                            goto Pack_Start;
                        }
                            
                    }
                    var buf = ConvertXBM(Encoding.Default.GetString(m.ToArray()));
                    UdpSendXBM(buf, width, height);
                    MagickImage img = new MagickImage(m.ToArray()) { Format = MagickFormat.Xbm };
                    img.Format = MagickFormat.Bmp;
                    buf = img.ToByteArray();
                    ms = new MemoryStream();
                    ms.Write(buf, 0, buf.Length);
                    pictureBox.Image = Image.FromStream(ms);
                    Thread.Sleep((int)timerInterval.Value);
                }

            }
        }
        private void BtnLed_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_TOGGLE_LED);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), Int32.Parse(s[1]));
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

                CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Aida64_DisplayControler", Process.GetCurrentProcess().MainModule.FileName,iconLocation: Process.GetCurrentProcess().MainModule.FileName);
            }
            catch (Exception)
            {
                MessageBox.Show("创建桌面快捷方式失败");
                throw;
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
                throw;
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
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

        private void BtnSendGif_Click(object sender, EventArgs e)
        {
            if (btnSendGif.Text == "停止发送动画")
            {
                resetBmp.Reset();
                btnSendGif.Text = "发送动画";
                return;
            }
            else
            {
                if (dataBox.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择动画文件!");
                    return;
                }
                string packfile = Directory.GetCurrentDirectory() + "/data/" + dataBox.Text;
                if (!System.IO.File.Exists(packfile))
                {
                    MessageBox.Show("动画文件不存在!");
                    return;
                }

                btnSendGif.Text = "停止发送动画";
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
                Sync.Send(SetLogbox, "已停止监测发送数据");
                btnSendData.Text = "发送监测数据";
            }
            else
            {
                if (!AIDAQuery())
                    return;
                string[] s;
                if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                    return;
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
                            Thread.Sleep((int)timerInterval.Value);
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