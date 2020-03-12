using System;
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

        private List<string[]> SplitAry(string[] arr, int splitCount)
        {
            int size = arr.Length / splitCount;
            if (arr.Length % splitCount !=0)
            {

                splitCount++;
            }
            List<string[]> splitList = new List<string[]>();
            for (int i = 0; i < splitCount; i++)
            {
                int index = i * size;
                string[] subarr = arr.Skip(index).Take(size).ToArray();
                splitList.Add(subarr);
            }
            return splitList;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(tbxPath.Text);
            int count = 0;
            foreach (var arry in SplitAry(files, Convert.ToInt32(threadCount.Text)))
            {
                count += arry.Length;
                main.logBox.AppendText(count + Environment.NewLine);
            }
            List<string[]> spitList = SplitAry(files, Convert.ToInt32(threadCount.Text));
        }
    }
}
