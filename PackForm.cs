using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
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
            this.Sync = SynchronizationContext.Current;
            this.main = main;
        }

        private void PackForm_Load(object sender, EventArgs e)
        {
            // nbxThread.Value = Environment.ProcessorCount;
        }

        private void setPbar(object o)
        {
            this.pbar.Value = (int)o;
        }

        private void setPanel(object o)
        {
            this.pnMain.Enabled = !this.pnMain.Enabled;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog od = new FolderBrowserDialog();

            if (od.ShowDialog(this) == DialogResult.OK)
            {
                tbxPath.Text = od.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            pnMain.Enabled = false;

            if (tbxPath.Text != string.Empty)
            {
                string fname;
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "PackFile(*.dat)|*.dat";
                sd.Title = "请输入保存文件名";

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
                        Sync.Send(setPbar, percent);

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
                    Sync.Send(setPanel, null);

                });


                procTask.Start();

            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }


    }
}
