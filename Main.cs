using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                IEnumerable<XElement> allEnumerator = xmldoc.Element("AIDA").Elements("sys");
                foreach (var element in allEnumerator)
                {
                    logBox.AppendText("name:" + element.Element("id").Value + "value:" + element.Element("value").Value + Environment.NewLine);
                    id.Add(element.Element("id").Value);
                    value.Add(element.Element("value").Value);
                }
                logBox.AppendText(id.Count.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("请开启AIDA64内存共享功能,并保持AIDA64后台运行");
            }

        }
        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetAidaInfo();
        }
    }
}
