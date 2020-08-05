using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using OpenHardwareMonitor.Hardware;

namespace Aida64_Esp8266_DisplayControler
{
    public class HardInfo : IVisitor
    {
        public float FreeRAM;
        public float RAMLoad;
        public float FreeVRAM;
        public float VRAMLoad;
        public List<float> CPUClock = new List<float>();
        public float CPULoad;
        public float CPUTemp;
        public float NCPUPower = -1;
        public float NGPUClock = -1;
        public float NGPULoad = -1;
        public float NGPUTemp = -1;
        public int NGPUFan = -1;
        public float AGPUClock = -1;
        public float AGPULoad = -1;
        public float AGPUPower = -1;
        public float AGPUFan = -1;
        public List<float> HDDtemp = new List<float>();
        public List<float> HDDLoad = new List<float>();

        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
        Computer computer = new Computer();
        public void GetHardInfo(HardInfo hardInfo)
        {
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;
            computer.HDDEnabled = true;
            computer.MainboardEnabled = true;
            computer.FanControllerEnabled = true;
            computer.Accept(this);
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        for (int i = 1; i < Environment.ProcessorCount; i++)
                        {
                            if (sensor.Name == "CPU Core #" + i && sensor.SensorType == SensorType.Clock)
                            {
                                hardInfo.CPUClock.Add((float)sensor.Value);
                                continue;
                            }
                        }
                        if (sensor.Name == "CPU Total" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.CPULoad = (float)sensor.Value;
                            continue;
                        }
                        if (sensor.Name == "CPU Package" && sensor.SensorType == SensorType.Power)
                        {
                            hardInfo.NCPUPower = (float)sensor.Value;
                            continue;
                        }
                        if (sensor.Name == "CPU Package" && sensor.SensorType == SensorType.Temperature)
                        {
                            hardInfo.CPUTemp = (float)sensor.Value;
                        }
                    }
                }
                if (hardware.HardwareType == HardwareType.RAM)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.Name == "Memory" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.RAMLoad = (float)sensor.Value;
                            continue;
                        }
                        if (sensor.Name == "Available Memory" && sensor.SensorType == SensorType.Data)
                        {
                            hardInfo.FreeRAM = (float)sensor.Value;
                        }
                    }
                }
                if (hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.Name == "GPU Core " && sensor.SensorType == SensorType.Temperature)
                        {
                            hardInfo.NGPUTemp = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Core " && sensor.SensorType == SensorType.Clock)
                        {
                            hardInfo.NGPUClock = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Core" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.NGPULoad = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU " && sensor.SensorType == SensorType.Fan)
                        {
                            hardInfo.NGPUFan = (int)sensor.Value;
                        }
                        if (sensor.Name == "GPU Memory" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.VRAMLoad = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Memory Free " && sensor.SensorType == SensorType.SmallData)
                        {
                            hardInfo.FreeVRAM = (float)sensor.Value / 1024;
                        }
                    }
                }
                if (hardware.HardwareType == HardwareType.GpuAti)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.Name == "GPU Core " && sensor.SensorType == SensorType.Clock)
                        {
                            hardInfo.AGPUClock = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Core" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.AGPULoad = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Fan" && sensor.SensorType == SensorType.Fan)
                        {
                            hardInfo.AGPUFan = (float)sensor.Value;
                        }
                        if (sensor.Name == "GPU Total" && sensor.SensorType == SensorType.Power)
                        {
                            hardInfo.AGPUPower = (float)sensor.Value;
                        }
                    }
                }
                if (hardware.HardwareType == HardwareType.HDD)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.Name == "Temperature" && sensor.SensorType == SensorType.Temperature)
                        {
                            hardInfo.HDDtemp.Add((float)sensor.Value);
                        }
                        if (sensor.Name == "Used Space" && sensor.SensorType == SensorType.Load)
                        {
                            hardInfo.HDDLoad.Add((float)sensor.Value);
                        }
                    }
                }
            }
        }
    }
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
