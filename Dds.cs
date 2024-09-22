using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class Dds
    {
        public class HeaderCreate
        {
            public HeaderCreate(BinaryWriter bw)
            {
                uint Magic = 0x44445320; // "DDS 

                byte[] header = new byte[0x7C];

                bw.Write(Magic);
                bw.Write(header);
            }
        }


        public int dwSize { get; }
        public int dwFlags { get; }
        public int dwHeight { get; set; }
        public int dwWidth { get; set; }
        public int dwPitchOrLinearSize { get; }
        public int dwDepth { get; }
        public int dwMipMapCount { get; set; }
    }
}
