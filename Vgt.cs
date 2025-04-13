using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace olkviewer
{
    class Vgt
    {

		public enum Type : byte
		{
			I4 = 0x0,
			I8 = 0x1,
			IA4 = 0x2,
			IA8 = 0x3,
			RGB565 = 0x4,
			RGB5A3 = 0x5,
			RGBA8 = 0x6,
			C4 = 0x8,
			C8 = 0x9,
			C14X2 = 0xA,
			CMPR = 0xE,
			XFB = 0xF,
			UNK = 0x10
		}

		public enum PaletteType : byte
        {
			IA8,
			RGB565,
			RGBA5A3
		}

		public class Header
		{

			// Constructor that takes no arguments:
			public Header()
			{
				Magic = 0;
				Unk0 = 0;
				Count = 0;
				Unk1 = 0;
				Offset = 0;
				Size = 0;

				dOffset = 0;
			}

			// Constructor that takes one argument:
			public Header(BinaryReader br)
			{
				Magic = Helper.readUInt32B(br);
				Unk0 = Helper.readUInt32L(br);
				Count = Helper.readUInt32B(br);
				Unk1 = Helper.readUInt32L(br);
				Offset = Helper.readUInt32B(br);
				Size = Helper.readUInt32B(br);

				dOffset = 0;
			}

			// Auto-implemented readonly property:
			public uint Magic { get; }
			public uint Unk0 { get; }
			public uint Count { get; }
			public uint Unk1 { get; }
			public uint Offset { get; }
			public uint Size { get; }
			public long dOffset { get; set; } // Texture header offset in file
		}

		public class Entry
		{
			public enum EType : byte
            {
				I4 = 0x0,
				I8 = 0x1,
				IA4 = 0x2,
				IA8 = 0x3,
				RGB565 = 0x4,
				RGB5A3 = 0x5,
				RGBA8 = 0x6,
				C4 = 0x8,
				C8 = 0x9,
				C14X2 = 0xA,
				CMPR = 0xE,
				UNK = 0xF
			}

			// Constructor that takes no arguments:
			public Entry()
			{
				Unk0 = 0;

				Unk1 = 0;
				Unk2 = 0; // 0x1000 = has mipmap?
				Unk3 = 0;
				Unk4 = 0;
				Unk5 = 0;
				Offset1 = 0;
				Unk6 = 0;
				Unk7 = 0;
				Type = 0;
				Unk9 = 0;
				UnkA = 0;
				UnkB = 0;

				Unk11 = 0;
				Unk12 = 0; // 0x1000 = has mipmap?
				Unk13 = 0;
				Unk14 = 0;
				Unk15 = 0;
				Offset2 = 0;
				Unk16 = 0;
				Unk17 = 0;
				Unk18 = 0;
				Unk19 = 0;
				Unk1A = 0;
				Unk1B = 0;

				dX = 0;
				dY = 0;
				dType = 0;
				dOffset = 0;
				dOffset2 = 0;
				dMipCount = 0;

			}

			// Constructor that takes one argument:
			public Entry(BinaryReader br)
			{
				/* 0x0000 */ Unk0 = Helper.readUInt32B(br); // TexTLUT?

				/* First Image */
				/* 0x0004 */ Unk1 = Helper.readUInt32B(br);
				/* 0x0008 */ Unk2 = Helper.readUInt16B(br);
				/* 0x000A */ Unk3 = Helper.readUInt16B(br);
				/* 0x000C */ Unk4 = Helper.readUInt32B(br); // TexImage0 in dolphin source
				/* 0x000E */ //Unk5 = Helper.readUInt16B(br);
				/* 0x0010 */ Offset1 = Helper.readUInt32B(br);
				/* 0x0014 */ Unk6 = Helper.readUInt16B(br); // palette format?
				/* 0x0016 */ Unk7 = Helper.readUInt16B(br);
				/* 0x0018 */ Type = Helper.readUInt32B(br);
				/* 0x001C */ Unk9 = Helper.readUInt32B(br);
				/* 0x0020 */ UnkA = Helper.readUInt16B(br); // width/height related?
				/* 0x0022 */ UnkB = Helper.readUInt16B(br); // width/height related?

				/* Second Image */
				/* 0x0024 */ Unk11 = Helper.readUInt32B(br);
				/* 0x0028 */ Unk12 = Helper.readUInt16B(br);
				/* 0x002A */ Unk13 = Helper.readUInt16B(br);
				/* 0x002C */ Unk14 = Helper.readUInt16B(br);
				/* 0x002E */ Unk15 = Helper.readUInt16B(br);
				/* 0x0030 */ Offset2 = Helper.readUInt32B(br);
				/* 0x0034 */ Unk16 = Helper.readUInt16B(br);
				/* 0x0036 */ Unk17 = Helper.readUInt16B(br);
				/* 0x0038 */ Unk18 = Helper.readUInt32B(br);
				/* 0x003C */ Unk19 = Helper.readUInt32B(br);
				/* 0x0040 */ Unk1A = Helper.readUInt16B(br);
				/* 0x0042 */ Unk1B = Helper.readUInt16B(br);

				//size = 0x44
			}

			// Auto-implemented readonly property:
			public uint Unk0 { get; }
			public uint Unk1 { get; }
			public ushort Unk2 { get; }
			public ushort Unk3 { get; }
			public uint Unk4 { get; }
			public ushort Unk5 { get; }
			public uint Offset1 { get; } // Texture Offset
			public ushort Unk6 { get; }
			public ushort Unk7 { get; }
			public uint Type { get; }
			public uint Unk9 { get; }
			public ushort UnkA { get; }
			public ushort UnkB { get; }
			public uint Unk11 { get; }
			public ushort Unk12 { get; }
			public ushort Unk13 { get; }
			public ushort Unk14 { get; }
			public ushort Unk15 { get; }
			public uint Offset2 { get; } // Texture Alpha Map Offset
			public ushort Unk16 { get; }
			public ushort Unk17 { get; }
			public uint Unk18 { get; }
			public uint Unk19 { get; }
			public ushort Unk1A { get; }
			public ushort Unk1B { get; }

			/* clean up some time */
			public int dX { get; set; }
			public int dY { get; set; }
			public EType dType { get; set; }
			public int dMipCount { get; set; }

			public long dOffset { get; set; } // Texture Offset
			public long dOffset2 { get; set; } // Texture Alpha Map Offset

		}

		public class Data
        {
			public Data(BinaryReader br)
            {
				Color0 = Helper.readUInt16B(br);
				Color1 = Helper.readUInt16B(br);

				//Color0 = br.ReadUInt16();
				//Color1 = br.ReadUInt16();

				Unk0 = Helper.swapByte(br);
				Unk1 = Helper.swapByte(br);
				Unk2 = Helper.swapByte(br);
				Unk3 = Helper.swapByte(br);

			}

			public ushort Color0 { get; }
			public ushort Color1 { get; }
			public byte Unk0 { get; }
			public byte Unk1 { get; }
			public byte Unk2 { get; }
			public byte Unk3 { get; }
		}

		public class Data2
		{
			public Data2(BinaryReader br)
			{
				Data00 = br.ReadUInt32();
				Data01 = br.ReadUInt32();
				Data02 = br.ReadUInt32();
				Data03 = br.ReadUInt32();

			}

			public uint Data00 { get; }
			public uint Data01 { get; }
			public uint Data02 { get; }
			public uint Data03 { get; }
		}
	}
}
