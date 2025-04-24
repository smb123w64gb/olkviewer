using S16.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static Bitmap RenderImage(String OlkFileName, Vgt2.Entry Entry) 
        {
            if(Entry.Diffuse.ImageType == Vgt2.Entry.EType.C8 || Entry.Diffuse.ImageType == Vgt2.Entry.EType.C4)
            {
                if (Entry.pEntry.Diffuse.textureLookupType.tlut_format == Vgt2.PaletteType.RGB565)
                {
                    byte[] palData = new byte[512];
                    Color[] colors = new Color[256];
                    byte[] vgtData;
                    using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        fs.Seek(Entry.pEntry.dDiffuseOffset, SeekOrigin.Begin);
                        //palData = br.ReadBytes(512);
                        var stride = 0;
                        for(int i = 0; i < colors.Length; i++) {
                            byte R, G, B;

                            /*r = (byte)(palData[0 + stride]);
                            g = (byte)(palData[0 + stride]);
                            b = (byte)(palData[1 + stride]);
                            a = (byte)(palData[1 + stride]);*/
                            UInt16 SrcPixel = (UInt16)Helper.readInt16B(br);

                            R = (byte)((SrcPixel & 0xf100) >> 11);
                            G = (byte)((SrcPixel & 0x7e0) >> 5);
                            B = (byte)((SrcPixel & 0x1f));

                            R = (byte)((R << (8 - 5)) | (R >> (10 - 8)));
                            G = (byte)((G << (8 - 6)) | (G >> (12 - 8)));
                            B = (byte)((B << (8 - 5)) | (B >> (10 - 8)));

                            colors[i] = Color.FromArgb(255 ,R, G, B);
                            stride += 2;
                        }

                        

                        int x = Entry.Diffuse.texImage0.width;
                        int y = Entry.Diffuse.texImage0.height;

                        int size = (x * y);
                        fs.Seek(Entry.dOffset, SeekOrigin.Begin);
                        
                        byte[] fixedData = new byte[size];
                        if (Entry.Diffuse.ImageType == Vgt2.Entry.EType.C4)
                        {
                            vgtData = br.ReadBytes(size / 2);
                            Texture.Fix8x8NoExpand(ref fixedData, vgtData, 0, x, y);
                        }
                        else
                        {
                            vgtData = br.ReadBytes(size);
                            Texture.Fix8x4(ref fixedData, vgtData, 0, x, y);
                        }
                        Bitmap bitmap = new Bitmap(x,y,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        int idx = 0;
                        for (int h = 0; h < y; h++)
                        {
                            for (int w = 0; w < x; w++)
                            {
                                bitmap.SetPixel(w,h,colors[fixedData[idx]]);
                                idx += 1;
                            }

                        }
                        return bitmap;


                    }
                }
                else if(Entry.pEntry.Diffuse.textureLookupType.tlut_format == Vgt2.PaletteType.RGBA5A3)
                {
                    byte[] palData = new byte[512];
                    Color[] colors = new Color[256];
                    byte[] vgtData;
                    using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        fs.Seek(Entry.pEntry.dDiffuseOffset, SeekOrigin.Begin);
                        //palData = br.ReadBytes(512);
                        var stride = 0;
                        for (int i = 0; i < colors.Length; i++)
                        {
                            byte r, g, b, a;

                            /*r = (byte)(palData[0 + stride]);
                            g = (byte)(palData[0 + stride]);
                            b = (byte)(palData[1 + stride]);
                            a = (byte)(palData[1 + stride]);*/
                            UInt16 SrcPixel = (UInt16)Helper.readInt16B(br);
                            if ((SrcPixel & 0x8000) == 0x8000)
                            {
                                a = 0xff;

                                r = (byte)((SrcPixel & 0x7c00) >> 10);
                                r = (byte)((r << (8 - 5)) | (r >> (10 - 8)));

                                g = (byte)((SrcPixel & 0x3e0) >> 5);
                                g = (byte)((g << (8 - 5)) | (g >> (10 - 8)));

                                b = (byte)(SrcPixel & 0x1f);
                                b = (byte)((b << (8 - 5)) | (b >> (10 - 8)));
                            }
                            else
                            {
                                a = (byte)((SrcPixel & 0x7000) >> 12);
                                a = (byte)((a << (8 - 3)) | (a << (8 - 6)) | (a >> (9 - 8)));

                                r = (byte)((SrcPixel & 0xf00) >> 8);
                                r = (byte)((r << (8 - 4)) | r);

                                g = (byte)((SrcPixel & 0xf0) >> 4);
                                g = (byte)((g << (8 - 4)) | g);

                                b = (byte)(SrcPixel & 0xf);
                                b = (byte)((b << (8 - 4)) | b);
                            }
                            colors[i] = Color.FromArgb(a, r, g, b);
                            stride += 2;
                        }



                        int x = Entry.Diffuse.texImage0.width;
                        int y = Entry.Diffuse.texImage0.height;

                        int size = (x * y);
                        byte[] fixedData = new byte[size];
                        fs.Seek(Entry.dOffset, SeekOrigin.Begin);
                        if (Entry.Diffuse.ImageType == Vgt2.Entry.EType.C4)
                        {
                            vgtData = br.ReadBytes(size/2);
                            Texture.Fix8x8NoExpand(ref fixedData, vgtData, 0, x, y);
                        }
                        else
                        {
                            vgtData = br.ReadBytes(size);
                            Texture.Fix8x4(ref fixedData, vgtData, 0, x, y);
                        }

                        
                        Bitmap bitmap = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        int idx = 0;
                        for (int h = 0; h < y; h++)
                        {
                            for (int w = 0; w < x; w++)
                            {
                                bitmap.SetPixel(w, h, colors[fixedData[idx]]);
                                idx += 1;
                            }

                        }
                        return bitmap;
                    }
                }else if (Entry.pEntry.Diffuse.textureLookupType.tlut_format == Vgt2.PaletteType.IA8)
                {
                    byte[] palData = new byte[512];
                    byte[] palData_alt = new byte[512];
                    Color[] colors = new Color[256];
                    byte[] vgtData;
                    using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        if (Entry.pEntry.Alpha.palletCount > 0)
                        {
                            
                            fs.Seek(Entry.pEntry.dAlphaOffset, SeekOrigin.Begin);
                            palData_alt = br.ReadBytes(512);

                        }
                        fs.Seek(Entry.pEntry.dDiffuseOffset, SeekOrigin.Begin);
                        palData = br.ReadBytes(512);
                        var stride = 0;
                        for (int i = 0; i < colors.Length; i++)
                        {
                            if(Entry.pEntry.Alpha.palletCount > 0)
                            {
                                colors[i] = Color.FromArgb(palData_alt[stride], palData[stride+1], palData[stride], palData_alt[stride+1]);
                            }
                            else { 
                            colors[i] = Color.FromArgb(palData[stride+1], palData[stride], palData[stride], palData[stride]);
                            }
                            stride += 2;
                        }



                        int x = Entry.Diffuse.texImage0.width;
                        int y = Entry.Diffuse.texImage0.height;

                        int size = (x * y);
                        fs.Seek(Entry.dOffset, SeekOrigin.Begin);
                       
                        byte[] fixedData = new byte[size];
                        if (Entry.Diffuse.ImageType == Vgt2.Entry.EType.C4)
                        {
                            vgtData = br.ReadBytes(size / 2);
                            Texture.Fix8x8NoExpand(ref fixedData, vgtData, 0, x, y);
                        }
                        else
                        {
                            vgtData = br.ReadBytes(size);
                            Texture.Fix8x4(ref fixedData, vgtData, 0, x, y);
                        }
                        Bitmap bitmap = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        int idx = 0;
                        for (int h = 0; h < y; h++)
                        {
                            for (int w = 0; w < x; w++)
                            {
                                bitmap.SetPixel(w, h, colors[fixedData[idx]]);
                                idx += 1;
                            }

                        }
                        return bitmap;


                    }
                }
            }
            return null;
        }
            

        



        public static Bitmap RenderImage(String OlkFileName, long Offset, int Width, int Height, bool Mipmap, int MipmapCount)
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

            //S16.Drawing.DDSImage ddsFile = DDSImage.DDSImage(FileDDS.WriteByte); 
            DDSImage img = new DDSImage(FileDDS.WriteByte(vgtData, mmData, Width, Height, Mipmap, MipmapCount));
            return img.BitmapImage;

            //FileDDS.Write(DdsFileName, vgtData, mmData, Width, Height, Mipmap, MipmapCount);

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

        public static Vgt2.Header GetFiles(FileStream fs, BinaryReader br, List<Vgt2.Entry> vgtEntries, TreeView treeView2, long Offset)
        {
            //byte[] bytes;

            fs.Seek(Offset, SeekOrigin.Begin);

            Vgt2.Header vgtHeader = new Vgt2.Header(br);

            vgtHeader.dOffset = fs.Position;
            //vgtHead = vgtHeader;

            vgtEntries.Clear();

            treeView2.BeginUpdate();
            if(vgtHeader.Version == 4) { 
            for (int i = 0; i < vgtHeader.TexCount; i++)
            {
                Vgt2.Entry vgtEntry = new Vgt2.Entry(br);
                string pString = "";
                if(vgtEntry.TexturePaletteOffset>0){
                    long returnOffset = fs.Position;
                    fs.Seek(Offset+vgtEntry.TexturePaletteOffset, SeekOrigin.Begin);
                    Vgt2.PaletteEntry palEntry = new Vgt2.PaletteEntry(br);
                    if(palEntry.Diffuse.palletCount > 0 && palEntry.Alpha.palletCount > 0)
                        {
                            pString = "_" + "RGBA8";
                        }
                        else { 
                    pString = "_" + palEntry.Diffuse.textureLookupType.tlut_format.ToString();
                        }
                        palEntry.dDiffuseOffset = palEntry.Diffuse.PalletOffset + Offset;
                    palEntry.dAlphaOffset = palEntry.Alpha.PalletOffset + Offset;
                    vgtEntry.pEntry = palEntry;
                    fs.Seek(returnOffset, SeekOrigin.Begin);
                }

                Vgt2.Entry.EType type = (vgtEntry.Diffuse.texImage0.texture_format);
                int x = (int)(vgtEntry.Diffuse.texImage0.width);
                int y = (int)(vgtEntry.Diffuse.texImage0.height);
                int mipFilter = (int)(vgtEntry.Diffuse.TexModeInfo.mipmap_filter);
                int maxlod = (int)(vgtEntry.Diffuse.maxlod);
                int mipcount = 0;
                if(mipFilter>0){
                    mipcount = (maxlod + 0xf) / 0x10;
                }

                vgtEntry.dX = x;
                vgtEntry.dY = y;
                vgtEntry.dMipCount = mipcount;

                vgtEntry.dOffset = vgtEntry.Diffuse.CLUTOffset + Offset;
                vgtEntry.dOffset2 = vgtEntry.Alpha.CLUTOffset + Offset;

                string cString = String.Format("[{0}x{1}]", x, y);

                vgtEntries.Add(vgtEntry);
                

                /* list textures */
                treeView2.Nodes.Add("T"+ + i + "-" + type.ToString()+ pString + "-" + cString);
            }
            }

            treeView2.EndUpdate();

            /* read header */
            //bytes = br.ReadBytes(Size);

            return vgtHeader;

        }
        public static void Import(string OlkFileName, Vgt2.Entry entry, Bitmap InTexture)
        {
            int x = entry.Diffuse.texImage0.width;
            int y = entry.Diffuse.texImage0.height;

            int size = (x * y);

            byte[] vgtData = new byte[size];

            byte[] pal1 = new byte[512];
            byte[] pal2 = new byte[512];


            /* Read data */
            if (entry.pEntry.Diffuse.palletCount > 0 && entry.pEntry.Alpha.palletCount > 0)
            {
                long stride = 0;
                foreach(Color vcolor in InTexture.Palette.Entries)
                {
                    pal1[stride + 1] = vcolor.R;
                    pal1[stride + 0] = vcolor.G;
                    pal2[stride + 1] = vcolor.B;
                    pal2[stride + 0] = vcolor.A;
                    stride += 2;
                }
            }
            int idx = 0;
            Rectangle rect = new Rectangle(0, 0, InTexture.Width, InTexture.Height);
            System.Drawing.Imaging.BitmapData bmpData =
            InTexture.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
            InTexture.PixelFormat);


            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptr, vgtData, 0, size);
            byte[] sizzled = new byte[size];
            Texture.Enc8x4(ref sizzled, vgtData, 0, x, y);


            //Write data
            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                fs.Seek(entry.dOffset, SeekOrigin.Begin);
                bw.Write(sizzled);
                fs.Seek(entry.pEntry.dDiffuseOffset, SeekOrigin.Begin);
                bw.Write(pal1);
                fs.Seek(entry.pEntry.dAlphaOffset,SeekOrigin.Begin);
                bw.Write(pal2);

                //if (Mipmap)
                //{
                //    bw.Write(mmData);
                //}
            }
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
