using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class FileDDS
    {
        public static byte[] CreateHeader(FileStream fs, BinaryWriter bw, int Width, int Height, bool Mipmap, int MipmapCount)
        {
            byte[] header = new byte[0x74];

            uint Magic = 0x20534444; // "DDS 

            uint Magic2 = 0x31545844; // "DXT1" 0x44585431

            uint Size = 0x7C;
            uint dwFlags = 0x00081007;

            uint x = (uint)Width; //(uint)vgtEntries[GetIndex(treeView2.SelectedNode)].dX;
            uint y = (uint)Height; //(uint)vgtEntries[GetIndex(treeView2.SelectedNode)].dY;


            uint Mipmaps = 1;

            if (Mipmap)
            {
                Mipmaps += (uint)MipmapCount;
                dwFlags |= 0x20000;
            }

            bw.Write(Magic);
            bw.Write(Size);
            bw.Write(dwFlags);
            bw.Write(header);

            /* write height/size */
            fs.Seek(0xC, SeekOrigin.Begin);

            bw.Write(y);
            bw.Write(x);

            /* write mipmap */
            fs.Seek(0x1C, SeekOrigin.Begin);
            bw.Write(Mipmaps);

            /* show thumbnail in explorer*/
            uint dword1 = 0x20;
            uint dword2 = 4;
            fs.Seek(0x4C, SeekOrigin.Begin);
            bw.Write(dword1);
            bw.Write(dword2);

            /* write type */
            fs.Seek(0x54, SeekOrigin.Begin);
            bw.Write(Magic2);

            /* write dwcaps */
            if (Mipmap)
            {
                uint dwcaps = 0x00401008;
                fs.Seek(0x6C, SeekOrigin.Begin);
                bw.Write(dwcaps);
            }

            return header;
        }
        public static byte[] ByteSwap(byte[] ddsData, int Size, int Width, int Height)
        {
            byte[] newData = ddsData;

            using (MemoryStream ddsStream = new MemoryStream(ddsData))
            {
                BinaryReader br = new BinaryReader(ddsStream);

                ddsStream.Seek(0, SeekOrigin.Begin);

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
                        List<Vgt.Data2> line2 = new List<Vgt.Data2>();

                        /* get line */
                        for (int j = 0; j < (Width >> 2); j++)
                        {
                            Vgt.Data2 pData = new Vgt.Data2(br2);
                            line.Add(pData);
                        }

                        int tLeft = 0;
                        int tRight = 0;
                        int bLeft = 0;
                        int bRight = 0;

                        /* Top Left */
                        for (int j = 0; j < (Width >> 4); j++)
                        {
                            if (j == 0)
                            {
                                TopLeft.Add(line[j]);
                            }
                            else if ((j % 2) == 1)
                            {
                                TopLeft.Add(line[j + (((Width >> 3) - 1) - (tLeft))]);
                                tLeft++;
                            }
                            else
                            {
                                TopLeft.Add(line[j - tLeft]);
                            }
                        }

                        /* Top Right */
                        for (int j = 0; j < (Width >> 4); j++)
                        {
                            if (j == 0)
                            {
                                TopRight.Add(line[j + (Width >> 5)]);
                            }
                            else if ((j % 2) == 1)
                            {
                                TopRight.Add(line[j + (((Width >> 5) + (Width >> 3) - 1) - (tRight))]);
                                tRight++;
                            }
                            else
                            {
                                TopRight.Add(line[(j + (Width >> 5)) - tRight]);
                            }
                        }

                        /* Bottom Left */
                        for (int j = 0; j < (Width >> 4); j++)
                        {
                            if (j == 0)
                            {
                                BottomLeft.Add(line[j + (Width >> 4)]);
                            }
                            else if ((j % 2) == 1)
                            {
                                BottomLeft.Add(line[j + (((Width >> 4) + (Width >> 3) - 1) - (bLeft))]);
                                bLeft++;
                            }
                            else
                            {
                                BottomLeft.Add(line[(j + (Width >> 4)) - bLeft]);
                            }
                        }

                        /* Bottom Right */
                        for (int j = 0; j < (Width >> 4); j++)
                        {
                            if (j == 0)
                            {
                                BottomRight.Add(line[j + ((Width >> 3) - (Width >> 5))]);
                            }
                            else if ((j % 2) == 1)
                            {
                                BottomRight.Add(line[j + (((Width >> 2) - (Width >> 5) - 1) - (bRight))]);
                                bRight++;
                            }
                            else
                            {
                                BottomRight.Add(line[(j + (Width >> 3) - (Width >> 5)) - bRight]);
                            }
                        }





                        line2.AddRange(TopLeft);
                        line2.AddRange(TopRight);
                        line2.AddRange(BottomLeft);
                        line2.AddRange(BottomRight);

                        /* write line */
                        newStream.Seek(offset, SeekOrigin.Begin);

                        for (int j = 0; j < (Width >> 2); j++)
                        {
                            bw.Write(line2[j].Data00);
                            bw.Write(line2[j].Data01);
                            bw.Write(line2[j].Data02);
                            bw.Write(line2[j].Data03);
                        }

                    }
                }


            }

            return newData;
        }

        public static void Write(string DestPath, byte[] TextureData, byte[] MipmapData, int Width, int Height, bool Mipmap, int MipmapCount)
        {
            using (FileStream fs = new FileStream(DestPath, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] header = FileDDS.CreateHeader(fs, bw, Width, Height, Mipmap, MipmapCount);

                /* seek to end of header */
                fs.Seek(0x80, SeekOrigin.Begin);

                /* write vgt data */
                bw.Write(TextureData);

                /* write mipmap data */
                if (Mipmap)
                {
                    bw.Write(MipmapData);
                }

            }
        }
    }
}
