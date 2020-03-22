using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace Aida64_Esp8266_DisplayControler
{
    public class CtrPack
    {
        //CPK
        private readonly byte[] MARK = { 0x43, 0x50, 0x4B, 0x0 };
        private const ushort VER = 0x1;
        private const ushort HEADER_DEFAULT_RESERVE = 0xFFF;
        private const ushort FILE_TYPE_DELETED = 0x0;
        private const ushort FILE_TYPE_FILE = 0x1;
        private const ushort FILE_TYPE_DIRECTORY = 0x2;
        private string _fname;
        private FileStream _file;
        private bool _newFile;
        private long _metaOffset;
        private long _metaLen;
        private ushort _describeOffset;
        private int _describeLen;
        private int _headerLen;

        protected UInt32[] Crc32Table;

        public string LastError { get; private set; }
        public ushort Version { get; private set; }
        public ushort Encrypt { get; private set; }
        public ushort Compress { get; private set; }
        public string Author { get; private set; }
        public string Describe { get; private set; }

       
        //文件头
        public struct Header
        {
            //类型标记 固定为 CPK
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] mark;
            //版本
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] version;
            //加密类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] encrypt;
            //压缩类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] compress;
            //meta索引偏移
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] metaOffset;
            ///meta索引长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] metaLen;
            //作者
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] author;
            //保留
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] reserve;
            //描述偏移
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] describeOffset;
            //描述长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] describeLen;
        }

        public struct Meta
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            //文件类型 0 已删除 1文件 2 文件夹
            public byte[] type;
            //文件唯一ID
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] guid;
            //文件父文件夹唯一ID
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] parent;
            //CRC32
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] crc32;
            //文件分配大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] allocSize;
            //文件实际大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] realSize;
            //是否存在下个meta
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] hasNext;
            //文件名长度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] nameLen;
        }

        public CtrPack(string fname, byte encrypt = 0x0, byte compress = 0x0, string author = "unknow", string describe = null)
        {
            GetCRC32Table();
            this._fname = fname;
            Encrypt = encrypt;
            Compress = compress;
            Author = author;
            Describe = describe;
            this._file = File.Open(this._fname, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        #region CRC32
        public void GetCRC32Table()
        {
            UInt32 Crc;
            Crc32Table = new UInt32[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (UInt32)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                Crc32Table[i] = Crc;
            }
        }
        
        public UInt32 genCRC32(byte[] data)
        {
            UInt32 value = 0xffffffff;
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ data[i]];
            }
            return value ^ 0xffffffff;
        }
        #endregion

        #region Struct convert

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }


        public static object BytesToStruct(byte[] bytes, Type strType)
        {
            int size = Marshal.SizeOf(strType);

            if (size > bytes.Length)
            {
                return null;
            }
        
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, strPtr, size);
            object obj = Marshal.PtrToStructure(strPtr, strType);
            Marshal.FreeHGlobal(strPtr);
            return obj;
        }


        public StructType ConverBytesToStructure<StructType>(byte[] bytesBuffer)
        {
  
            if (bytesBuffer.Length != Marshal.SizeOf(typeof(StructType)))
            {
                throw new ArgumentException("bytesBuffer参数和structObject参数字节长度不一致。");
            }

            IntPtr bufferHandler = Marshal.AllocHGlobal(bytesBuffer.Length);

            for (int index = 0; index < bytesBuffer.Length; index++)
            {
                Marshal.WriteByte(bufferHandler, index, bytesBuffer[index]);
            }

            StructType structObject = (StructType)Marshal.PtrToStructure(bufferHandler, typeof(StructType));
            Marshal.FreeHGlobal(bufferHandler);
            return structObject;
        }
        
        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        #endregion

    

        private bool moveMeta()
        {
            int i = 0, rlen = 0;
            var buf = new byte[4096];
            long newMetaOffset = _file.Length - _metaLen;
            try
            {
                do
                {
                    _file.Seek(_metaOffset + i * 4096, SeekOrigin.Begin);
                    rlen = _file.Read(buf, 0, 4096);
                    _file.Seek(newMetaOffset + i * 4096, SeekOrigin.Begin);
                    _file.Write(buf, 0, rlen);
                    i++;
                } while (rlen > 0);

                
                _file.Seek(0xA, SeekOrigin.Begin);
                _file.Write(BitConverter.GetBytes(newMetaOffset), 0, 8);
                _metaOffset = newMetaOffset;
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return false;
            }

            return true;
        }

        private long allocFile(long size)
        {
            size += _metaLen;
            size = size % 4096 == 0 ? size : (size / 4096 + 1) * 4096;

            try
            {
                _file.SetLength(_file.Length + size);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return 0;
            }
            finally
            {
                _file.Close();
            }

            return size;
        }


        private bool genMeta(ushort type, string abspath, MemoryStream ms)
        {
            try
            {
                abspath = abspath.Replace(@"\\", @"\").Replace("//", "/");
                var meta = new Meta();
                meta.guid = Guid.NewGuid().ToByteArray();

                if (type == FILE_TYPE_FILE)
                {
        
            
                
                        meta.realSize = BitConverter.GetBytes(ms.Length);
                         meta.crc32 = BitConverter.GetBytes(genCRC32(ms.ToArray()));

     
                }

                

            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
            finally
            {
                _file.Close();
            }

            return true;
        }

        //初始化文件
        public void initFile()
        {
            _file.Seek(0, SeekOrigin.Begin);
            int describeLen = Describe == null ? 0 : Describe.Length * 2;
            Header h = new Header();
            h.mark = MARK;
            h.version = BitConverter.GetBytes(VER);
            h.encrypt = BitConverter.GetBytes(Encrypt);
            h.compress = BitConverter.GetBytes(Compress);
            h.metaOffset = BitConverter.GetBytes((ulong)(0));
            h.metaLen = BitConverter.GetBytes((ulong)(0));
            h.author = Encoding.Unicode.GetBytes(Author.PadRight(32, '\0').ToCharArray());
            h.reserve = Encoding.ASCII.GetBytes("".PadRight(32, '\0').ToCharArray());
            h.describeOffset = describeLen > 0 ? BitConverter.GetBytes((ushort)(Marshal.SizeOf(typeof(Header)) + HEADER_DEFAULT_RESERVE)) : BitConverter.GetBytes((ushort)0x0);
            h.describeLen = BitConverter.GetBytes(describeLen);
            byte[] ba = StructToBytes(h);
            _file.Write(ba, 0, ba.Length);
            _file.Write(new byte[HEADER_DEFAULT_RESERVE], 0, HEADER_DEFAULT_RESERVE);
            if (Describe != null)
                _file.Write(Encoding.Unicode.GetBytes(Describe.ToCharArray()), 0, describeLen);

            _file.Flush();
        }

        //解析文件
        public bool parseFile()
        {
            try
            {
               
                _file.Seek(0, SeekOrigin.Begin);
                byte[] buf = new byte[4];
                //标记
                _file.Read(buf, 0, 4);
                if (!Enumerable.SequenceEqual(buf, MARK))
                    return false;

                buf = new byte[2];
                //版本
                _file.Read(buf, 0, 2);
                Version = BitConverter.ToUInt16(buf, 0);
                //加密
                _file.Read(buf, 0, 2);
                Encrypt = BitConverter.ToUInt16(buf, 0);
                //压缩
                _file.Read(buf, 0, 2);
                Compress = BitConverter.ToUInt16(buf, 0);
                //meta索引偏移
                buf = new byte[8];
                _file.Read(buf, 0, 8);
                _metaOffset = BitConverter.ToInt64(buf, 0);
                //meta索引长度
                _file.Read(buf, 0, 8);
                _metaLen = BitConverter.ToInt64(buf, 0);
                //作者
                buf = new byte[64];
                _file.Read(buf, 0, 64);
                Author = Encoding.Unicode.GetString(buf);
                //保留
                _file.Seek(24, SeekOrigin.Current);
                //描述偏移
                buf = new byte[2];
                _file.Read(buf, 0, 2);
                _describeOffset = BitConverter.ToUInt16(buf, 0);

                if (this._describeOffset > 0)
                {
                    //描述长度
                    buf = new byte[4];
                    _file.Read(buf, 0, 4);
                    _describeLen = BitConverter.ToInt32(buf, 0);
                    //描述
                    _file.Seek(this._describeOffset, SeekOrigin.Begin);
                    buf = new byte[this._describeLen];
                    _file.Read(buf, 0, this._describeLen);

                    if (_describeLen > 0)
                        Describe = Encoding.Unicode.GetString(buf);
                }
  

                //文件头长度
                _headerLen = (Marshal.SizeOf(typeof(Header)) + this._describeLen + HEADER_DEFAULT_RESERVE);
                //是否为新文件
                _newFile = this._metaOffset == 0 ? true : false;


            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
            finally
            {
                _file.Close();
            }

            return true;
        }


        public bool addFile(string fname)
        {
            try
            {
                if (!File.Exists(fname))
                    return false;

                FileInfo fi = new FileInfo(fname);

                if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var files = Directory.GetFiles(fname, "*.*", SearchOption.AllDirectories);
                    return addFile(files);
                }
 

            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
            finally
            {
                _file.Close();
            }


            return true;
        }

        public bool addFile(string[] files)
        {
            return true;
        }





    }
}
