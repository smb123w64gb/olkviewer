using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace olkviewer
{
    class File
    {
        public enum Type : ushort
        {
            Null,
            Olk,
            Pkg,
            Vgt,
            Vpt,
            Vxt,
            Vmg,
            Vmx,
            Vmp,
            Mmg,
            Mmp,
            Mmx,
            Lpb,
            Vtb,
            Mot,
            Par,
            Unk,
        }

        public class Entry
        {
            public Entry()
            {
                Name = "*";
                Offset = 0;
                Size = 0;
                Type = Type.Unk;

            }

            public Entry(String name, uint offset, uint size, File.Type type)
            {
                Name = name;
                Offset = offset;
                Size = size;
                Type = type;
            }

            public string Name { get; set; }
            public uint Offset { get; set; }
            public uint Size { get; set; }
            public File.Type Type { get; set; }
        }
    }
}
