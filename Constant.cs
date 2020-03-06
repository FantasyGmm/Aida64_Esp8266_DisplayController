using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms
    ;
namespace Aida64_Esp8266_DisplayControler
{
    public partial class Main : Form
    {
        //Network
        const byte PACKET_ALIVE = 0X0;
        const byte PACKET_OK = 0X1;
        const byte PACKET_FAIL = 0X2;
        const byte PACKET_DISPLAY_IMG = 0XF;
        const byte PACKET_DISPLAY_INFO = 0X10;
        const byte PACKET_GET_INFO = 0X11;
        const byte PACKET_TOGGLE_LED = 0X12;
        const byte PACKET_TOGGLE_DISPLAY = 0X13;
        const byte PACKET_REBOOT = 0X14;


        //UI
        const UInt32 UI_TEMP_CPU = 0X1;
        const UInt32 UI_TEMP_BOARD = 0X2;
        const UInt32 UI_TEMP_GPU = 0X4;
        const UInt32 UI_TEMP_HDD = 0X8;
        const UInt32 UI_USE_CPU = 0X10;
        const UInt32 UI_USE_RAM = 0X20;
        const UInt32 UI_USE_GPU = 0X40;
        const UInt32 UI_USE_VRAM = 0X80;
        const UInt32 UI_RATE_CPU = 0X100;
        const UInt32 UI_RATE_GPU = 0X200;
        const UInt32 UI_SPEED_CPU = 0X400;
        const UInt32 UI_SPEED_GPU = 0X800;
        const UInt32 UI_POWER_CPU = 0X1000;
        const UInt32 UI_POWER_GPU = 0X2000;

    }
}
