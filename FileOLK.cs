using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace olkviewer
{
    class FileOLK
    {
        public static Olk.Header rootH = new Olk.Header();
        public static bool Loaded = false;

        private static void addNode(TreeNodeCollection nodes, TreeNode newnode, int idx)
        {
            nodes[idx].Nodes.Add(newnode);
        }

        private static void addNode2(TreeNode start, TreeNode newnode)
        {
            if (start.Nodes.Count == 0) start.Nodes.Add(newnode);
            else addNode2(start.Nodes[0], newnode);
        }

        private static void SetText(TextBox textBox, string text)
        {
            textBox.Text = text;
        }

        public static bool DtpCheck(uint fHeader, uint fHeader2)
        {
            if ((((fHeader > 0) &&(fHeader < 0x200)) && ((fHeader2 >= 0x10) && (fHeader2 <= 0x800))) ||
                (((Helper.swap32(fHeader) > 0) && (Helper.swap32(fHeader) < 0x200)) && ((Helper.swap32(fHeader2) >= 0x10) && (Helper.swap32(fHeader2) <= 0x800))))
            {
                return true;
            }

            return false;
        }

        public static File.Type GetFileType(Olk.Entry olkEntry, uint fHeader, uint fHeader2)
        {
            File.Type type = File.Type.Unk;

            //uint headerLo = fHeader & 0xFFFF;
            uint headerLo = (fHeader & 0xFFFF0000) >> 16;

            if (olkEntry.Size != 0)
            {
                if (fHeader2 == 0x6F6C6E6B) // olnk 
                {
                    type = File.Type.Olk;
                }
                //else if ((fHeader2 == 0x80) || (fHeader2 == 0x800) || (fHeader2 == 0x80000000) || (fHeader2 == 0x00080000)) // pkg, find another way to detect this
                else if (DtpCheck(fHeader, fHeader2))
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
                else if (fHeader == 0x50415200) // PAR
                {
                    type = File.Type.Par;
                }

            }
            else
            {
                type = File.Type.Null;
            }

            return type;
        }

        public static string GetFileName(Olk.Entry olkEntry, uint fHeader, uint fHeader2, int idx, long fOffset)
        {
            string entryString;
            string extString = ".unk";

            //uint headerLo = fHeader & 0xFFFF;
            uint headerLo = (fHeader & 0xFFFF0000) >> 16;

            if (olkEntry.Size != 0)
            {

                extString = ".unk";

                if (fHeader2 == 0x6F6C6E6B) // olnk 
                {
                    extString = ".olk";
                }
                //else if ((fHeader2 == 0x80) || (fHeader2 == 0x800) || (fHeader2 == 0x80000000) || (fHeader2 == 0x00080000)) // pkg, find another way to detect this
                else if (DtpCheck(fHeader, fHeader2))
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
                else if (fHeader == 0x50415200) // PAR
                {
                    extString = ".par";
                }

                entryString = "File" + (idx + 1) + extString;
                //string entryString2 = "File_" + olkEntry.Offset.ToString("x8") + extString;

            }
            else
            {
                entryString = "*";
            }

            return entryString;
        }


        public static void Extract(string SrcFileName, string DestFileName, uint Offset, int Size)
        {
            byte[] bytes = null;
            //int align;
            //int totalSize;

            using (FileStream fs = new FileStream(SrcFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                fs.Seek((long)Offset, SeekOrigin.Begin);
                bytes = br.ReadBytes(Size);

                /* to do: align size by 0x800?*/

                /*
                align = Size % 0x800;
                if (align != 0) { align = 0x800 - align; }
                totalSize = Size + align;
                */
                /* pad end */
                //Array.Resize(ref bytes, totalSize);

            }

            using (FileStream fs = new FileStream(DestFileName, FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, false))
                {
                    bw.Write(bytes);

                    /* clear buffers? */
                    bytes = null;
                    fs.Flush();
                    GC.Collect();
                }

            }

        }

        public static void ReadEntry(FileStream fs, BinaryReader br, List<File.Entry> fileEntries, TreeView treeView1, Olk.Header rootHeader, Olk.Entry rootEntry, long parentOffset, int parentIdx, bool isChild)
        {
            List<Olk.Entry> entries = new List<Olk.Entry>();

            entries.Clear();


            for (int i = 0; i < rootHeader.Count; i++)
            {
                var olkEntry = new Olk.Entry(br);
                var fileEntry = new File.Entry();

                string fileName = "*";


                //fileEntries.Add(fileEntry);
                //entryList.Add(olkEntry.Offset);
                entries.Add(olkEntry);

                long pos = fs.Position;
                long fOffset2 = 0;
                uint fHeader = 0;
                uint fHeader2 = 0;


                if ((olkEntry.Size != 0) || ((olkEntry.Size != 0) && (olkEntry.Offset != 0)))
                {
                    fOffset2 = olkEntry.Offset + rootEntry.Offset + parentOffset; /// + olkEntry.Offset;//olkEntry.Offset;

                    /* Check file type */
                    fs.Seek(fOffset2, SeekOrigin.Begin);

                    fHeader = Helper.readUInt32B(br);
                    fHeader2 = Helper.readUInt32B(br);

                    fs.Seek(pos, SeekOrigin.Begin);

                    File.Type type = GetFileType(olkEntry, fHeader, fHeader2);
                    fileName = GetFileName(olkEntry, fHeader, fHeader2, i, fOffset2);


                    /* add entry */
                    TreeNode tn = new TreeNode(fileName);

                    if (!isChild)
                    {
                        treeView1.Nodes.Add(tn);
                    }
                    else
                    {
                        addNode(treeView1.Nodes, tn, parentIdx);
                    }

                    fileEntry.Name = fileName;
                    fileEntry.Offset = (uint)fOffset2; //olkEntry.Offset + rootEntry.Offset;// + parentOffset;
                    fileEntry.Size = olkEntry.Size;
                    fileEntry.Type = type;

                    fileEntries.Add(fileEntry);


                    if (fHeader2 == 0x6F6C6E6B) // olnk
                    {
                        fs.Seek(fOffset2, SeekOrigin.Begin);
                        var subRootHeader = new Olk.Header(br);
                        var subOlkEntry2 = new Olk.Entry(br);

                        long pos2 = fs.Position;

                        //fOffset2 += 0x20; //olkEntry.Offset + 0x20;
                        long fOffset3 = 0;
                        fOffset3 += fOffset2 + 0x20;

                        //var subOlkEntry2 = new sc2.Olk.Entry(br);
                        //if (subOlkEntry2.Size != 0)
                        //{
                        fs.Seek(fOffset2 + 0x20, SeekOrigin.Begin);
                        ReadEntry(fs, br, fileEntries, treeView1, subRootHeader, subOlkEntry2, fOffset2, parentIdx, true);
                        //}

                    }
                    //else if ((fHeader2 == 0x80) || (fHeader2 == 0x800) || (fHeader2 == 0x80000000) || (fHeader2 == 0x00080000))
                    else if (DtpCheck(fHeader, fHeader2))
                    {
                        TreeNode tn2 = new TreeNode("*");
                        uint pkgCount = 0;
                        uint pkgCount2 = 0;
                        uint pkgfHeader = 0;
                        uint pkgfHeader2 = 0;
                        uint pkgSize = 0;
                        long pkgFileOffset = 0;
                        long prevFileOffset = 0;
                        long nextFileOffset = 0;

                        pkgSize = olkEntry.Size;


                        /* parse pkg */
                        //treeView1.Nodes[parentIdx].Nodes.Add(fileName);

                        /* read header */
                        fs.Seek(fOffset2 + 4, SeekOrigin.Begin);
                        uint pkg0Offset = Helper.readUInt32B(br);

                        //if ((pkg0Offset == 0x80) || (pkg0Offset == 0x800)) // big endian
                        if (pkg0Offset <= 0x800)
                        {
                            fs.Seek(fOffset2, SeekOrigin.Begin);
                            pkgCount = Helper.readUInt32B(br);
                            pkgCount += 1;
                        }
                        //else if ((pkg0Offset == 0x80000000) || (pkg0Offset == 0x00080000)) // little endian
                        else //if (Helper.swap32(pkg0Offset) <= 0x800)
                        {
                            fs.Seek(fOffset2, SeekOrigin.Begin);
                            pkgCount = Helper.readUInt32L(br);
                            pkgCount += 1;
                        }

                        fs.Seek(fOffset2 + 4, SeekOrigin.Begin);

                        for (int j = 0; j < pkgCount; j++)
                        {
                            string pkgFileName = "*";
                            var pkgFileEntry = new File.Entry();


                            long prevFilePos = fs.Position - 4;
                            long nextFilePos = fs.Position + 4;

                            long curPos = fs.Position;
                            if (j != 0)
                            {

                                fs.Seek(prevFilePos, SeekOrigin.Begin);
                                //if (pkg0Offset == 0x80) // big endian
                                if (pkg0Offset <= 0x800)
                                {
                                    prevFileOffset = Helper.readUInt32B(br);
                                }
                                else
                                {
                                    prevFileOffset = Helper.readUInt32L(br);
                                }

                            }
                            else
                            {
                                prevFileOffset = 0;
                            }


                            //if ((pkg0Offset == 0x80) || (pkg0Offset == 0x800)) // big endian
                            if (pkg0Offset <= 0x800)
                            {
                                pkgFileOffset = Helper.readUInt32B(br);
                            }
                            else
                            {
                                pkgFileOffset = Helper.readUInt32L(br);
                            }

                            long pkgPos = fs.Position;

                            if (j != (pkgCount - 1))
                            {
                                fs.Seek(nextFilePos, SeekOrigin.Begin);
                                nextFileOffset = FilePKG.GetOffset(fs, br, pkg0Offset);
                            }
                            else
                            {
                                nextFileOffset = fOffset2 + pkgSize;
                            }

                            if (nextFileOffset == pkgFileOffset)
                            {

                                for (int k = 0; k < (pkgCount - j); k++)
                                {
                                    //fs.Seek(nextFilePos + (k * 4), SeekOrigin.Begin);
                                    if (k == (pkgCount - j - 1))
                                    {
                                        nextFileOffset = fOffset2 + pkgSize;
                                    }
                                    else
                                    {
                                        nextFileOffset = FilePKG.GetOffset(fs, br, pkg0Offset);
                                    }

                                    if (nextFileOffset != pkgFileOffset)
                                    {
                                        break;
                                    }
                                }
                            }

                            fs.Seek(pkgPos, SeekOrigin.Begin);

                            File.Type pkgType = File.Type.Unk;

                            long pkgFileOffset2 = pkgFileOffset + fOffset2;
                            long prevFileOffset2 = prevFileOffset + fOffset2;
                            long nextFileOffset2 = nextFileOffset + fOffset2;



                            if ((pkgFileOffset != prevFileOffset) && (pkgFileOffset != pkgSize))
                            {

                                //if ((pkg0Offset == 0x80) || (pkg0Offset == 0x800)) // big endian
                                //if (pkg0Offset <= 0x800)
                                //{
                                    fs.Seek(pkgFileOffset2, SeekOrigin.Begin);
                                    pkgfHeader = Helper.readUInt32B(br);
                                    pkgfHeader2 = Helper.readUInt32B(br);
                                //}
                                /*else
                                {
                                    fs.Seek(pkgFileOffset2, SeekOrigin.Begin);
                                    pkgfHeader = Helper.readUInt32L(br);
                                    pkgfHeader2 = Helper.readUInt32L(br);
                                }
                                */
                                /* read file header */

                                /* add entry */
                                //tn2 = new TreeNode("File" + j + ".unk");
                                pkgType = FilePKG.GetFileType(1, pkgfHeader, pkgfHeader2);
                                pkgFileName = FilePKG.GetFileName(pkgfHeader, pkgfHeader2, j, 1);

                                tn.Nodes.Add(pkgFileName);

                                uint nextOff = 0;

                                if ((j == pkgCount - 1) && (pkgFileOffset == pkgSize)) // last item
                                {
                                    nextOff = (uint)(fOffset2 + pkgSize);// (pkgFileOffset + pkgSize);
                                }
                                else if (prevFileOffset == pkgFileOffset)
                                {
                                    nextOff = (uint)pkgFileOffset2;
                                }
                                else
                                {
                                    nextOff = (uint)nextFileOffset2;
                                }
                                pkgFileEntry.Size = (uint)(nextOff - pkgFileOffset2);

                            }
                            else
                            {

                                pkgType = FilePKG.GetFileType(0, pkgfHeader, pkgfHeader2);
                                pkgFileName = FilePKG.GetFileName(pkgfHeader, pkgfHeader2, j, 0);
                                tn.Nodes.Add(pkgFileName);

                                pkgFileEntry.Size = 0;
                            }

                            pkgFileEntry.Name = pkgFileName;
                            pkgFileEntry.Offset = (uint)pkgFileOffset2;
                            pkgFileEntry.Type = pkgType;


                            //textBox2.Text = String.Format("{0:X8", nextOff);


                            fileEntries.Add(pkgFileEntry);

                            pkgCount2++;
                            fs.Seek(pkgPos, SeekOrigin.Begin);
                        }

                        fs.Seek(pos, SeekOrigin.Begin);

                    }

                }
                else
                {
                    TreeNode tn = new TreeNode(fileName);

                    if (!isChild)
                    {
                        treeView1.Nodes.Add(tn);
                    }
                    else
                    {
                        addNode(treeView1.Nodes, tn, parentIdx);
                    }


                    fileEntry.Name = fileName;
                    fileEntry.Offset = olkEntry.Offset + rootEntry.Offset + (uint)parentOffset;
                    fileEntry.Size = olkEntry.Size;
                    fileEntry.Type = File.Type.Null;

                    fileEntries.Add(fileEntry);
                }

                if (!isChild)
                {
                    parentIdx++;
                }

                fs.Seek(pos, SeekOrigin.Begin);
            }
        }

        // TODO: Cleanup
        public static void Read(FileStream fs, BinaryReader br, string OlkFileName, TextBox textBox1, TreeView treeView1, List<File.Entry> fileEntries)
        {
            /* Read OLK Header */
            //var
            rootH = new Olk.Header(br);


            string fCount = "count " + rootH.Count;

            /* Confirm olk file */

            if (rootH.Magic != 0x6B6E6C6F) // olnk 0x6F6C6E6B
            {
                return;
            }

            SetText(textBox1, OlkFileName);
            //textBox1.Enabled = true;
            //treeView1.Enabled = true;

            //FIX
            //EnableItems();


            /* Get Root Info */
            var rootEntry = new Olk.Entry(br);

            long pos = fs.Position;


            /* clear tree */
            treeView1.Nodes.Clear();
            fileEntries.Clear();


            /* Populate List */

            treeView1.BeginUpdate();

            fs.Seek(pos, SeekOrigin.Begin);

            ReadEntry(fs, br, fileEntries, treeView1, rootH, rootEntry, 0, 0, false);
            treeView1.EndUpdate();
        }

        public static void Load(string OlkFileName, bool bFileOpened, TextBox textBox1, TreeView treeView1, List<File.Entry> fileEntries)
        {
            Loaded = false;

            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                bFileOpened = true;

                Read(fs, br, OlkFileName, textBox1, treeView1, fileEntries);
            }

            Loaded = true;

        }

        public static void Replace(string OlkFileName, string SrcFileName, int idx, int rootOffset, int Offset, int Size)
        {
            byte[] data = null;// = new byte[0];
            bool fileLarger = false;
            int newSize = 0;
            int offsetDiff = 0;

            int align = 0;
            int align2 = 0;
            int align3 = 0;

            int totalSize = 0;

            int endoffset = 0;
            int endoffset2 = 0;

            int oldOlkSize = 0;
            int newOlkSize = 0;

            using (FileStream fs = new FileStream(SrcFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);

                newSize = (int)fs.Length;

                /* original */
                align = (Offset + Size) % 0x800;
                if (align != 0) { align = 0x800 - align; }
                endoffset = Offset + Size + align;

                /* new */
                align2 = (Offset + newSize) % 0x800;
                if (align2 != 0) { align2 = 0x800 - align2; }
                endoffset2 = Offset + newSize + align2;

                align3 = newSize % 0x800;
                if (align3 != 0) { align3 = 0x800 - align3; }
                totalSize = newSize + align3;

                if (fs.Length > Size)
                {
                    //MessageBox.Show("New file is larger than old!");
                    fileLarger = true;

                    offsetDiff = endoffset2 - endoffset;
                    //return;
                }

                // read file data
                data = br.ReadBytes(newSize);

                /* pad end */
                Array.Resize(ref data, totalSize);

            }

            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                using (var br = new BinaryReader(fs))
                {
                    using (var bw = new BinaryWriter(fs, Encoding.UTF8, false))
                    {
                        /* Write data if same size or smaller */

                        if (!fileLarger)
                        {
                            fs.Seek(Offset, SeekOrigin.Begin);
                            bw.Write(data);
                        }
                        else
                        {
                            byte[] data1 = null;
                            byte[] data2 = null;

                            align = (Offset + Size) % 0x800;
                            if (align != 0) { align = 0x800 - align; }
                            endoffset = Offset + Size + align;

                            int size1 = (int)Offset;
                            int size2 = (int)fs.Length - endoffset; // get size by Offset + Size + (0x800 - ((Offset + Size) % 0x800)) ?


                            /* get old size */
                            fs.Seek(0, SeekOrigin.End);
                            oldOlkSize = (int)new System.IO.FileInfo(OlkFileName).Length;

                            /* read before/after file */

                            fs.Seek(0, SeekOrigin.Begin);
                            data1 = br.ReadBytes(size1);
                            fs.Seek(endoffset, SeekOrigin.Begin);
                            data2 = br.ReadBytes(size2);

                            /* write data */
                            fs.Seek(0, SeekOrigin.Begin);
                            bw.Write(data1); // before new file
                            bw.Write(data); // new file
                            bw.Write(data2); // after new file

                            //data2.c

                            /* fix offsets */
                            fs.Seek(rootOffset, SeekOrigin.Begin);
                            int fcount = br.ReadInt32(); // 

                            /* write new size */
                            int entryAddress = rootOffset + 0x20 + (idx * 0x10);
                            fs.Seek(entryAddress + 4, SeekOrigin.Begin);
                            bw.Write(newSize);

                            /* fix other offsets */
                            for (int i = idx + 1; i < fcount; i++)
                            {
                                int nextAddr = rootOffset + 0x20 + (i * 0x10);
                                fs.Seek(nextAddr, SeekOrigin.Begin);
                                int entryoffset = br.ReadInt32();
                                //uint entryOffset = fileEntries[i].Offset;
                                entryoffset += offsetDiff;
                                fs.Seek(nextAddr, SeekOrigin.Begin);
                                bw.Write(entryoffset);
                            }

                            /* fix root size */
                            newOlkSize = (int)new System.IO.FileInfo(OlkFileName).Length;
                            int sizeAddr = rootOffset + 0x14;
                            int dataAddr = rootOffset + 0x10;
                            fs.Seek(dataAddr, SeekOrigin.Begin);
                            int dataOff = br.ReadInt32();
                            newOlkSize -= dataOff;
                            fs.Seek(sizeAddr, SeekOrigin.Begin);
                            bw.Write(newOlkSize);

                            /* clear buffers? */
                            data1 = null;
                            data2 = null;
                            //data = null;

                            //fs.Flush();
                            //GC.Collect();

                            /* test *//*
                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                using (FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                                {
                                    BinaryReader br2 = new BinaryReader(fs2);
                                    using (var bw2 = new BinaryWriter(fs2, Encoding.UTF8, false))
                                    {
                                        fs2.Seek(0, SeekOrigin.Begin);
                                        bw2.Write(data1); // before new file
                                        bw2.Write(data); // new file
                                        bw2.Write(data2); // after new file

                                        /* fix offsets *//*
                                        fs2.Seek(rootOffset, SeekOrigin.Begin);
                                        int fcount = br2.ReadInt32(); // 

                                        /* write new size *//*
                            int entryAddress = rootOffset + 0x20 + (idx * 0x10);
                                        fs2.Seek(entryAddress + 4, SeekOrigin.Begin);
                                        bw2.Write(newSize);

                                        /* fix other offsets *//*
                            for (int i = idx + 1; i < fcount; i++)
                                        {
                                            int nextAddr = rootOffset + 0x20 + (i * 0x10);
                                            fs2.Seek(nextAddr, SeekOrigin.Begin);
                                            int entryoffset = br2.ReadInt32();
                                            uint entryOffset = fileEntries[i].Offset;
                                            entryoffset += offsetDiff;
                                            fs2.Seek(nextAddr, SeekOrigin.Begin);
                                            bw2.Write(entryoffset);
                                        }
                                    }
                                }
                            }*/


                        }

                        /* clear buffers? */
                        data = null;

                    }

                    //fs.Flush();
                    GC.Collect();
                }
            }

            /* test *//*
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs, Encoding.UTF8, false))
                    {
                        bw.Write(data);
                    }
                }
            }*/
        }
    }
}
