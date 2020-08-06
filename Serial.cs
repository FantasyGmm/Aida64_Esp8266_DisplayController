using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Aida64_Esp8266_DisplayControler
{
    public class Serial
    {
        private SerialPort _serial;
        public string LastError { get; private set; }
        public bool IsOpen { get; private set; }
        public Serial(string portname, SerialDataReceivedEventHandler handler)
        {
            IsOpen = false;
            _serial = new SerialPort(portname);
            _serial.BaudRate = 115200;
            _serial.DataBits = 8;
            _serial.Parity = Parity.None;
            _serial.StopBits = StopBits.One;
            _serial.Handshake = Handshake.None;
            _serial.ReceivedBytesThreshold = 13;
            _serial.RtsEnable = false;
            _serial.DtrEnable = false;
            _serial.DataReceived += new SerialDataReceivedEventHandler(handler);
        }


        public bool Open()
        {
            try
            {
                _serial.Open();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }

            IsOpen = true;
            return true;
        }


        public void SendText(string str)
        {
            if (_serial.IsOpen)
            {
                byte[] ba = Encoding.UTF8.GetBytes(str);
                _serial.Write(ba, 0, ba.Length);
            }
        }



    }



}
