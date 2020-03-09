using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Aida64_Esp8266_DisplayControler
{

    public class Serial
    {
        #region 初始化


        public SerialPort _serialPort = new SerialPort();


        public static string[] getSerialPort()
        {
            //create vars for testing
            bool _available = false;
            SerialPort _tempPort;
            String[] portNames = SerialPort.GetPortNames();

            return portNames;

            /*

            //create a loop for each string in SerialPort.GetPortNames
            foreach (string str in portNames)
            {
                try
                {
                    _tempPort = new SerialPort(str);
                    _tempPort.Open();

                    if (_tempPort.IsOpen)
                    {
                        //comboBox1.Items.Add(str);
                        _tempPort.Close();
                        _available = true;
                    }
                }

                //else we have no ports or can't open them display the 
                //precise error of why we either don't have ports or can't open them
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error - No Ports available", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _available = false;
                }
            }

            //return the temp bool
            return portNames;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="baudRate">波特率</param>
        public Serial(string portName, int baudRate)
        {
            // 串口名  
            _serialPort.PortName = portName;
            // 波特率  
            _serialPort.BaudRate = baudRate;
            // 数据位  
            _serialPort.DataBits = 8;
            // 停止位  
            _serialPort.StopBits = StopBits.One;
            // 无奇偶校验位  
            _serialPort.Parity = Parity.None;
            _serialPort.Encoding = Encoding.ASCII;
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        #endregion


        #region Public   


        /// <summary>  
        /// 是否处于打开状态  
        /// </summary>  
        public bool IsOpen
        {
            get { return _serialPort != null && _serialPort.IsOpen; }
        }


        /// <summary>  
        /// 打开串口  
        /// </summary>  
        /// <returns></returns>  
        public bool Open()
        {
            try
            {

                if (_serialPort == null)
                    return this.IsOpen;



                if (_serialPort.IsOpen)
                    this.Close();


                _serialPort.Open();


            }
            catch (Exception e)
            {
                _serialPort.Close();
            }

            return this.IsOpen;
        }


        /// <summary>  
        /// 关闭串口  
        /// </summary>  
        public void Close()
        {
            if (this.IsOpen)
                _serialPort.Close();
        }


        /// <summary>  
        /// 向串口内写入  
        /// </summary>  
        /// <param name="send">写入数据</param>  
        /// <param name="offSet">偏移量</param>  
        /// <param name="count">写入数量</param>  
        public void Write(byte[] send, int offSet, int count)
        {
            if (this.IsOpen)
            {
                _serialPort.Write(send, offSet, count);
            }
        }


        public void Dispose()
        {
            if (this._serialPort == null)
                return;
            if (this._serialPort.IsOpen)
                this.Close();
            this._serialPort.Dispose();
            this._serialPort = null;
        }


        #endregion

        void _serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // 等待100ms，防止读取不全的情况  
            //Thread.Sleep(100);


            ReceiveDate();

        }


        //public event EventHandler<SerialSortEventArgs> DataReceived;

        public void ReceiveDate()
        {
            byte[] m_recvBytes = new byte[_serialPort.BytesToRead]; //定义缓冲区大小  
            int result = _serialPort.Read(m_recvBytes, 0, m_recvBytes.Length); //从串口读取数据  
            if (result <= 0)
                return;
            string strResult = Encoding.UTF8.GetString(m_recvBytes, 0, m_recvBytes.Length); //对数据进行转换  
            _serialPort.DiscardInBuffer();


            //if (this.DataReceived != null)
            //this.DataReceived(this, new SerialSortEventArgs() { Code = strResult });
        }






    }

}