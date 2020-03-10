using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace Aida64_Esp8266_DisplayControler
{
    public partial class Main : Form
    {
        //Network
        const byte PACKET_ALIVE = 0X0;
        const byte PACKET_OK = 0X1;
        const byte PACKET_FAIL = 0X2;
        const byte PACKET_DEBUG = 0X4;
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

    }
}
