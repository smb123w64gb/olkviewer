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
    class Texture
    {
        #region Dolphin
        static uint Convert3To8(uint v)
        {
            // Swizzle bits: 00000123 -> 12312312
            return (v << 5) | (v << 2) | (v >> 1);
        }
        static uint Convert4To8(uint v)
        {
            // Swizzle bits: 00001234 -> 12341234
            return (v << 4) | v;
        }
        static uint Convert5To8(uint v)
        {
            // Swizzle bits: 00012345 -> 12345123
            return (v << 3) | (v >> 2);
        }
        static uint Convert6To8(uint v)
        {
            // Swizzle bits: 00123456 -> 12345612
            return (v << 2) | (v >> 4);
        }

        static uint DecodePixel_IA8(ushort Value)
        {
            int a = Value & 0xFF;
            int i = Value >> 8;
            return (uint)(i | (i << 8) | (i << 16) | (a << 24));
        }
        static uint DecodePixel_RGB565(ushort Value)
        {
            int r, g, b, a;

            r = (int)Convert5To8((uint)((Value >> 11) & 0x1f));
            g = (int)Convert6To8((uint)((Value >> 5) & 0x3f));
            b = (int)Convert5To8((uint)((Value) & 0x1f));
            a = 0xFF;

            return (uint)(r | (g << 8) | (b << 16) | (a << 24));
        }
        static uint DecodePixel_RGB5A3(ushort Value)
        {
            int r, g, b, a;

            if ((Value & 0x8000) != 0)
            {
                r = (int)Convert5To8((uint)((Value >> 10) & 0x1f));
                g = (int)Convert5To8((uint)((Value >> 5) & 0x1f));
                b = (int)Convert5To8((uint)((Value) & 0x1F));
                a = 0xFF;

            } else
            {
                a = (int)Convert3To8((uint)((Value >> 12) & 0x7));
                r = (int)Convert4To8((uint)((Value >> 8) & 0xF));
                g = (int)Convert4To8((uint)((Value >> 4) & 0xF));
                b = (int)Convert4To8((uint)((Value) & 0xF));
            }

            return (uint)(r | (g << 8) | (b << 16) | (a << 24));
        }
        public static uint DecodePixel_Paletted(ushort Pixel, int TlutFmt)
        {
            switch (TlutFmt)
            {
                case 0: // IA8
                    return DecodePixel_IA8(Pixel);
                case 1: // RGB565
                    return DecodePixel_RGB565(Helper.BS16U(Pixel));
                case 2: // RGBA5A3
                    return DecodePixel_RGB5A3(Helper.BS16U(Pixel));
                default:
                    return 0;
            }
        }
        public static void DecodeBytes_C8(ref uint[] Dst, byte[] Src, int DstPos, int SrcPos, byte[] Tlut_, int TlutFmt)
        {
            ushort[] tlut = new ushort[(int)Math.Ceiling(Tlut_.Length / 2.0)];
            Buffer.BlockCopy(Tlut_, 0, tlut, 0, Tlut_.Length);

            for (int x = 0; x < 8; x++)
            {
                byte val = Src[SrcPos + x];
                Dst[DstPos++] = DecodePixel_Paletted(tlut[val], TlutFmt);
            }

        }
        public static void Decode(uint[] Dst, byte[] Src, int Width, int Height, int TextureFormat, byte[] Tlut, int Tlutfmt)
        {
            int Wsteps4 = (Width + 3) / 4;
            int Wsteps8 = (Width + 7) / 8;

            switch (TextureFormat)
            {
                case 9:
                    for (int y = 0; y < Height; y += 4)
                        for (int x = 0, yStep = (y / 4) * Wsteps8; x < Width; x += 8, yStep++)
                            for (int iy = 0, xStep = 4 * yStep; iy < 4; iy++, xStep++)
                                DecodeBytes_C8(ref Dst, Src, (y + iy) * Width + x, 8 * xStep, Tlut, Tlutfmt);
                    break;
            }

        }
        #endregion

        #region WindViewer
        static void R5G6B5ToRGBA8(UInt16 SrcPixel, ref byte[] Dest, int Offset)
        {
            byte R, G, B;
            R = (byte)((SrcPixel & 0xf100) >> 11);
            G = (byte)((SrcPixel & 0x7e0) >> 5);
            B = (byte)((SrcPixel & 0x1f));

            R = (byte)((R << (8 - 5)) | (R >> (10 - 8)));
            G = (byte)((G << (8 - 6)) | (G >> (12 - 8)));
            B = (byte)((B << (8 - 5)) | (B >> (10 - 8)));

            Dest[Offset] = R;
            Dest[Offset + 1] = G;
            Dest[Offset + 2] = B;
            Dest[Offset + 3] = 0xff;
        }
        static void RGB5A3ToRGBA8(UInt16 SrcPixel, ref byte[] Dest, int Offset)
        {
            byte r, g, b, a;

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

            Dest[Offset] = r;
            Dest[Offset + 1] = g;
            Dest[Offset + 2] = b;
            Dest[Offset + 3] = a;
        }
        static void Fix4x4(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 4)
                for (int x = 0; x < Width; x += 4)
                    for (int dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 4; ++dx, S += 2)
                            if (x + dx < Width && y + dy < Height)
                            {
                                int di = 2 * (Width * (y + dy) + x + dx);
                                Dest[di + 0] = Src[S + 1];
                                Dest[di + 1] = Src[S + 0];
                            }
        }
        public static void Fix8x4(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 4)
                for (int x = 0; x < Width; x += 8)
                    for (int dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 8; ++dx, ++S)
                            if (x + dx < Width && y + dy < Height)
                                Dest[Width * (y + dy) + x + dx] = Src[S];
        }
        public static void Enc8x4(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 4)
                for (int x = 0; x < Width; x += 8)
                    for (int dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 8; ++dx, ++S)
                            if (x + dx < Width && y + dy < Height)
                                Dest[S] = Src[Width * (y + dy) + x + dx];
        }
        static void Fix8x4Expand(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 4)
                for (int x = 0; x < Width; x += 8)
                    for (int dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 8; ++dx, ++S)
                            if (x + dx < Width && y + dy < Height)
                            {
                                byte Lum = (byte)(Src[S] & 0xf);
                                Lum |= (byte)(Lum << 4);
                                byte Alpha = (byte)(Src[S] & 0xf0);
                                Alpha |= (byte)(Alpha >> 4);
                                Dest[2 * (Width * (y + dy) + x + dx)] = Lum;
                                Dest[2 * (Width * (y + dy) + x + dx) + 1] = Alpha;
                            }
        }
        public static void Fix8x8Expand(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            int y, x, dy, dx;

            for (y = 0; y < Height; y += 8)
                for (x = 0; x < Width; x += 8)
                    for (dy = 0; dy < 8; ++dy)
                        for (dx = 0; dx < 8; dx += 2, ++S)
                            if (x + dx < Width && y + dy < Height)
                            {
                                byte t = (byte)(Src[S] & 0xf0);
                                Dest[Width * (y + dy) + x + dx] = (byte)(t | (t >> 4));
                                t = (byte)(Src[S] & 0xf);
                                Dest[Width * (y + dy) + x + dx + 1] = (byte)((t << 4) | t);
                            }
        }
        public static void Fix8x8NoExpand(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 8)
                for (int x = 0; x < Width; x += 8)
                    for (int dy = 0; dy < 8; ++dy)
                        for (int dx = 0; dx < 8; dx += 2, ++S)
                            if (x + dx < Width && y + dy < Height)
                            {
                                byte t = (byte)(Src[S] & 0xf0);
                                Dest[Width * (y + dy) + x + dx] = (byte)(t >> 4);
                                t = (byte)(Src[S] & 0xf);
                                Dest[Width * (y + dy) + x + dx + 1] = t;
                            }
        }

        static void FixRGB5A3(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            int y, x, dy, dx;
            for (y = 0; y < Height; y += 4)
                for (x = 0; x < Width; x += 4)
                    for (dy = 0; dy < 4; ++dy)
                        for (dx = 0; dx < 4; ++dx, S += 2)
                            if (x + dx < Width && y + dy < Height)
                            {
                                UInt16 srcPixel = Helper.Read16(Src, S);
                                RGB5A3ToRGBA8(srcPixel, ref Dest, 4 * (Width * (y + dy) + x + dx));
                            }
        }

        static void FixR5G6B5(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            int y, x, dy, dx;
            for (y = 0; y < Height; y += 4)
                for (x = 0; x < Width; x += 4)
                    for (dy = 0; dy < 4; ++dy)
                        for (dx = 0; dx < 4; ++dx, S += 2)
                            if (x + dx < Width && y + dy < Height)
                            {
                                UInt16 srcPixel = Helper.Read16(Src, S);
                                R5G6B5ToRGBA8(srcPixel, ref Dest, 4 * (Width * (y + dy) + x + dx));
                            }
        }

        static void FixRGBA8(ref byte[] Dest, byte[] Src, int S, int Width, int Height)
        {
            for (int y = 0; y < Height; y += 4)
                for (int x = 0; x < Width; x += 4)
                {
                    int dy;

                    for (dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 4; ++dx, S += 2)
                            if (x + dx < Width && y + dy < Height)
                            {
                                UInt32 di = (UInt32)(4 * (Width * (y + dy) + x + dx));
                                Dest[di + 0] = Src[S + 1];
                                Dest[di + 3] = Src[S + 0];
                            }

                    for (dy = 0; dy < 4; ++dy)
                        for (int dx = 0; dx < 4; ++dx, S += 2)
                            if (x + dx < Width && y + dy < Height)
                            {
                                UInt32 di = (UInt32)(4 * (Width * (y + dy) + x + dx));
                                Dest[di + 1] = Src[S + 0];
                                Dest[di + 2] = Src[S + 1];
                            }
                }
        }

        static byte S3TC1ReverseByte(byte B)
        {
            byte B1 = (byte)(B & 0x3);
            byte B2 = (byte)(B & 0xC);
            byte B3 = (byte)(B & 0x30);
            byte B4 = (byte)(B & 0xC0);
            return (byte)((B1 << 6) | (B2 << 2) | (B3 >> 2) | (B4 >> 6));
        }

        static void UnpackPixel(int Index, ref byte[] Dest, int Address, byte[] Palette, Vgt.PaletteType PaletteFormat)
        {
            switch (PaletteFormat)
            {
                case Vgt.PaletteType.IA8:
                    Dest[0] = Palette[2 * Index + 1];
                    Dest[1] = Palette[2 * Index + 0];
                    break;

                case Vgt.PaletteType.RGB565:
                    R5G6B5ToRGBA8(Helper.Read16(Palette, 2 * Index), ref Dest, Address);
                    break;

                case Vgt.PaletteType.RGBA5A3:
                    RGB5A3ToRGBA8(Helper.Read16(Palette, 2 * Index), ref Dest, Address);
                    break;
            }
        }
        static void Unpack8(ref byte[] dst, byte[] src, int w, int h,
                     byte[] palette, Vgt.PaletteType paletteFormat)
        {
            int Address = 0;

            int pixSize = (paletteFormat == Vgt.PaletteType.IA8 ? 2 : 4);

            for (int y = 0; y < h; ++y)
                for (int x = 0; x < w; ++x, Address += pixSize)
                    UnpackPixel(src[y * w + x], ref dst, Address, palette, paletteFormat);
        }
        public static byte[] Load(byte[] Data, uint TextureOffset, int Width, int Height, Vgt.Type Format, byte[] Palette, Vgt.PaletteType PalFormat)
        {
            int BufferSize = (Width * Height) * 4;
            byte[] Tmp = new byte[BufferSize], Tmp2 = new byte[BufferSize];

            for (int i = 0; i < 1; i++) // MipmapCount
            {
                switch (Format)
                {
                    case Vgt.Type.I4:
                        Fix8x8Expand(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.I8:
                        Fix8x4(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.IA4:
                        Fix8x4Expand(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.IA8:
                        Fix4x4(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.RGB565:
                        FixR5G6B5(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.RGB5A3:
                        FixRGB5A3(ref Tmp, Data, (int)TextureOffset, Width, Height);
                        break;
                    case Vgt.Type.RGBA8:
                        FixRGBA8(ref Tmp, Data, (int) TextureOffset, Width, Height);
                        break;

                    case Vgt.Type.C4:
                        Tmp2 = new byte[Tmp.Length * 2];
                        Fix8x8NoExpand(ref Tmp2, Data, (int)TextureOffset, Width, Height);
                        Unpack8(ref Tmp, Tmp2, Width, Height, Palette, PalFormat);
                        break;
                    case Vgt.Type.C8:
                        Tmp2 = new byte[Tmp.Length * 2];
                        Fix8x4(ref Tmp2, Data, (int)TextureOffset, Width, Height);
                        Unpack8(ref Tmp, Tmp2, Width, Height, Palette, PalFormat);
                        break;

                    case Vgt.Type.CMPR:
                        //FixS3TC1
                        //DecompressDXT1
                        break;
                    default:
                        MessageBox.Show("Unknown Texture Type!");
                        //Tmp.Fill(new byte[] { 0xFF, 0x00, 0x00, 0xFF });
                        //System.Windows.Forms.MessageBox.Show(
                        //    string.Format("Unsupported texture type 0x{0:X2}!", TexInfo.Format), "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        break;
                }
            }

            return Tmp;
        }
        public static void SavePNG(string FilePath, byte[] Data, int Width, int Height)
        {
            //byte[] Buffer = new byte[(Width * Height) * 4];
            byte[] Buffer = Data;

            Bitmap TexImage = new Bitmap(Width, Height);
            Rectangle Rect = new Rectangle(0, 0, Width, Height);
            System.Drawing.Imaging.BitmapData BmpData = TexImage.LockBits(Rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, TexImage.PixelFormat);
            IntPtr Ptr = BmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(Buffer, 0, Ptr, Buffer.Length);
            TexImage.UnlockBits(BmpData);

            string SavePath = FilePath;
            TexImage.Save(SavePath, System.Drawing.Imaging.ImageFormat.Png);
        }
        #endregion
    }
}
