using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace olkviewer
{
    class FilePKG
    {
        public static File.Type GetFileType(uint Size, uint fHeader, uint fHeader2)
        {
            File.Type type = File.Type.Unk;

            //uint headerLo = fHeader & 0xFFFF;
            uint headerLo = (fHeader & 0xFFFF0000) >> 16;

            if (Size != 0)
            {
                if (fHeader2 == 0x6F6C6E6B) // olnk 
                {
                    type = File.Type.Olk;
                }
                //else if ((fHeader2 == 0x80) || (fHeader2 == 0x800) || (fHeader2 == 0x80000000) || (fHeader2 == 0x00080000)) // pkg, find another way to detect this
                else if (FileOLK.DtpCheck(fHeader, fHeader2))
                {
                    type = File.Type.Pkg;
                }
                else if (((fHeader2 == 0x33303032) || (fHeader2 == 0x32303033)) || // 2003
                            ((fHeader2 == 0x32303032) || (fHeader2 == 0x32303032)) || // 2002
                            ((fHeader2 == 0x32303031) || (fHeader2 == 0x31303032))) // 2001
                {
                    type = File.Type.Mot;
                }
                else if (fHeader == 0x6D6D6700) // mmg
                {
                    type = File.Type.Mmg;
                }
                else if (fHeader == 0x6D6D7800) // mmx
                {
                    type = File.Type.Mmx;
                }
                else if (fHeader == 0x6D6D7000) // mmp
                {
                    type = File.Type.Mmp;
                }
                else if (fHeader == 0x564D472E) // VMG.
                {
                    type = File.Type.Vmg;
                }
                else if (fHeader == 0x564D582E) // VMX.
                {
                    type = File.Type.Vmx;
                }
                //else if ((headerLo == 0x0C08) || (headerLo == 0x0E09)) // VMP.
                else if ((headerLo == 0x080C) || (headerLo == 0x090E)) // VMP.
                {
                    type = File.Type.Vmp;
                }
                else if (fHeader == 0x6C706200) // lpb
                {
                    type = File.Type.Lpb;
                }
                else if (fHeader == 0x76746200) // vtb
                {
                    type = File.Type.Vtb;
                }
                else if (fHeader == 0x5647542E) // VGT.
                {
                    type = File.Type.Vgt;
                }
                //else if ((headerLo == 0x0402)) // VPT.
                else if ((headerLo == 0x0204)) // VPT.
                {
                    type = File.Type.Vpt;
                }

            }
            else
            {
                type = File.Type.Null;
            }

            return type;
        }

        public static string GetFileName(uint fHeader, uint fHeader2, int idx, uint size)
        {
            string entryString;
            string extString = ".unk";

            uint headerHi = (fHeader & 0xFFFF0000);
            //uint headerLo = (fHeader & 0x0000FFFF);
            uint headerLo = (fHeader & 0xFFFF0000) >> 16;

            if (size != 0)
            {

                extString = ".unk";

                if (fHeader2 == 0x6F6C6E6B) // olnk 
                {
                    extString = ".olk";
                }
                //else if ((fHeader2 == 0x80) || (fHeader2 == 0x800) || (fHeader2 == 0x80000000) || (fHeader2 == 0x00080000)) // pkg, find another way to detect this
                else if (FileOLK.DtpCheck(fHeader, fHeader2))
                {
                    extString = ".dtp";
                }
                else if (((fHeader2 == 0x33303032) || (fHeader2 == 0x32303033)) || // 2003
                            ((fHeader2 == 0x32303032) || (fHeader2 == 0x32303032)) || // 2002
                            ((fHeader2 == 0x32303031) || (fHeader2 == 0x31303032))) // 2001
                {
                    extString = ".mot";
                }
                else if (fHeader == 0x6D6D6700) // mmg
                {
                    extString = ".mmg";
                }
                else if (fHeader == 0x6D6D7800) // mmx
                {
                    extString = ".mmx";
                }
                else if (fHeader == 0x6D6D7000) // mmp
                {
                    extString = ".mmp";
                }
                else if (fHeader == 0x564D472E) // VMG.
                {
                    extString = ".vmg";
                }
                else if (fHeader == 0x564D582E) // VMX.
                {
                    extString = ".vmx";
                }
                //else if ((headerLo == 0x0C08) || (headerLo == 0x0E09)) // VMP.
                else if ((headerLo == 0x080C) || (headerLo == 0x090E)) // VMP.
                {
                    extString = ".vmp";
                }
                else if (fHeader == 0x6C706200) // lpb
                {
                    extString = ".lpb";
                }
                else if (fHeader == 0x76746200) // vtb
                {
                    extString = ".vtb";
                }
                else if (fHeader == 0x5647542E) // VGT.
                {
                    extString = ".vgt";
                }
                else if ((headerLo == 0x0204)) // VPT.
                {
                    extString = ".vpt";
                }

                entryString = "File" + (idx + 1) + extString;

            }
            else
            {
                entryString = "*";
            }

            return entryString;
        }

        /* TODO: make get size function */
        public static uint GetOffset(FileStream fs, BinaryReader br, long pkgOffset)
        {
            // endian true = big

            uint nextFileOffset;

            //if ((pkgOffset == 0x80) || (pkgOffset == 0x800)) // big endian
            if (pkgOffset <= 0x800)
            {
                nextFileOffset = Helper.readUInt32B(br);
            }
            else
            {
                nextFileOffset = Helper.readUInt32L(br);
            }

            return nextFileOffset;
        }
    }
}
