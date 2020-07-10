using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Ionic.Zlib;
using ImageMagick;

namespace Aida64_Esp8266_DisplayControler
{
    public partial class PackForm : Form
    {
        private SynchronizationContext Sync;
        private Main main;
        private Hashtable hashtable = new Hashtable();
        private Dictionary<int, int> threadPercent = new Dictionary<int, int>();
        int threadCount;

        public PackForm(Main main)
        {
            InitializeComponent();
            Sync = SynchronizationContext.Current;
            this.main = main;
        }

        private void PackForm_Load(object sender, EventArgs e)
        {
            var tcount = Environment.ProcessorCount;
            tbxThread.Value = tcount - 1;
            tbxThread.Maximum = tcount;
        }

        private void SetPbar(object o)
        {
            var dic = (int[])o;

            threadPercent[dic[0]] = dic[1];
            var total = 0;

            foreach (var d in threadPercent)
            {
                total += d.Value;
            }


            pbar.Value = total / threadCount;
        }
        private void BtnBrowser_Click(object sender, EventArgs e)
        {
            if(isVideo.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = false
                };
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    tbxPath.Text = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog od = new FolderBrowserDialog();
                if (od.ShowDialog(this) == DialogResult.OK)
                {
                    tbxPath.Text = od.SelectedPath;
                }
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if(!isVideo.Checked)
            {
                isVideo.Enabled = false;
                hashtable.Clear();
                threadPercent.Clear();
                threadCount = Convert.ToInt32(tbxThread.Value);
                var files = Directory.GetFiles(tbxPath.Text);
                MutilPack(files, threadCount);
            }
            else
            {
                isVideo.Enabled = false;
                hashtable.Clear();
                threadPercent.Clear();
                GetPicFromVideo(tbxPath.Text,nbxWidth.Value.ToString()+"x"+nbxHeight.Value.ToString(),fpsnum.Value.ToString());
                threadCount = Convert.ToInt32(tbxThread.Value);
                var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\VideoTempOutput");
                MutilPack(files, threadCount);
            }
        }


        private void PackImage(object files)
        {
            decimal i = 1;
            int lastpercent = 0;
            var fileArr = (string[])files;
            //Sync.Send(SetLog, $"线程{Thread.CurrentThread.ManagedThreadId}开始执行");


            foreach (var file in fileArr)
            {
                byte[] buf = Main.GetSingleBitmap(file);
                MagickImage img = new MagickImage(buf) { Format = MagickFormat.Xbm };
                var width = Convert.ToInt32(nbxWidth.Value);
                var height = Convert.ToInt32(nbxHeight.Value);
                img.Resize(new MagickGeometry($"{width}x{height}!"));
                buf = img.ToByteArray();

                lock (hashtable)
                {
                    hashtable.Add(Path.GetFileName(file), buf);
                }

                var td = (i / fileArr.Length) * 100;
                var percent = decimal.ToInt32(td);

                if (lastpercent != percent)
                {
                    lastpercent = percent;
                    Sync.Send(SetPbar, new int[] { Thread.CurrentThread.ManagedThreadId, percent });
                }

                i++;
            }
        }

        private async void MutilPack(string[] arr, int splitCount)
        {
            string fname;
            SaveFileDialog sd = new SaveFileDialog
            {
                Filter = "PackFile(*.dat)|*.dat",
                Title = "请输入保存文件名"
            };
            if (sd.ShowDialog(this) == DialogResult.OK)
            {
                fname = sd.FileName;
            }
            else
            {
                return;
            }
            int size = arr.Length / splitCount;

            Task[] taskArr = new Task[splitCount];
            pnMain.Enabled = false;

            for (int i = 0; i < splitCount; i++)
            {
                int index = i * size;
                if (i == splitCount - 1)
                    size = arr.Length;
                string[] subarr = arr.Skip(index).Take(size).ToArray();
                var t = Task.Run(() =>
                {

                    PackImage(subarr);

                });
                taskArr[i] = t;
            }
            await Task.WhenAll(taskArr);
            pbar.Value = 99;
            Main.PackData pack = new Main.PackData();
            List<MemoryStream> ls = new List<MemoryStream>();
            ArrayList akeys = new ArrayList(hashtable.Keys);
            akeys.Sort();


            foreach (string skey in akeys)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var ba = (byte[])hashtable[skey];
                    ms.Write(ba, 0, ba.Length);
                    ls.Add(ms);
                }
            }

            pack.img = ls;
            MemoryStream mm = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(mm, pack);
            var data = GZipStream.CompressBuffer(mm.ToArray());


            if (File.Exists(fname))
                File.Delete(fname);

            FileStream fs = new FileStream(fname, FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Dispose();
            pbar.Value = 100;
            if(Directory.GetFiles(Directory.GetCurrentDirectory() + "\\VideoTempOutput").Length >= 0)
            {
                foreach(var file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\VideoTempOutput"))
                {
                    File.Delete(file);
                }
                Directory.Delete(Directory.GetCurrentDirectory() + "\\VideoTempOutput");
            }
            Process.Start("Explorer.exe", "/select," + fname);
            pnMain.Enabled = true;
            mm.Dispose();
            Close();
        }

        public void GetPicFromVideo(string VideoName, string WidthAndHeight,string FrameRate)
        {
            if(string.IsNullOrEmpty(tbxPath.Text))
            {

            }
            string ffmpeg = Directory.GetCurrentDirectory() + "\\ffmpeg.exe";//ffmpeg执行文件的路径
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\VideoTempOutput");
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpeg)
            {
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = @"-i """ + VideoName + @"""" + " -r " + FrameRate + " -f image2 -s " + WidthAndHeight + " " + @"""" + Directory.GetCurrentDirectory() + "\\VideoTempOutput\\%d.jpg" + @""""
            };
            Process ffmpegP = new Process
            {
                StartInfo = startInfo
            };
            try
            {
                ffmpegP.Start();
                ffmpegP.WaitForExit();
                ffmpegP.Close();
            }
            catch
            {
                MessageBox.Show("导出视频帧失败，请确保ffmpeg.exe存在并且能够使用");
                return;
            }
        }
    }
}
