using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace olkviewer
{
    class Vmg
    {
		public class Header
		{

			// Constructor that takes no arguments:
			public Header()
			{
				/* 0x0000 */ Magic = 0;
				/* 0x0004 */ Version = 0;
				/* 0x0005 */ Unk01 = 0;
				/* 0x0006 */ Unk02 = 0;
				/* 0x0007 */ Unk03 = 0;
				/* 0x0008 */ Unk04 = 0;
				/* 0x0009 */ Contents = 0;
				/* 0x000A */ MatricesCount = 0;
				/* 0x000C */ Objects1Count = 0;
				/* 0x000E */ Objects2Count = 0;
				/* 0x0010 */ Objects3Count = 0;
				/* 0x0012 */ BonesCount = 0;
				/* 0x0014 */ MaterialsCount = 0;
				/* 0x0016 */ MeshCount = 0;
				/* 0x0018 */ MaterialOffset = 0;
				/* 0x001C */ VertexOffset = 0;
				/* 0x0020 */ TextureTableOffset = 0;
				/* 0x0024 */ MatrixTableffset = 0;
				/* 0x0028 */ Unk01Offset = 0;
				/* 0x002C */ Object1Offset = 0;
				/* 0x0030 */ Object2Offset = 0;
				/* 0x0034 */ Object3Offset = 0;
				/* 0x0038 */ WeightTableOffset = 0;
				/* 0x003C */ Unk02Offset = 0;
				/* 0x0040 */ BoneOffset = 0;
				/* 0x0044 */ NameOffset = 0;
				/* 0x0048 */ Unk03Offset = 0;
			}

			// Constructor that takes one argument:
			public Header(BinaryReader br)
			{
				Magic = Helper.readUInt32B(br);
				Version = br.ReadByte();

				Unk01 = br.ReadByte();
				Unk02 = br.ReadByte();
				Unk03 = br.ReadByte();
				Unk04 = br.ReadByte();
				Contents = br.ReadByte();

				MatricesCount = Helper.readUInt16B(br);
				Objects1Count = Helper.readUInt16B(br);
				Objects2Count = Helper.readUInt16B(br);
				Objects3Count = Helper.readUInt16B(br);
				BonesCount = Helper.readUInt16B(br);
				MaterialsCount = Helper.readUInt16B(br);
				MeshCount = Helper.readUInt16B(br);

				MaterialOffset = Helper.readUInt32B(br);
				VertexOffset = Helper.readUInt32B(br);
				TextureTableOffset = Helper.readUInt32B(br);
				MatrixTableffset = Helper.readUInt32B(br);
				Unk01Offset = Helper.readUInt32B(br);
				Object1Offset = Helper.readUInt32B(br);
				Object2Offset = Helper.readUInt32B(br);
				Object3Offset = Helper.readUInt32B(br);
				WeightTableOffset = Helper.readUInt32B(br);
				Unk02Offset = Helper.readUInt32B(br);
				BoneOffset = Helper.readUInt32B(br);
				NameOffset = Helper.readUInt32B(br);
				Unk03Offset = Helper.readUInt32B(br);
			}

			// Auto-implemented readonly property:
			public uint Magic { get; }
			public byte Version { get; } // 0x02=Xbox?, 0x03=Gamecube, 0x04=Xbox
			public byte Unk01 { get; } // pointer to offsets? + 8
			public byte Unk02 { get; } // ? usually 0x0D
			public byte Unk03 { get; } // usually 0
			public byte Unk04 { get; } // usually 0
			public byte Contents { get; } // 0x00=Stage, 0x01=Character, 0x02=Weapon
			public ushort MatricesCount { get; }
			public ushort Objects1Count { get; }
			public ushort Objects2Count { get; }
			public ushort Objects3Count { get; }
			public ushort BonesCount { get; }
			public ushort MaterialsCount { get; }
			public ushort MeshCount { get; } // always 1?
			public uint MaterialOffset { get; }
			public uint VertexOffset { get; }
			public uint TextureTableOffset { get; }
			public uint MatrixTableffset { get; }
			public uint Unk01Offset { get; }
			public uint Object1Offset { get; }
			public uint Object2Offset { get; }
			public uint Object3Offset { get; }
			public uint WeightTableOffset { get; }
			public uint Unk02Offset { get; }
			public uint BoneOffset { get; }
			public uint NameOffset { get; }
			public uint Unk03Offset { get; }

		}

		public class Object1Entry
		{
			public Object1Entry()
			{
				/* 0x0000 */ Unk0 = 0; // & 0x2
				/* 0x0001 */ Unk1 = 0; // 0xC? // count for buffer1?
				/* 0x0002 */ Unk2 = 0;
				/* 0x0003 */ Unk3 = 0; // 0x00=TriangleStrip, 0x01=TriangleList
				/* 0x0004 */ Unk4 = 0; // count for buffer3?
				/* 0x0005 */ Unk5 = 0;
				/* 0x0006 */ Unk6 = 0;
				/* 0x0007 */ Unk7 = 0;
				/* 0x0008 */ Unk8 = 0;
				/* 0x0009 */ Unk9 = 0;
				/* 0x000A */ UnkA = 0;
				/* 0x000B */ UnkB = 0;
				/* 0x000C */ UnkC = 0;
				/* 0x000D */ UnkD = 0;
				/* 0x000E */ FaceCount = 0;
				/* 0x0010 */ MatrixOffset = 0;
				/* 0x0014 */ VertexOffset = 0;
				/* 0x0018 */ Buffer1Offset = 0; // idxPosition// (?) vertex x y z positions // size = ((faceCount / 2) + 1) * 0xC index buffer?
				/* 0x001C */ Buffer2Offset = 0; //  The value maybe zero when object type equal to zero (Static mesh)
				/* 0x0020 */ Buffer3Offset = 0; // // idxNormal // u16?
				/* 0x0024 */ Buffer4Offset = 0; // The value maybe zero when object type equal to zero (Static mesh)
				/* 0x0028 */ Buffer5Offset = 0; // idxColor // 0xFFFFFFFF data, vertex colour?
				/* 0x002C */ Buffer6Offset = 0; // u16 idxTexCoord (?) // 0x10 * face count??
				/* 0x0030 */ FaceOffset = 0;
				/* 0x0034 */ UnkE = 0;
			}

			public Object1Entry(BinaryReader br)
			{
				Unk0 = br.ReadByte();//Helper.readUInt32B(br);
				Unk1 = br.ReadByte();
				Unk2 = br.ReadByte();
				Unk3 = br.ReadByte();
				Unk4 = br.ReadByte();
				Unk5 = br.ReadByte();
				Unk6 = br.ReadByte();
				Unk7 = br.ReadByte();
				Unk8 = br.ReadByte();
				Unk9 = br.ReadByte();
				UnkA = br.ReadByte();
				UnkB = br.ReadByte();
				UnkC = br.ReadByte();
				UnkD = br.ReadByte();

				FaceCount = Helper.readUInt16B(br);

				MatrixOffset = Helper.readUInt32B(br);
				VertexOffset = Helper.readUInt32B(br);
				Buffer1Offset = Helper.readUInt32B(br);
				Buffer2Offset = Helper.readUInt32B(br);
				Buffer3Offset = Helper.readUInt32B(br);
				Buffer4Offset = Helper.readUInt32B(br);
				Buffer5Offset = Helper.readUInt32B(br);
				Buffer6Offset = Helper.readUInt32B(br);

				FaceOffset = Helper.readUInt32B(br);
				UnkE = Helper.readUInt32B(br);
			}

			public byte Unk0 { get; }
			public byte Unk1 { get; }
			public byte Unk2 { get; }
			public byte Unk3 { get; }
			public byte Unk4 { get; }
			public byte Unk5 { get; }
			public byte Unk6 { get; }
			public byte Unk7 { get; }
			public byte Unk8 { get; }
			public byte Unk9 { get; }
			public byte UnkA { get; }
			public byte UnkB { get; }
			public byte UnkC { get; }
			public byte UnkD { get; }

			public ushort FaceCount { get; }

			public uint MatrixOffset { get; }
			public uint VertexOffset { get; }
			public uint Buffer1Offset { get; }
			public uint Buffer2Offset { get; }
			public uint Buffer3Offset { get; }
			public uint Buffer4Offset { get; }
			public uint Buffer5Offset { get; }
			public uint Buffer6Offset { get; }

			public uint FaceOffset { get; }

			public uint UnkE { get; }
		}
	}
}
