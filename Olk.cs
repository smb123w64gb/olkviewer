using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class Olk
    {
		

		public class Header
		{
			public Header()
			{
				Count = 0;
				Magic = 0;
				Offset = 0;
				_p0 = 0;
			}
			public Header(BinaryReader br)
			{
				Count = br.ReadUInt32();
				Magic = br.ReadUInt32();
				Offset = br.ReadUInt32();
				_p0 = br.ReadUInt32();
			}

			public uint Count { get; }
			public uint Magic { get; }
			public uint Offset { get; }
			public uint _p0 { get; }
		}


		public class Entry
		{

			public Entry(BinaryReader br)
			{

				Offset = br.ReadUInt32(); 
				Size = br.ReadUInt32();
				Date = br.ReadUInt32();
				_p0 = br.ReadUInt32();
			}

			public uint Offset { get; }
			public uint Size { get; }
			public uint Date { get; }
			public uint _p0 { get; }

		}
	}
}
