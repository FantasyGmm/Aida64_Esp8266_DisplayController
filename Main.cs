using System;
using System.IO;
using System.Net;
using System.Data;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Globalization;
using ImageMagick;


namespace Aida64_Esp8266_DisplayControler
{
    /*
     * TODO:加入动画自选下拉框，自适应识别是打包好的dat文件或者文件夹
     */
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
        private CancellationToken token;

        public List<string> id = new List<string>();
        public List<string> value = new List<string>();
        public List<string> selested = new List<string>();
        public List<string> hddid = new List<string>();
        public List<string> hddvalue = new List<string>();
        public UdpClient Udp;
        public Task recivesTask;
        public Task sendBmpTask, sendInfoTask;
        public string bmpPath = "";
        public int bmpDealy = 100;
        public string json_out;
        public string xml_out;
        ManualResetEvent resetBmp = new ManualResetEvent(true), resetInfo = new ManualResetEvent(true);
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

        public void CreatDebugFile(string file, string data)
        {
            if (File.Exists(file))
                File.Delete(file);
            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Dispose();
            fs.Dispose();
        }

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
                xml_out = tmp;
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
            if (cpuTmp.Checked)
                selested.Add("TCPU"); //CPU温度
            if (gpuTmp.Checked)
                selested.Add("TGPU1DIO"); //GPU温度
            if (mbTmp.Checked)
                selested.Add("TMOBO"); //主板温度
            if (gpuClk.Checked)
                selested.Add("SGPU1CLK"); //GPU频率
            if (cpuClk.Checked)
                selested.Add("SCPUCLK"); //CPU频率
            if (cpuUTI.Checked)
                selested.Add("SCPUUTI"); //CPU使用率
            if (gpuUTI.Checked)
                selested.Add("SGPU1UTI"); //GPU使用率
            if (ramUTI.Checked)
                selested.Add("SMEMUTI"); //RAM使用率
            if (vramUTI.Checked)
                selested.Add("SVMEMUSAGE"); //显存使用率
            if (cpuRpm.Checked)
                selested.Add("FCPU"); //CPU风扇转速
            if (gpuRpm.Checked)
                selested.Add("FGPU1"); //GPU风扇转速
            if (gpuVol.Checked)
                selested.Add("VGPU1");
            if (cpuVol.Checked)
            {
                selested.Add("VCPU");
                selested.Add("PCPUPKG");
            }
        }


        public void SetLogbox(object o)
        {
            logBox.AppendText(o as string + Environment.NewLine);
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
            clientList.Add(o as string);
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
        private void Main_Load(object sender, EventArgs e)
        {
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

        private void GetAidaData_Tick(object sender, EventArgs e)
        {
            id.Clear();
            value.Clear();
            selested.Clear();
            hddid.Clear();
            hddvalue.Clear();
            GetAidaInfo();
            QuerySelested();
        }
        private void 清空日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.ResetText();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            bmpPanel.Enabled = cbSendBmp.Checked;
        }

        public static byte[] GetSingleBitmap(string file)
        {
            Bitmap pimage = new Bitmap(file);
            Bitmap source;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (pimage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(pimage.Width, pimage.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(pimage.HorizontalResolution, pimage.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(pimage, 0, 0);
                }
            }
            else
            {
                source = pimage;
            }

            // Lock source bitmap in memory
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits(sourceData);

            // Create destination bitmap
            System.Drawing.Bitmap destination =
                new System.Drawing.Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);

            // Lock destination bitmap in memory
            BitmapData destinationData =
                destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines
            for (int y = 0; y < height; y++)
            {
                int sourceIndex = y * sourceData.Stride;
                int destinationIndex = y * destinationData.Stride;
                byte destinationValue = 0;
                int pixelValue = 128;

                // Iterate pixels
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    int pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] +
                                     sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }

                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }

                    sourceIndex += 4;
                }

                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits(destinationData);

            // Dispose of source if not originally supplied bitmap
            if (source != pimage)
            {
                source.Dispose();
            }

            MemoryStream ms = new MemoryStream();
            destination.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }



        private byte[] ConvertXBM(string input)
        {
            string bytes = System.Text.RegularExpressions.Regex.Match(input, @"\{(.*)\}", System.Text.RegularExpressions.RegexOptions.Singleline).Groups[1].Value;
            string[] StringArray = bytes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            byte[] pixels = new byte[StringArray.Length - 1];
            for (int k = 0; k < StringArray.Length - 1; k++)
                if (byte.TryParse(StringArray[k].TrimStart().Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out byte result))
                    pixels[k] = result;
                else
                    throw new Exception();

            return pixels;
        }

        private void BtnLed_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;
            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_TOGGLE_LED);
            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), Int32.Parse(s[1]));
            Udp.Send(ba, ba.Length, addr);
            //
        }

        private void BtnDisplay_Click(object sender, EventArgs e)
        {
            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                return;

            string[] s = clientList[0].Split(':');
            byte[] ba = BuildPacket(PACKET_TOGGLE_DISPLAY);
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

        private void TimerInterval_ValueChanged(object sender, EventArgs e)
        {
            getAidaData.Interval = (int)timerInterval.Value;
            bmpDealy = (int)timerInterval.Value;
        }

        private void BaButton_CheckedChanged(object sender, EventArgs e)
        {
            bmpPath = Directory.GetCurrentDirectory() + @"\bad apple\";
        }

        private void BiliButton_CheckedChanged(object sender, EventArgs e)
        {
            bmpPath = Directory.GetCurrentDirectory() + @"\bilibili\";
        }

        private void AsusButton_CheckedChanged(object sender, EventArgs e)
        {
            bmpPath = Directory.GetCurrentDirectory() + @"\asus\";
        }

        private void CustomButton_CheckedChanged(object sender, EventArgs e)
        {
            customPath.Enabled = (sender as RadioButton).Checked;
            selButton.Enabled = (sender as RadioButton).Checked;
        }

        private void SelButton_Click(object sender, EventArgs e)
        {
            var op = new FolderBrowserDialog
            {
                Description = "请选择存放图片的文件夹"
            };
            if (op.ShowDialog() == DialogResult.OK)
            {
                bmpPath = op.SelectedPath;
                customPath.Text = op.SelectedPath;
            }
        }

        private void OutDebugFile_Click(object sender, EventArgs e)
        {
            CreatDebugFile("Aidainfo.json", json_out);    //输出源JSON
            CreatDebugFile("Aidainfo.xml", xml_out);    //输出源XML
        }

        private void BtnSendGif_Click(object sender, EventArgs e)
        {
            if (!cbSendBmp.Checked)
                return;
            string bmppath = bmpPath;
            int bmpindex = 0;
            if (!Directory.Exists(bmppath))
            {
                MessageBox.Show("请选择正确的文件夹！");
                return;
            }
            string[] bmplist = Directory.GetFiles(bmppath);
            if (btnSendGif.Text == "停止发送动画")
            {
                resetBmp.Reset();
                btnSendGif.Text = "发送动画";
                return;
            }
            else
            {
                btnSendGif.Text = "停止发送动画";
                if (sendBmpTask == null)
                {
                    sendBmpTask = new Task(() =>
                    {

                        while (!token.IsCancellationRequested)
                        {
                            resetBmp.WaitOne();
                            //检测按钮变动
                            if (bmppath != bmpPath)
                            {
                                if (!Directory.Exists(bmppath))
                                    continue;
                                bmppath = bmpPath;
                                bmpindex = 0;
                                bmplist = Directory.GetFiles(bmppath);
                            }
                            //重置动画播放
                            if (bmpindex > bmplist.Length)
                                bmpindex = 0;
                            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                                continue;
                            string[] s = clientList[0].Split(':');
                            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
                            //未选择文件不作任何操作
                            if (bmplist.Length == 0)
                                continue;

                            byte[] ib = GetSingleBitmap(bmplist[bmpindex]);
                            MagickImage img = new MagickImage(ib);
                            img.Format = MagickFormat.Xbm;

                            var width = Convert.ToInt32(nbxWidth.Value);
                            var height = Convert.ToInt32(nbxHeight.Value);
                            img.Resize(new MagickGeometry($"{width}x{height }!"));
                            byte[] tb = img.ToByteArray();
                           
                            using (MemoryStream memStream = new MemoryStream())
                            {
                                img.Format = MagickFormat.Jpg;
                                img.Write(memStream);
                                pictureBox.Image = Image.FromStream(memStream);
                            }


                            var data = ConvertXBM(System.Text.Encoding.Default.GetString(tb));

                            MemoryStream ms = new MemoryStream();
                            ms.Write(new byte[] { Convert.ToByte(width) , Convert.ToByte(height) }, 0, 2);
                            ms.Write(data, 0, data.Length);
                            
                            byte[] packet = BuildPacket(PACKET_DISPLAY_IMG, ms.ToArray());
                            Udp.Send(packet, packet.Length, addr);
                            bmpindex++;
                            Thread.Sleep(bmpDealy);
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
                getAidaData.Stop();
                resetInfo.Reset();
                Sync.Send(SetLogbox, "已停止监测发送数据");
                btnSendData.Text = "发送监测数据";
            }
            else
            {
                btnSendData.Text = "停止发送数据";
                getAidaData.Start();
                if (sendInfoTask == null)
                {
                    sendInfoTask = new Task(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {

                            if (clientList.Count == 0 || clientList[0].IndexOf(":") < 0)
                                continue;
                            resetInfo.WaitOne();
                            string[] s = clientList[0].Split(':');
                            IPEndPoint addr = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
                            if (!hddTmp.Checked)
                            {
                                hddid.Clear();
                                hddvalue.Clear();
                            }
                            JObject jsobj = new JObject
                            {
                                { "l", selested.Count },
                                { "hl",hddid.Count }
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
                            json_out = jsobj.ToString();
                            //Sync.Send(SetLogbox,json_out);
                            byte[] pack = BuildPacket(PACKET_DISPLAY_INFO,
                                System.Text.Encoding.UTF8.GetBytes(jsobj.ToString()));
                            Udp.Send(pack, pack.Length, addr);
                            Thread.Sleep(bmpDealy);
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
        #region 废弃代码
        /*
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
*/


        #endregion 废弃代码
    }
}