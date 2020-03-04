using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace Aida64_Esp8266_DisplayControler
{
    class Bmp
    {
        #region 二值化

        #region Otsu阈值法二值化模块   

        /// <summary>   
        /// Otsu阈值   
        /// </summary>   
        /// <param name="b">位图流</param>   
        /// <returns></returns>   
        public static byte[] OtsuThreshold(string file)
        {
            Bitmap bitmap = new System.Drawing.Bitmap(file);
            // 图像灰度化   
            // b = Gray(b);   
            int width = bitmap.Width;
            int height = bitmap.Height;

            byte threshold = 0;
            int[] hist = new int[256];

            int AllPixelNumber = 0, PixelNumberSmall = 0, PixelNumberBig = 0;

            double MaxValue, AllSum = 0, SumSmall = 0, SumBig, ProbabilitySmall, ProbabilityBig, Probability;
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* p = (byte*)bmpData.Scan0;
                int offset = bmpData.Stride - width * 4;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        hist[p[0]]++;
                        p += 4;
                    }
                    p += offset;
                }
                bitmap.UnlockBits(bmpData);
            }
            //计算灰度为I的像素出现的概率   
            for (int i = 0; i < 256; i++)
            {
                AllSum += i * hist[i];     //   质量矩   
                AllPixelNumber += hist[i];  //  质量       
            }
            MaxValue = -1.0;
            for (int i = 0; i < 256; i++)
            {
                PixelNumberSmall += hist[i];
                PixelNumberBig = AllPixelNumber - PixelNumberSmall;
                if (PixelNumberBig == 0)
                {
                    break;
                }

                SumSmall += i * hist[i];
                SumBig = AllSum - SumSmall;
                ProbabilitySmall = SumSmall / PixelNumberSmall;
                ProbabilityBig = SumBig / PixelNumberBig;
                Probability = PixelNumberSmall * ProbabilitySmall * ProbabilitySmall + PixelNumberBig * ProbabilityBig * ProbabilityBig;
                if (Probability > MaxValue)
                {
                    MaxValue = Probability;
                    threshold = (byte)i;
                }
            }

            Threshoding(bitmap, threshold);
            bitmap = twoBit(bitmap);


            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }
        #endregion

        #region      固定阈值法二值化模块
        public static Bitmap Threshoding(Bitmap b, byte threshold)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                byte R, G, B, gray;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        R = p[2];
                        G = p[1];
                        B = p[0];
                        gray = (byte)((R * 19595 + G * 38469 + B * 7472) >> 16);
                        if (gray >= threshold)
                        {
                            p[0] = p[1] = p[2] = 255;
                        }
                        else
                        {
                            p[0] = p[1] = p[2] = 0;
                        }
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
                return b;

            }

        }
        #endregion

        #region 创建1位图像

        /// <summary>
        /// 创建1位图像
        /// </summary>
        /// <param name="srcBitmap"></param>
        /// <returns></returns>
        public static Bitmap twoBit(Bitmap srcBitmap)
        {
            int midrgb = Color.FromArgb(128, 128, 128).ToArgb();
            int stride;//简单公式((width/8)+3)&(~3)
            stride = (srcBitmap.Width % 8) == 0 ? (srcBitmap.Width / 8) : (srcBitmap.Width / 8) + 1;
            stride = (stride % 4) == 0 ? stride : ((stride / 4) + 1) * 4;
            int k = srcBitmap.Height * stride;
            byte[] buf = new byte[k];
            int x = 0, ab = 0;
            for (int j = 0; j < srcBitmap.Height; j++)
            {
                k = j * stride;//因图像宽度不同、有的可能有填充字节需要跳越
                x = 0;
                ab = 0;
                for (int i = 0; i < srcBitmap.Width; i++)
                {
                    //从灰度变单色（下法如果直接从彩色变单色效果不太好，不过反相也可以在这里控制）
                    if ((srcBitmap.GetPixel(i, j)).ToArgb() > midrgb)
                    {
                        ab = ab * 2 + 1;
                    }
                    else
                    {
                        ab = ab * 2;
                    }
                    x++;
                    if (x == 8)
                    {
                        buf[k++] = (byte)ab;//每字节赋值一次，数组buf中存储的是十进制。
                        ab = 0;
                        x = 0;
                    }
                }
                if (x > 0)
                {
                    //循环实现：剩余有效数据不满1字节的情况下须把它们移往字节的高位部分
                    for (int t = x; t < 8; t++) ab = ab * 2;
                    buf[k++] = (byte)ab;
                }
            }
            int width = srcBitmap.Width;
            int height = srcBitmap.Height;
            Bitmap dstBitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);
            BitmapData dt = dstBitmap.LockBits(new Rectangle(0, 0, dstBitmap.Width, dstBitmap.Height), ImageLockMode.ReadWrite, dstBitmap.PixelFormat);
            Marshal.Copy(buf, 0, dt.Scan0, buf.Length);
            dstBitmap.UnlockBits(dt);
            return dstBitmap;
        }

        #endregion

        #endregion
    }
}
