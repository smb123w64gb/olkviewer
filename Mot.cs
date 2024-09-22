using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class Mot
    {
        public class Header
        {
            public Header()
            {
                Unk0 = 0;
                Count = 0;
                Date = new char[0x20];
                Unk1 = 0;
                Unk2 = 0;
                Count2 = 0;
                Unk3 = 0;
                EntriesOffset = 0;
                DataOffset = 0;
                DataOffset2 = 0;
                DataOffset3 = 0;
                DataOffset4 = 0;
                Unk4 = 0;
                Unk5 = 0;
                Unk6 = 0;
                Unk7 = 0;
            }

            public Header(BinaryReader br)
            {
                Unk0 = br.ReadUInt16();
                Count = br.ReadUInt16();
                Date = br.ReadChars(0x20);
                Unk1 = br.ReadUInt32();
                Unk2 = br.ReadUInt16();
                Count2 = br.ReadUInt16();
                Unk3 = br.ReadUInt32();
                EntriesOffset = br.ReadUInt32();
                DataOffset = br.ReadUInt32();
                DataOffset2 = br.ReadUInt32();
                DataOffset3 = br.ReadUInt32();
                DataOffset4 = br.ReadUInt32();
                Unk4 = br.ReadUInt32();
                Unk5 = br.ReadUInt32();
                Unk6 = br.ReadUInt16();
                Unk7 = br.ReadUInt16();
            }

            public ushort Unk0 { get; }
            public ushort Count { get; }
            public char[] Date { get; }
            public uint Unk1 { get; }
            public ushort Unk2 { get; }
            public ushort Count2 { get; }
            public uint Unk3 { get; }
            public uint EntriesOffset { get; }
            public uint DataOffset { get; }
            public uint DataOffset2 { get; }
            public uint DataOffset3 { get; }
            public uint DataOffset4 { get; }
            public uint Unk4 { get; }
            public uint Unk5 { get; }
            public ushort Unk6 { get; }
            public ushort Unk7 { get; }

        }

        public class Entry
        {
            public Entry()
            {
                MotId = 0;
                _p0 = new byte[0x4E];
                DataPtr = 0;
                Data2Ptr = 0;
                Data3Ptr = 0;
                Unk1 = 0;
                _p1 = new byte[4];
                dOffset = 0;
            }

            public Entry(BinaryReader br)
            {
                MotId = br.ReadUInt16();
                _p0 = br.ReadBytes(0x4E);
                DataPtr = br.ReadUInt32();
                Data2Ptr = br.ReadUInt32();
                Data3Ptr = br.ReadUInt16();
                Unk1 = br.ReadUInt16();
                _p1 = br.ReadBytes(4);
                dOffset = 0;
            }

            public ushort MotId;
            public byte[] _p0; // 0x4E
            public uint DataPtr;
            public uint Data2Ptr;
            public ushort Data3Ptr;
            public ushort Unk1;
            public byte[] _p1; // 4

            public uint dOffset;
        }
    }
}
