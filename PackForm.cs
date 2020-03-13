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
        private SynchronizationContext Sync = null;
        private Task procTask = null;
        private Main main;
        private Hashtable hashtable = new Hashtable();

        public PackForm(Main main)
        {
            InitializeComponent();
            Sync = SynchronizationContext.Current;
            this.main = main;
        }

        private void PackForm_Load(object sender, EventArgs e)
        {
            // nbxThread.Value = Environment.ProcessorCount;
        }

        private void SetPbar(object o)
        {
            pbar.Value = (int)o;
        }

        private void SetPanel(object o)
        {
            pnMain.Enabled = !pnMain.Enabled;
        }

        private void BtnBrowser_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog od = new FolderBrowserDialog();

            if (od.ShowDialog(this) == DialogResult.OK)
            {
                tbxPath.Text = od.SelectedPath;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            pnMain.Enabled = false;

            if (tbxPath.Text != string.Empty)
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


                Main.PackData pack = new Main.PackData();
                List<MemoryStream> ls = new List<MemoryStream>();
                var files = Directory.GetFiles(tbxPath.Text);
                decimal count = 1;

                procTask = new Task(() =>
                {


                    foreach (var file in files)
                    {
                        var td = (count / files.Length) * 100;
                        var percent = decimal.ToInt32(td);
                        Sync.Send(SetPbar, percent);
                        var buf = Main.GetSingleBitmap(file);
                        MagickImage img = new MagickImage(buf) { Format = MagickFormat.Xbm };
                        var width = Convert.ToInt32(nbxWidth.Value);
                        var height = Convert.ToInt32(nbxHeight.Value);
                        img.Resize(new MagickGeometry($"{width}x{height}!"));
                        buf = img.ToByteArray();
                        MemoryStream ms = new MemoryStream();
                        ms.Write(buf, 0, buf.Length);
                        ls.Add(ms);
                        count++;
                    }

                    pack.img = ls;
                    MemoryStream mm = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(mm, pack);

                    var zdata = GZipStream.CompressBuffer(mm.ToArray());

                    if (File.Exists(fname))
                        File.Delete(fname);

                    FileStream fs = new FileStream(fname, FileMode.Create);
                    fs.Write(zdata, 0, zdata.Length);
                    fs.Dispose();
                    Process.Start("Explorer.exe", "/select," + fname);
                    Sync.Send(SetPanel, null);

                });
                procTask.Start();
            }
        }


        private void PackImage(object files)
        {
            foreach (var file in (string[])files)
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
            }
        }
        private void SaveFile(string fname, byte[] data)
        {
            byte[] zdata = GZipStream.CompressBuffer(data);
            if (File.Exists(fname))
                File.Delete(fname);
            FileStream fs = new FileStream(fname, FileMode.Create);
            fs.Write(zdata, 0, zdata.Length);
            fs.Dispose();
            Process.Start("Explorer.exe", "/select," + fname);
        }
        private void MutilPack(string[] arr, int splitCount)
        {
            int size = arr.Length / splitCount;
            List<string[]> ls = new List<string[]>();
            for (int i = 0; i < splitCount; i++)
            {
                int index = i * size;

                if (i == splitCount - 1)
                    size = arr.Length;

                string[] subarr = arr.Skip(index).Take(size).ToArray();
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(PackImage),subarr))
                {
                    Sync.Send(main.SetLogbox, "添加线程成功");
                }
                ls.Add(subarr);
            }
            Task updatpbar = new Task(() =>
                {
                    pbar.Value = hashtable.Count / Directory.GetFiles(tbxPath.Text).Length * 100;
                });
            updatpbar.Start();
            Task saveTask = new Task(() =>
            {
                while (true)
                {
                    if (hashtable.Count == Directory.GetFiles(tbxPath.Text).Length)
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
                        byte[] data = null;
                        List<byte[]> lb = new List<byte[]>();
                        foreach (var subarr in ls)
                        {
                            foreach (var file in subarr)
                            {
                                lb.Add((byte[])hashtable[Path.GetFileName(file)]);
                            }
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(ms, data);
                        }
                        SaveFile(fname,data);
                        break;
                    }
                }
            });
            saveTask.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            int threadcount = Convert.ToInt32(threadCount.Text);
            var files = Directory.GetFiles(tbxPath.Text);
            MutilPack(files, threadcount);
        }
    }
}
