using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace olkviewer
{
    class FileVGT
    {
        public static void ByteSwap(byte[] vgtData, int Size, int Width, int Height)
        {
            // MemoryStream ms = new MemoryStream(vgtData);
            //BinaryReader br = new BinaryReader(ms);

            byte[] newData = vgtData;

            using (MemoryStream vgtStream = new MemoryStream(vgtData))
            {

                BinaryReader br = new BinaryReader(vgtStream);

                vgtStream.Seek(0, SeekOrigin.Begin);

                using (MemoryStream newStream = new MemoryStream(newData))
                {
                    BinaryWriter bw = new BinaryWriter(newStream);
                    newStream.Seek(0, SeekOrigin.Begin);

                    for (int i = 0; i < (Size >> 3); i++)
                    {
                        // byte swap
                        Vgt.Data data = new Vgt.Data(br);

                        bw.Write(data.Color0);
                        bw.Write(data.Color1);

                        bw.Write(data.Unk0);
                        bw.Write(data.Unk1);
                        bw.Write(data.Unk2);
                        bw.Write(data.Unk3);
                    }

                    BinaryReader br2 = new BinaryReader(newStream);
                    newStream.Seek(0, SeekOrigin.Begin);

                    /* interleave data */

                    for (int i = 0; i < (Height >> 3); i++)
                    {
                        long offset = (long)(i * (Width * 4));
                        newStream.Seek(offset, SeekOrigin.Begin);

                        List<Vgt.Data2> TopLeft = new List<Vgt.Data2>();
                        List<Vgt.Data2> TopRight = new List<Vgt.Data2>();
                        List<Vgt.Data2> BottomLeft = new List<Vgt.Data2>();
                        List<Vgt.Data2> BottomRight = new List<Vgt.Data2>();

                        List<Vgt.Data2> line = new List<Vgt.Data2>();

                        /* get line */
                        for (int j = 0; j < (Width >> 2); j++)
                        {
                            Vgt.Data2 pData = new Vgt.Data2(br2);
                            //Vgt.Data2 pData2 = new Vgt.Data2(br2);

                            if ((j % 2) == 0)
                            {
                                if ((j >= Width >> 3))
                                {
                                    // top right
                                    TopRight.Add(pData);
                                    //TopRight.Add(pData2);
                                }
                                else
                                {
                                    // top left
                                    TopLeft.Add(pData);
                                    //TopLeft.Add(pData2);
                                }
                            }
                            else if ((j % 2) == 1)
                            {
                                if ((j >= Width >> 3))
                                {
                                    //bottom right
                                    BottomRight.Add(pData);
                                    //BottomRight.Add(pData2);
                                }
                                else
                                {
                                    // bottom left
                                    BottomLeft.Add(pData);
                                    //BottomLeft.Add(pData2);
                                }
                            }
                        }

                        line.AddRange(TopLeft);
                        line.AddRange(TopRight);
                        line.AddRange(BottomLeft);
                        line.AddRange(BottomRight);

                        /* write line */
                        newStream.Seek(offset, SeekOrigin.Begin);

                        for (int j = 0; j < (Width >> 2); j++)
                        {
                            bw.Write(line[j].Data00);
                            bw.Write(line[j].Data01);
                            bw.Write(line[j].Data02);
                            bw.Write(line[j].Data03);
                        }

                    }
                    newStream.Seek(0, SeekOrigin.Begin);
                    vgtData = br2.ReadBytes(Size);

                }

            }

        }

        public static void WriteTextureData(String DestFileName, byte[] Data)
        {
            using (FileStream fs = new FileStream(DestFileName, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(Data);
            }
        }

        public static byte[] GetTextureData_C8(String OlkFileName, long Offset, int Size)
        {
            byte[] dataBuf = new byte[16];
            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                fs.Seek(Offset, SeekOrigin.Begin);
                dataBuf = br.ReadBytes(Size);
            }

            return dataBuf;
        }
        public static void Export(string OlkFileName, string DdsFileName, long Offset, int Width, int Height, bool Mipmap, int MipmapCount)
        {
            byte[] vgtData;
            byte[] mmData;
            var mmDataBuf = new List<byte>();
            long mmOffset;

            /* Read Vgt data */
            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                //long fOffset = (long)vgtEntries[GetIndex(treeView2.SelectedNode)].dOffset;
                //long fOffset2 = (long)vgtEntries[GetIndex(treeView2.SelectedNode)].dOffset2;

                int x = Width;
                int y = Height;

                int size = (x * y) >> 1;
                mmOffset = Offset + size;

                fs.Seek(Offset, SeekOrigin.Begin);

                /* get vgt data */
                vgtData = br.ReadBytes(size);

                ByteSwap(vgtData, size, x, y);
                //Vgt_Data_Read(fs, br, fOffset);

                if (Mipmap)
                {

                    for (int i = 0; i < MipmapCount; i++)
                    {
                        x = x >> 1;
                        y = y >> 1;


                        fs.Seek(mmOffset, SeekOrigin.Begin);

                        size = (x * y) >> 1;
                        mmOffset += (long)size;

                        /* get vgt data */
                        mmData = br.ReadBytes(size);

                        ByteSwap(mmData, size, x, y);

                        mmDataBuf.AddRange(mmData); // add array to buffer
                    }

                    mmData = mmDataBuf.ToArray();

                }
                else
                {
                    // nothing
                    mmData = new byte[16];
                }
            }

            /* write DDS */
            FileDDS.Write(DdsFileName, vgtData, mmData, Width, Height, Mipmap, MipmapCount);
        }

        public static Vgt.Header GetFiles(FileStream fs, BinaryReader br, List<Vgt.Entry> vgtEntries, TreeView treeView2, long Offset)
        {
            //byte[] bytes;

            fs.Seek(Offset, SeekOrigin.Begin);

            Vgt.Header vgtHeader = new Vgt.Header(br);

            vgtHeader.dOffset = fs.Position;
            //vgtHead = vgtHeader;

            vgtEntries.Clear();

            treeView2.BeginUpdate();

            for (int i = 0; i < vgtHeader.Count; i++)
            {
                Vgt.Entry vgtEntry = new Vgt.Entry(br);

                string tString = "UNK";
                //ushort x = 0;
                //ushort y = 0;

                int type = (int)(vgtEntry.Unk4 >> 20) & 0xF;
                int x = (int)(vgtEntry.Unk4 & 0x3FF) + 1;
                int y = (int)((vgtEntry.Unk4 >> 10) & 0x3FF) + 1;


                switch (type)
                {
                    case 0:
                        tString = "I4";
                        vgtEntry.dType = Vgt.Entry.EType.I4;
                        break;
                    case 1:
                        tString = "I8";
                        vgtEntry.dType = Vgt.Entry.EType.I8;
                        break;
                    case 2:
                        tString = "IA4";
                        vgtEntry.dType = Vgt.Entry.EType.IA4;
                        break;
                    case 3:
                        tString = "IA8";
                        vgtEntry.dType = Vgt.Entry.EType.IA8;
                        break;
                    case 4:
                        tString = "RGB565";
                        vgtEntry.dType = Vgt.Entry.EType.RGB565;
                        break;
                    case 5:
                        tString = "RGB5A3";
                        vgtEntry.dType = Vgt.Entry.EType.RGB5A3;
                        break;
                    case 6:
                        tString = "RGBA8";
                        vgtEntry.dType = Vgt.Entry.EType.RGBA8;
                        break;
                    case 8:
                        tString = "C4";
                        vgtEntry.dType = Vgt.Entry.EType.C4;
                        break;
                    case 9:
                        tString = "C8";
                        vgtEntry.dType = Vgt.Entry.EType.C8;
                        break;
                    case 14:
                        tString = "CMPR";
                        vgtEntry.dType = Vgt.Entry.EType.CMPR;
                        break;
                    default:
                        tString = "UNK";
                        vgtEntry.dType = Vgt.Entry.EType.UNK;
                        break;
                };

                vgtEntry.dX = x;
                vgtEntry.dY = y;

                vgtEntry.dOffset = vgtEntry.Offset1 + Offset;
                vgtEntry.dOffset2 = vgtEntry.Offset2 + Offset;

                string cString = String.Format("[{0}x{1}]", x, y);

                vgtEntries.Add(vgtEntry);

                /* list textures */
                treeView2.Nodes.Add("T" + i + "-" + tString + "-" + cString);
            }

            treeView2.EndUpdate();

            /* read header */
            //bytes = br.ReadBytes(Size);

            return vgtHeader;

        }

        public static void Import(string VgtFileName, string OlkFileName, long Offset, bool Mipmap, int MipmapCount)
        {
            byte[] ddsData;
            byte[] vgtData;
            byte[] mmData;
            var mmDataBuf = new List<byte>();
            long mmOffset;

            /* Read data */
            using (FileStream fs = new FileStream(VgtFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                fs.Seek(0, SeekOrigin.Begin);
                uint magic = Helper.readUInt32B(br);

                if (magic != 0x44445320) // "DDS "
                {
                    MessageBox.Show("Not a DDS File!");
                    return;

                }

                /* get x/y */
                fs.Seek(0xC, SeekOrigin.Begin);
                int y = br.ReadInt32();
                int x = br.ReadInt32();

                int size = (x * y) >> 1;

                fs.Seek(0x80, SeekOrigin.Begin);
                ddsData = br.ReadBytes(size);

                /* convert */

                vgtData = FileDDS.ByteSwap(ddsData, size, x, y);
                //vgtData = ddsData;

                mmOffset = 0x80 + size;

                if (Mipmap)
                {
                    for (int i = 0; i < MipmapCount; i++)
                    {
                        fs.Seek(mmOffset, SeekOrigin.Begin);

                        x = x >> 1;
                        y = y >> 1;

                        size = (x * y) >> 1;
                        mmOffset += (long)size;

                        ddsData = br.ReadBytes(size);

                        /* convert */

                        mmData = FileDDS.ByteSwap(ddsData, size, x, y);

                        mmDataBuf.AddRange(mmData);
                    }
                    mmData = mmDataBuf.ToArray();
                }
                else
                {
                    // nothing
                    mmData = new byte[16];
                }


            }

            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                fs.Seek(Offset, SeekOrigin.Begin);
                bw.Write(vgtData);

                if (Mipmap)
                {
                    bw.Write(mmData);
                }
            }
        }
    }
}
