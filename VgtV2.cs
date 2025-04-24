using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static olkviewer.Vgt2.Entry;
using static olkviewer.Vgt2.PaletteEntry;

namespace olkviewer
{
    class Vgt2
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
				Version = 4;
				Unk0 = 0;
				TexCount = 0;
				Unk1 = 0;
				HeaderLen = 0;
				HeaderBlockSize = 0;
				
                dOffset = 0;
			}

			// Constructor that takes one argument:
			public Header(BinaryReader br)
			{
				Magic = Helper.readUInt32B(br);
				Version = br.ReadByte();
                Unk0 = Helper.readUInt16B(br);
				LoadedIngame = br.ReadByte();
				TexCount = Helper.readUInt32B(br);
				Unk1 = Helper.readUInt32B(br);
				HeaderLen = Helper.readUInt32B(br);
				HeaderBlockSize = Helper.readUInt32B(br);

				dOffset = 0;
			}

			// Auto-implemented readonly property:
			public uint Magic { get; }
			public byte Version { get; }
			public ushort Unk0 { get; }
			public byte LoadedIngame { get; }
			public uint TexCount { get; }
			public uint Unk1 { get; }
			public uint HeaderLen { get; }
			public uint HeaderBlockSize { get; }
			
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
            public enum WrapMode : uint
			{
			Clamp = 0,
			Repeat = 1,
			Mirror = 2,
			// Hardware testing indicates that WrapMode set to 3 behaves the same as clamp, though this is an
			// invalid value
			};
            public enum FilterMode : uint
            {
			Near = 0,
			Linear = 1,
			};
            public enum MipMode : uint
            {
			None = 0,
			Point = 1,
			Linear = 2,
			};
            public enum LODType : uint
            {
			Edge = 0,
			Diagonal = 1,
			};
            public enum MaxAniso
			{
			One = 0,
			Two = 1,
			Four = 2,
			};
			public class TexMode0
			{
				public uint bytes;
				
				public WrapMode wrap_s 
				{ 
					get => (WrapMode)((bytes >> 0) & 0b11);
					set => bytes = (uint)((((uint)value & 0b11) << 0) | (bytes & ~((uint)0b11 << 0))); 
				}
				public WrapMode wrap_t 
				{ 
					get => (WrapMode)((bytes >> 2) & 0b11);
					set => bytes = (uint)((((uint)value & 0b11) << 2) | (bytes & ~((uint)0b11 << 2))); 
				}
				public FilterMode mag_filter 
				{ 
					get => (FilterMode)((bytes >> 4) & 0b1);
					set => bytes = (uint)((((uint)value & 0b1) << 4) | (bytes & ~((uint)0b1 << 4))); 
				}
				public MipMode mipmap_filter 
				{ 
					get => (MipMode)((bytes >> 5) & 0b11);
					set => bytes = (uint)((((uint)value & 0b11) << 5) | (bytes & ~((uint)0b11 << 5))); 
				}
				public FilterMode min_filter 
				{ 
					get => (FilterMode)((bytes >> 7) & 0b1);
					set => bytes = (uint)((((uint)value & 0b1) << 7) | (bytes & ~((uint)0b1 << 7))); 
				}
				public LODType max_filter 
				{ 
					get => (LODType)((bytes >> 8) & 0b1);
					set => bytes = (uint)((((uint)value & 0b1) << 8) | (bytes & ~((uint)0b1 << 8))); 
				}
				public int lod_bias 
				{ 
					get => (int)((bytes >> 9) & 0b11111111);
					set => bytes = (uint)((((uint)value & 0b11111111) << 9) | (bytes & ~((uint)0b11111111 << 9))); 
				}
				public MaxAniso max_aniso 
				{ 
					get => (MaxAniso)((bytes >> 19) & 0b11);
					set => bytes = (uint)((((uint)value & 0b11) << 19) | (bytes & ~((uint)0b11 << 19))); 
				}
				public LODType lod_clamp 
				{ 
					get => (LODType)((bytes >> 21) & 0b1);
					set => bytes = (uint)((((uint)value & 0b1) << 21) | (bytes & ~((uint)0b1 << 21))); 
				}


			}
			public class TexImage0
            {
                public uint bytes;
                public ushort width
                {
                    get => (ushort)(1+((bytes >> 0) & 0b1111111111));
                    set => bytes = (uint)(((((uint)value & 0b1111111111) << 0) | (bytes & ~((uint)0b1111111111 << 0)))-1);
                }
                public ushort height
                {
                    get => (ushort)(1+((bytes >> 10) & 0b1111111111));
                    set => bytes = (uint)(((((uint)value & 0b1111111111) << 10) | (bytes & ~((uint)0b1111111111 << 10)))-1);
                }
                public EType texture_format
                {
                    get => (EType)((bytes >> 20) & 0b1111);
                    set => bytes = (uint)((((uint)value & 0b1111) << 20) | (bytes & ~((uint)0b1111 << 20)));
                }

            }
            public class TextureSlice
			{
				public TextureSlice() {
					TexModeInfo = new TexMode0();
					unk0 = 0;
					maxlod = 0;
					minlod = 0;
					texImage0 = new TexImage0();
					CLUTOffset = 0;
					unk1 = 0;
					unk2 = 0;
					ImageType = EType.CMPR;
					unk3 = 0;
					imageSizeGC = 2;
					unk4 = 0;
                }
                public TextureSlice(BinaryReader br)
                {
                    TexModeInfo = new TexMode0();
                    TexModeInfo.bytes = Helper.readUInt32B(br);
                    unk0 = Helper.readUInt16B(br);
					maxlod = br.ReadByte();
                    minlod = br.ReadByte();
                    texImage0 = new TexImage0();
                    texImage0.bytes = Helper.readUInt32B(br);
					CLUTOffset = Helper.readUInt32B(br);
					unk1 = Helper.readUInt16B(br);
					unk2 = Helper.readUInt16B(br);
					ImageType = (EType)Helper.readUInt32B(br);
					unk3 = Helper.readUInt32B(br);
					imageSizeGC = Helper.readUInt16B(br);
					unk4 = Helper.readUInt16B(br);
                }
                public TexMode0 TexModeInfo { get; }
                public ushort unk0 { get; }
                public byte maxlod { get; }
                public byte minlod { get; }
                public TexImage0 texImage0 { get; }
                public uint CLUTOffset { get; }
                public ushort unk1 { get; }
                public ushort unk2 { get; }
                public EType ImageType { get; }
                public uint unk3 { get; }
                public ushort imageSizeGC { get; }
                public ushort unk4 { get; }

            }
            
            // Constructor that takes no arguments:
            public Entry()
			{
                TexturePaletteOffset = 0;

				Diffuse = new TextureSlice();
				Alpha = new TextureSlice();

				dX = 0;
				dY = 0;
				dType = 0;
				dOffset = 0;
				dOffset2 = 0;
				dMipCount = 0;
                pEntry = new PaletteEntry();

            }

			// Constructor that takes one argument:
			public Entry(BinaryReader br)
			{
                /* 0x0000 */
                TexturePaletteOffset = Helper.readUInt32B(br); // TexTLUT?
                Diffuse = new TextureSlice();
                Diffuse = new TextureSlice(br);

                Alpha = new TextureSlice();
                Alpha = new TextureSlice(br);

                //size = 0x44
            }

            // Auto-implemented readonly property:

            public TextureSlice Diffuse { get; set; }
            public TextureSlice Alpha { get; set; }
            public uint TexturePaletteOffset { get; }

			/* clean up some time */
			public int dX { get; set; }
			public int dY { get; set; }
			public EType dType { get; set; }
			public int dMipCount { get; set; }

			public long dOffset { get; set; } // Texture Offset
			public long dOffset2 { get; set; } // Texture Alpha Map Offset
            public PaletteEntry pEntry { get; set; }

        }

		public class PaletteEntry{

			public PaletteEntry(){
				Diffuse = new PaletteSlice();
				Alpha = new PaletteSlice();
			}
			public PaletteEntry(BinaryReader br){
				Diffuse = new PaletteSlice(br);
				Alpha = new PaletteSlice(br);
			}
            public class TexTLUT
            {
                public ushort bytes;
                public ushort tmem_offset
                {
                    get => (ushort)( ((bytes >> 0) & 0b1111111111));
                    set => bytes = (ushort)(((((uint)value & 0b1111111111) << 0) | (bytes & ~((uint)0b1111111111 << 0))));
                }
                public PaletteType tlut_format
                {
                    get => (PaletteType)( ((bytes >> 10) & 0b11));
                    set => bytes = (ushort)(((((uint)value & 0b11) << 10) | (bytes & ~((uint)0b11 << 10))));
                }

            }
            public class PaletteSlice{
				public PaletteSlice (){
					Unk0 = 0;
					PalletOffset = 0;
					palletCount = 0;
					textureLookupType = new TexTLUT();

                    textureLookupType.tlut_format = PaletteType.IA8;
					textureLookupType.tmem_offset = 0;
				}
				public PaletteSlice (BinaryReader br)
                {
                    Unk0 = Helper.readUInt16B(br);
                    textureLookupType = new TexTLUT();
                    textureLookupType.bytes = Helper.readUInt16B(br);
                    PalletOffset = Helper.readUInt32B(br);
					palletCount = Helper.readUInt16B(br);
                    Unk1 = Helper.readUInt16B(br);
				}
			public ushort Unk0 {get;}
			public TexTLUT textureLookupType { get; set; }
            public uint PalletOffset {get;}
			public ushort palletCount {get;}
			public ushort Unk1 {get;}
			}

			public PaletteSlice Diffuse{get;set;}
			public PaletteSlice Alpha{get;set;}
			public long dDiffuseOffset;
			public long dAlphaOffset;
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
