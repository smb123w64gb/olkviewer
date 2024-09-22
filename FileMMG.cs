using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class FileMMG
    {
        /*
        public static void Extract()
        {

            byte[] dspHeader = null;
            byte[] dspData = null;

            int i = listBox1.SelectedIndex;

            using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                //MessageBox.Show(cnt.ToString());
                //Mmg_Extract(fs, br, chdpEntries[i].dHOffset, chdpEntries[i].dOffset, chdpEntries[i].dSize);

                long headerOffset = chdpEntries[i].dHOffset;
                long dataOffset = chdpEntries[i].dOffset;

                int dataSize = int.Parse(textBox3.Text, System.Globalization.NumberStyles.HexNumber);//chdpEntries[i].dSize;

                /* get adpcm header 
                fs.Seek(headerOffset, SeekOrigin.Begin);

                dspHeader = br.ReadBytes(0x60);

                /* get adpcm data 
                fs.Seek(dataOffset, SeekOrigin.Begin);

                dspData = br.ReadBytes(dataSize);

                string dspFn = saveFileDialog2.FileName;

                //Directory.CreateDirectory(Path.GetDirectoryName(dspFn));

                using (FileStream fs2 = new FileStream(dspFn, FileMode.Create))
                {
                    BinaryWriter bw = new BinaryWriter(fs2);

                    bw.Write(dspHeader);
                    bw.Write(dspData);
                }
            }
        }

        public static void ExtractAll()
        {
            int cnt = 0;

            byte[] dspHeader = null;
            byte[] dspData = null;

            using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                cnt = listBox1.Items.Count;

                //MessageBox.Show(cnt.ToString());

                for (int i = 0; i < cnt; i++)
                {

                    //Mmg_Extract(fs, br, chdpEntries[i].dHOffset, chdpEntries[i].dOffset, chdpEntries[i].dSize);

                    long headerOffset = chdpEntries[i].dHOffset;
                    long dataOffset = chdpEntries[i].dOffset;

                    int dataSize = chdpEntries[i].dSize;

                    /* get adpcm header 
                    fs.Seek(headerOffset, SeekOrigin.Begin);

                    dspHeader = br.ReadBytes(0x60);

                    /* get adpcm data 
                    fs.Seek(dataOffset, SeekOrigin.Begin);

                    dspData = br.ReadBytes(dataSize);

                    string dspFn = folderBrowserDialog1.SelectedPath + "\\out\\" + listBox1.GetItemText(listBox1.Items[i]) + ".dsp";

                    Directory.CreateDirectory(Path.GetDirectoryName(dspFn));

                    using (FileStream fs2 = new FileStream(dspFn, FileMode.Create))
                    {
                        BinaryWriter bw = new BinaryWriter(fs2);

                        bw.Write(dspHeader);
                        bw.Write(dspData);
                    }
                }
            }

        }*/

        public static void GetCHDPEntries(FileStream fs, BinaryReader br, Mmg.Entry mmgEntry, List<string> mmgEntriesString, List<Mmg.ChdpEntry> chdpEntries, long fOffset, int i)
        {
            Mmg.ChdpHeader chdpHeader = new Mmg.ChdpHeader(br);

            //MessageBox.Show(String.Format("{0:X2} nextpos {1:X8}", chdpHeader.Count, nextPos));

            for (int j = 0; j < chdpHeader.Count; j++)
            {

                long curPos2 = fs.Position;

                //get header offset and data offset
                Mmg.ChdpEntry chdpEntry = new Mmg.ChdpEntry(br);

                long curPos3 = fs.Position;

                chdpEntry.dHOffset = (int)curPos2 + 0x50;

                chdpEntry.dOffset = (int)(fOffset + mmgEntry.DataOffset + chdpEntry.Offset);

                // figure out how to get size

                uint nextOffset = Helper.readUInt32B(br);

                if (j == (chdpHeader.Count - 1))
                {
                    nextOffset = mmgEntry.DataSize;
                }

                //chdpEntry.dSize = (int)(nextOffset - chdpEntry.Offset);

                int dspSizeAlign = 0;
                int align = (((chdpEntry.dspNibbleCount >> 1) & 0xFFFFFF) % 0x20);
                if (align != 0)
                {
                    dspSizeAlign = 0x20 - align;
                }

                chdpEntry.dSize = ((chdpEntry.dspNibbleCount >> 1) & 0xFFFFFF) + dspSizeAlign;

                string test = "wave-" + (i + 1) + "-" + String.Format("{0:d2}", (j + 1));

                string mmgIdxString = test;

                mmgEntriesString.Add(mmgIdxString);


                chdpEntries.Add(chdpEntry);

                fs.Seek(curPos3, SeekOrigin.Begin);
            }
        }
    }
}
