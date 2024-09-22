using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace olkviewer
{
    public static class Helper
    {
        public static ushort BS16U(ushort Value)
        {
            ushort b0 = (ushort)((Value & 0x00FF) << 8);
            ushort b1 = (ushort)((Value & 0xFF00) >> 8);

            ushort val = (ushort)(b0 | b1);
            return val;
        }

        #region WindViewer
        public static void Swap(ref byte V1, ref byte V2)
        {
            byte Tmp = V1; V1 = V2; V2 = Tmp;
        }

        public static void Swap(ref int V1, ref int V2)
        {
            int Tmp = V1; V1 = V2; V2 = Tmp;
        }

        public static byte Read8(byte[] Data, int Offset)
        {
            return (Buffer.GetByte(Data, Offset));
        }

        public static ushort Read16(byte[] Data, int Offset)
        {
            return (ushort)((Buffer.GetByte(Data, Offset) << 8) | Buffer.GetByte(Data, Offset + 1));
        }
        public static uint Read32(byte[] Data, int Offset)
        {
            return (uint)((Buffer.GetByte(Data, Offset) << 24) | (Buffer.GetByte(Data, Offset + 1) << 16) | (Buffer.GetByte(Data, Offset + 2) << 8) | Buffer.GetByte(Data, Offset + 3));
        }

        public static ushort Read16Swap(byte[] Data, int Offset)
        {
            return (ushort)((Buffer.GetByte(Data, Offset + 1) << 8) | Buffer.GetByte(Data, Offset));
        }

        public static uint Read32Swap(byte[] Data, int Offset)
        {
            return (uint)((Buffer.GetByte(Data, Offset + 3) << 24) | (Buffer.GetByte(Data, Offset + 2) << 16) | (Buffer.GetByte(Data, Offset + 1) << 8) | Buffer.GetByte(Data, Offset));
        }
        #endregion

        public static uint swap32(uint val)
        {
            uint b0 = ((val & 0x000000FF) << 24);
            uint b1 = ((val & 0x0000FF00) << 8);
            uint b2 = ((val & 0x00FF0000) >> 8);
            uint b3 = ((val & 0xFF000000) >> 24);

            uint val2 = b0 | b1 | b2 | b3;
            return val2;
        }

        public static byte swapByte(BinaryReader br)
        {
            byte val = br.ReadByte();

            int val2 = val;

            int v0 = ((val2 & 0x33) << 2) | ((val2 & 0xCC) >> 2);
            int v1 = ((v0 & 0xF) << 4) | ((v0 & 0xF0) >> 4);

            byte b0 = (byte)(v1 & 0xFF);

            return b0;
        }

        public static short readInt16B(BinaryReader br)
        {
            ushort val = br.ReadUInt16();

            ushort b0 = (ushort)((val & 0x00FF) << 8);
            ushort b1 = (ushort)((val & 0xFF00) >> 8);

            val = (ushort)(b0 | b1);
            short val2 = (short)val;
            return val2;
        }

        public static ushort readUInt16B(BinaryReader br)
        {
            ushort val = br.ReadUInt16();

            ushort b0 = (ushort)((val & 0x00FF) << 8);
            ushort b1 = (ushort)((val & 0xFF00) >> 8);

            val = (ushort)(b0 | b1);
            return val;
        }


        public static int readInt32B(BinaryReader br)
        {
            uint val = br.ReadUInt32();

            uint b0 = ((val & 0x000000FF) << 24);
            uint b1 = ((val & 0x0000FF00) << 8);
            uint b2 = ((val & 0x00FF0000) >> 8);
            uint b3 = ((val & 0xFF000000) >> 24);

            int val2 = (int)(b0 | b1 | b2 | b3);
            return val2;
        }

        public static uint readUInt32B(BinaryReader br)
        {
            uint val = br.ReadUInt32();

            uint b0 = ((val & 0x000000FF) << 24);
            uint b1 = ((val & 0x0000FF00) << 8);
            uint b2 = ((val & 0x00FF0000) >> 8);
            uint b3 = ((val & 0xFF000000) >> 24);

            val = b0 | b1 | b2 | b3;
            return val;
        }

        public static double readDoubleL(BinaryReader br)
        {
            double val = br.ReadDouble();
            return val;
        }

        public static float readFloatL(BinaryReader br)
        {
            float val = br.ReadSingle();
            return val;
        }

        public static uint readUInt32L(BinaryReader br)
        {
            uint val = br.ReadUInt32();
            return val;
        }

        public static int readInt32L(BinaryReader br)
        {
            int val = br.ReadInt32();
            return val;
        }
    }
}
