using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class Mmg
    {

        public class Header
        {

            public Header(BinaryReader br)
            {
                Magic = Helper.readUInt32B(br);
                Size = Helper.readUInt32B(br);
                Unk1 = br.ReadByte();
                Unk2 = br.ReadByte();
                Unk3 = Helper.readUInt16B(br);
                Pad0 = br.ReadBytes(4);
                Unk5 = Helper.readUInt32B(br);
                Count = Helper.readInt32B(br);
            }

            public uint  Magic { get; } // 4
            public uint    Size { get; }
            //ushort  Unk0 { get; }
            public byte    Unk1 { get; }
            public byte    Unk2 { get; }
            public ushort  Unk3 { get; }
            public byte[]  Pad0 { get; } // 4
            public uint     Unk5 { get; }
            public int Count { get; }
        }

        public class Entry
        {
            public Entry(BinaryReader br)
            {
                HeaderOffset = Helper.readUInt32B(br);
                HeaderSize = Helper.readUInt32B(br);
                DataOffset = Helper.readUInt32B(br);
                DataSize = Helper.readUInt32B(br);
            }

            public uint    HeaderOffset { get; }
            public uint    HeaderSize { get; }
            public uint    DataOffset { get; }
            public uint DataSize { get; }

        }

        public class ChdpHeader
        {
            public ChdpHeader(BinaryReader br)
            {
                Magic = Helper.readUInt32B(br);
                U0 = br.ReadBytes(4);
                Count = Helper.readUInt16B(br);
                Unk1 = Helper.readUInt16B(br);
                U1 = br.ReadBytes(4);

            }
            
            public uint  Magic { get; }
            public byte[]  U0 { get; } // 4
            public ushort  Count { get; }
            public ushort  Unk1 { get; }
            public byte[]  U1 { get; } // 4

        }

        public class ChdpEntry
        {
            public ChdpEntry(BinaryReader br)
            {
                Offset = Helper.readUInt32B(br);
                U0 = br.ReadBytes(0x4C);
                dspSampleCount = Helper.readInt32B(br);
                dspNibbleCount = Helper.readInt32B(br);
                AdpcmHeader = br.ReadBytes(0x58);
            }
            
            public uint Offset { get; }
            public byte[] U0 { get; } // 0x4C

            public int dspSampleCount; // TODO: don't be dumb and make proper structs?
            public int dspNibbleCount;
            public byte[] AdpcmHeader { get; } // 0x58

            public int dHOffset { get; set; }
            public int dOffset { get; set; }
            public int dSize { get; set; }
        }

    }
}
