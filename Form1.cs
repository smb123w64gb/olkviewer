using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace olkviewer
{
    public partial class OlkViewer : Form
    {
        bool bFileOpened = false;
        int motIndex = 0;

        Olk.Header rootH = new Olk.Header();
        List<File.Entry> fileEntries =  new List<File.Entry>();
        List<Vgt2.Entry> vgtEntries = new List<Vgt2.Entry>();
        List<Mot.Entry> motEntries = new List<Mot.Entry>();
        List<Mmg.ChdpEntry> chdpEntries = new List<Mmg.ChdpEntry>();

        Vgt2.Header vgtHeader = new Vgt2.Header();

        public OlkViewer()
        {
            InitializeComponent();

            /* Right clicking tree selects node */

            treeView1.MouseDown += (sender, args) =>
                treeView1.SelectedNode = treeView1.GetNodeAt(args.X, args.Y);

            treeView2.MouseDown += (sender, args) =>
                treeView2.SelectedNode = treeView2.GetNodeAt(args.X, args.Y);
        }

        public void EnableItems()
        {
            textBox1.Enabled = true;
            treeView1.Enabled = true;
            indexBox.Enabled = true;
            offsetTextBox.Enabled = true;
            sizeTextBox.Enabled = true;
        }

        private void SetText(TextBox textBox, string text)
        {
            textBox.Text = text;
        }

        private static void addNode(TreeNodeCollection nodes, TreeNode newnode, int idx)
        {
            nodes[idx].Nodes.Add(newnode);
        }

        private static void addNode2(TreeNode start, TreeNode newnode)
        {
            if (start.Nodes.Count == 0) start.Nodes.Add(newnode);
            else addNode2(start.Nodes[0], newnode);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //endian = Endian.Big;
                    FileOLK.Load(openFileDialog1.FileName, bFileOpened, textBox1, treeView1, fileEntries);

                    EnableItems();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private int GetIndex2(TreeNode node)
        {
            int returnValue = 0;

            // Always make a way to exit the recursion.
            if (node.Index == 0 && node.Parent == null)
                return returnValue;

            // Now, count every node.
            //returnValue = 1;

            // If I have siblings higher in the index, then count them and their decendants.
            if (node.Index > 0)
            {
                TreeNode previousSibling = node.PrevNode;
                while (previousSibling != null)
                {
                    returnValue++;
                    previousSibling = previousSibling.PrevNode;
                }
            }

            if (node.Parent == null)
                return returnValue;
            else
                return returnValue + GetIndex(node.Parent);
        }

        private int GetIndex(TreeNode node)
        {
            int returnValue = 0;

            // Always make a way to exit the recursion.
            if (node.Index == 0 && node.Parent == null)
                return returnValue;

            // Now, count every node.
            returnValue = 1;

            // If I have siblings higher in the index, then count them and their decendants.
            if (node.Index > 0)
            {
                TreeNode previousSibling = node.PrevNode;
                while (previousSibling != null)
                {
                    returnValue += GetDecendantCount(previousSibling);
                    previousSibling = previousSibling.PrevNode;
                }
            }

            if (node.Parent == null)
                return returnValue;
            else
                return returnValue + GetIndex(node.Parent);
        }

        public int GetDecendantCount(TreeNode node)
        {
            int returnValue = 0;

            // If the node is not the root node, then we want to count it.
            if (node.Index != 0 || node.Parent != null)
                returnValue = 1;

            // Always make a way to exit a recursive function.
            if (node.Nodes.Count == 0)
                return returnValue;

            foreach (TreeNode childNode in node.Nodes)
            {
                returnValue += GetDecendantCount(childNode);
            }
            return returnValue;
        }

        private void Mmg_Entries_Get(FileStream fs, BinaryReader br, Mmg.Header mmgHeader, long fOffset, int Count)
        {
            chdpEntries.Clear();

            listBox1.BeginUpdate();

            List<string> mmgEntriesString = new List<string>();


            for (int i = 0; i < Count; i++)
            {
                Mmg.Entry mmgEntry = new Mmg.Entry(br);

                long nextPos = fs.Position;


                if (mmgEntry.HeaderSize != 0)
                {
                    long chdOffset = mmgEntry.HeaderOffset + fOffset;
                    fs.Seek(chdOffset, SeekOrigin.Begin);

                    // read CHDp

                    FileMMG.GetCHDPEntries(fs, br, mmgEntry, mmgEntriesString, chdpEntries, fOffset, i);
                }

                //mmgEntry.dOffset = (uint)(mmgEntry.DataPtr + mmgEntry.DataOffset + fOffset);

                // go back
                fs.Seek(nextPos, SeekOrigin.Begin);

            }

            

            listBox1.DisplayMember = "asdf";
            listBox1.ValueMember = "0";

            listBox1.DataSource = mmgEntriesString;

            listBox1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int idx = GetIndex(treeView1.SelectedNode);
            int pidx = 0;
            File.Type rootType = File.Type.Olk;

            if (treeView1.SelectedNode.Parent != null)
            {
                pidx = GetIndex(treeView1.SelectedNode.Parent);
                rootType = fileEntries[pidx].Type;
            }

            // test
            int offset = (int)fileEntries[idx].Offset;
            int size = (int)fileEntries[idx].Size;

            int align = (offset + size) % 0x800;
            if (align != 0) { align = 0x800 - align; }
            int endoffset = offset + size + align;
            textBox2.Text = String.Format("{0:X8}", endoffset);

            index2Box.Value = GetIndex2(treeView1.SelectedNode);

            String offsetText = String.Format("0x{0:X}", fileEntries[idx].Offset);
            String sizeText = String.Format("0x{0:X}", fileEntries[idx].Size);

            indexBox.Value = idx;

            treeView2.Nodes.Clear();
            listView1.Items.Clear();

            if (fileEntries[idx].Size != 0)
            {
                if ((rootType == File.Type.Olk))
                {
                    if ((treeView1.SelectedNode.Parent == null))
                    {
                        replaceMenuItem.Enabled = true;
                    }
                    else
                    {
                        replaceMenuItem.Enabled = false;
                    }
                } else
                {
                    replaceMenuItem.Enabled = false;
                }

                extractMenuItem.Enabled = true;
            } else
            {
                extractMenuItem.Enabled = false;
                if ((rootType == File.Type.Olk))
                {
                    if ((treeView1.SelectedNode.Parent == null))
                    {
                        replaceMenuItem.Enabled = true;
                    }
                    else
                    {
                        replaceMenuItem.Enabled = false;
                    }
                }
                else
                {
                    replaceMenuItem.Enabled = false;
                }


            }


            /* make this a switch statement */

            if (FileOLK.Loaded)
            {
                if (fileEntries[idx].Type == File.Type.Vgt)
                {
                    long fOffset = (long)fileEntries[idx].Offset;
                    treeView2.Enabled = true;
                    tabControl1.SelectTab("vgtPage");

                    using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        vgtHeader = FileVGT.GetFiles(fs, br, vgtEntries, treeView2, fOffset);
                    }
                }
                else if (fileEntries[idx].Type == File.Type.Vmg)
                {
                    long fOffset = (long)fileEntries[idx].Offset;
                    treeView2.Enabled = true;

                    tabControl1.SelectTab("vgtPage");

                    using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);

                        fs.Seek(fOffset, SeekOrigin.Begin);

                        Vmg.Header vmgHeader = new Vmg.Header(br);

                        long vgtOffset = vmgHeader.MaterialOffset + fOffset;

                        vgtHeader = FileVGT.GetFiles(fs, br, vgtEntries, treeView2, vgtOffset);
                    }
                }
                else if (fileEntries[idx].Type == File.Type.Mot)
                {
                    long fOffset = (long)fileEntries[idx].Offset;
                    tabControl1.SelectTab("motPage");

                    using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);

                        fs.Seek(fOffset, SeekOrigin.Begin);

                        Mot.Header motHeader = new Mot.Header(br);

                        long entriesOffset = motHeader.EntriesOffset + fOffset;

                        fs.Seek(entriesOffset, SeekOrigin.Begin);

                        FileMOT.GetEntries(fs, br, motEntries, listView1, motHeader, fOffset, motHeader.Count);
                    }
                }
                else if (fileEntries[idx].Type == File.Type.Mmg)
                {
                    long fOffset = (long)fileEntries[idx].Offset;
                    tabControl1.SelectTab("mmgPage");

                    using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);

                        fs.Seek(fOffset, SeekOrigin.Begin);

                        Mmg.Header mmgHeader = new Mmg.Header(br);

                        //long entriesOffset = mmgHeader.Count + fOffset;

                        Mmg_Entries_Get(fs, br, mmgHeader, fOffset, mmgHeader.Count);

                        //Vgt_Files_Get(fs, br, vgtOffset);

                    }
                }
                else
                {
                    treeView2.Enabled = false;
                }
            }

            SetText(offsetTextBox, offsetText);
            SetText(sizeTextBox, sizeText);
        }

        private void extractMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = fileEntries[GetIndex(treeView1.SelectedNode)].Name;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //endian = Endian.Big;
                    //Olk_Load();
                    FileOLK.Extract(openFileDialog1.FileName, saveFileDialog1.FileName, fileEntries[GetIndex(treeView1.SelectedNode)].Offset, (int)fileEntries[GetIndex(treeView1.SelectedNode)].Size);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.FileName = fileEntries[GetIndex(treeView1.SelectedNode)].Name;
            int idx = GetIndex(treeView1.SelectedNode);
            int idx2 = GetIndex2(treeView1.SelectedNode);
            int pidx = 0;
            int offset = 0;
            int rootOffset = 0;
            File.Type rootType = File.Type.Olk;

            if (replaceFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //endian = Endian.Big;

                    if (treeView1.SelectedNode.Parent != null)
                    {
                        pidx = GetIndex(treeView1.SelectedNode.Parent);

                        idx2 = (idx2 - pidx) - 1;
                        rootType = fileEntries[pidx].Type;
                        rootOffset = (int)fileEntries[pidx].Offset;
                    }
                    else
                    {
                        //idx2 -= 1;
                    }

                    offset = (int)fileEntries[idx].Offset;

                    FileOLK.Replace(openFileDialog1.FileName, replaceFileDialog.FileName, idx2, rootOffset, offset, (int)fileEntries[idx].Size);

                    MessageBox.Show("Finished!");

                    /* reload file */
                    FileOLK.Load(openFileDialog1.FileName, bFileOpened, textBox1, treeView1, fileEntries);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

            Vgt2.Entry.EType ImgType = vgtEntries[GetIndex(treeView2.SelectedNode)].Diffuse.texImage0.texture_format;

            xBox.Value = vgtEntries[GetIndex(treeView2.SelectedNode)].dX;
            yBox.Value = vgtEntries[GetIndex(treeView2.SelectedNode)].dY;

            Bitmap testimg = new Bitmap(256, 256);
            using (Graphics gfx = Graphics.FromImage(testimg))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, 256, 256);
            }
            OLKImagePreview.Image = testimg;

            int entrySize = 0x44;
            long hOffset = vgtHeader.dOffset + (entrySize * GetIndex(treeView2.SelectedNode));

            String textureHeaderOffsetText = String.Format("0x{0:X}", hOffset);
            String textureOffsetText = String.Format("0x{0:X}", vgtEntries[GetIndex(treeView2.SelectedNode)].dOffset);

            SetText(textureHeaderOffTextBox, textureHeaderOffsetText);
            SetText(textureOffTextBox, textureOffsetText);

            // TODO: Figure out how to get mipmap count SMB: I gotchu fam
            if (vgtEntries[GetIndex(treeView2.SelectedNode)].dMipCount > 0 )
            {
                mipmapCheckBox.Checked = true;
                mipmapNumBox.Value = vgtEntries[GetIndex(treeView2.SelectedNode)].dMipCount;
            }
            else
            {
                mipmapCheckBox.Checked = false;
                mipmapNumBox.Value = 0;
            }

            if (vgtEntries[GetIndex(treeView2.SelectedNode)].Alpha.CLUTOffset != 0)
            {
                alphaCheckBox.Checked = true;
            }
            else
            {
                alphaCheckBox.Checked = false;
            }

            if (ImgType == Vgt2.Entry.EType.CMPR)
            {
                exportPNGItem.Enabled = false;
            } else
            {
                exportPNGItem.Enabled = true;
            }

            if (ImgType == Vgt2.Entry.EType.CMPR)
            {
                Vgt2.Entry vgtEntry = vgtEntries[GetIndex(treeView2.SelectedNode)];
                Vgt2.Entry.EType eType = vgtEntries[GetIndex(treeView2.SelectedNode)].Diffuse.texImage0.texture_format;
                Bitmap Diffuse = FileVGT.RenderImage(openFileDialog1.FileName, vgtEntry.dOffset, (int)vgtEntry.Diffuse.texImage0.width, (int)vgtEntry.Diffuse.texImage0.height, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                
                /*if (vgtEntry.Alpha.CLUTOffset > 0)
                {
                    Bitmap Alpha = FileVGT.RenderImage(openFileDialog1.FileName, vgtEntry.dOffset2, (int)vgtEntry.Alpha.texImage0.width, (int)vgtEntry.Alpha.texImage0.height, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);

                    Rectangle Drect = new Rectangle(0, 0, vgtEntry.Diffuse.texImage0.width, vgtEntry.Diffuse.texImage0.height);
                    System.Drawing.Imaging.BitmapData DbmpData = Diffuse.LockBits(Drect, System.Drawing.Imaging.ImageLockMode.ReadWrite, Diffuse.PixelFormat);
                    IntPtr Dptr = DbmpData.Scan0;
                    int Dbytes = Math.Abs(DbmpData.Stride) * vgtEntry.Diffuse.texImage0.height;
                    byte[] DrgbValues = new byte[Dbytes];
                    System.Runtime.InteropServices.Marshal.Copy(Dptr, DrgbValues, 0, Dbytes);





                    Rectangle Arect = new Rectangle(0, 0, vgtEntry.Alpha.texImage0.width, vgtEntry.Alpha.texImage0.height);
                    System.Drawing.Imaging.BitmapData AbmpData = Alpha.LockBits(Arect, System.Drawing.Imaging.ImageLockMode.ReadWrite, Alpha.PixelFormat);
                    IntPtr Aptr = AbmpData.Scan0;
                    int Abytes = Math.Abs(DbmpData.Stride) * vgtEntry.Alpha.texImage0.height;
                    byte[] ArgbValues = new byte[Abytes];
                    System.Runtime.InteropServices.Marshal.Copy(Aptr, ArgbValues, 0, Abytes);

                    for (int counter = 3; counter < DrgbValues.Length; counter += 4)
                    {
                        DrgbValues[counter] = ArgbValues[counter - 1];
                    }

                    System.Runtime.InteropServices.Marshal.Copy(DrgbValues, 0, Dptr, Dbytes);
                    Diffuse.UnlockBits(DbmpData);

                }*/

                OLKImagePreview.Image = Diffuse;
                OLKImagePreview.BackColor = Color.Transparent;
            }else if(ImgType == Vgt2.Entry.EType.C8 || ImgType == Vgt2.Entry.EType.C4)
            {
                Vgt2.Entry vgtEntry = vgtEntries[GetIndex(treeView2.SelectedNode)];
                Bitmap Diffuse = FileVGT.RenderImage(openFileDialog1.FileName, vgtEntry);
                OLKImagePreview.Image = Diffuse;
                OLKImagePreview.BackColor = Color.Transparent;
            }
            //OLKImagePreview.Image = null;
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Vgt2.Entry vgtEntry = vgtEntries[GetIndex(treeView2.SelectedNode)];
            Vgt2.Entry.EType eType = vgtEntries[GetIndex(treeView2.SelectedNode)].Diffuse.texImage0.texture_format;
            vgtExportDialog.Filter = "DDS files|*.dds|All files|*.*";
            if (vgtEntry.Diffuse.texImage0.texture_format == Vgt2.Entry.EType.CMPR)
            {
                // do stuff

                /* write vgt */
                string fn = GetIndex(treeView2.SelectedNode) + "-" + vgtEntry.Diffuse.texImage0.texture_format + ".dds";

                vgtExportDialog.FileName = fn;

                if (vgtExportDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileVGT.Export(openFileDialog1.FileName, vgtExportDialog.FileName, vgtEntry.dOffset, (int)xBox.Value, (int)yBox.Value, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }

                /* alpha */
                if (alphaCheckBox.Checked)
                {
                    /* write vgt */
                    string fna = GetIndex(treeView2.SelectedNode) + "-" + vgtEntry.Diffuse.texImage0.texture_format + "_a" + ".dds";

                    vgtExportDialog.FileName = fna;

                    if (vgtExportDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            FileVGT.Export(openFileDialog1.FileName, vgtExportDialog.FileName, vgtEntry.dOffset2, (int)xBox.Value, (int)yBox.Value, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                        }
                        catch (SecurityException ex)
                        {
                            MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                            $"Details:\n\n{ex.StackTrace}");
                        }
                    }
                }
            }
            else
            {
                //pop up box

                string msg = "Not supported!";
                MessageBox.Show(msg);
            }

        }

        private void importToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Vgt2.Entry vgtEntry = vgtEntries[GetIndex(treeView2.SelectedNode)];

            if (vgtEntry.Diffuse.texImage0.texture_format == Vgt2.Entry.EType.CMPR)
            {
                // do stuff
                if (vgtImportDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileVGT.Import(vgtImportDialog.FileName, openFileDialog1.FileName, vgtEntry.dOffset, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }

                /* alpha */
                if (alphaCheckBox.Checked)
                {
                    if (vgtImportDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            FileVGT.Import(vgtImportDialog.FileName, openFileDialog1.FileName, vgtEntry.dOffset2, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                        }
                        catch (SecurityException ex)
                        {
                            MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                            $"Details:\n\n{ex.StackTrace}");
                        }
                    }
                }

                MessageBox.Show("Texture imported!");
            } else
            {
                string msg = "Not supported!";
                MessageBox.Show(msg);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mot.Entry motEntry = motEntries[motIndex];

            if (motExportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Vgt_Import(vgtEntry.dOffset, checkBox1.Checked);
                    FileMOT.Export(openFileDialog1.FileName, motExportDialog.FileName, motEntry.dOffset);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.FocusedItem == null) return;
            //index2Box.Value =
            motIndex = listView1.FocusedItem.Index;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutF = new AboutForm();

            aboutF.StartPosition = FormStartPosition.CenterScreen;
            aboutF.Show();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Mmg_Extract()
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

                /* get adpcm header */
                fs.Seek(headerOffset, SeekOrigin.Begin);

                dspHeader = br.ReadBytes(0x60);

                /* get adpcm data */
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

        private void Mmg_Extract_All()
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

                    /* get adpcm header */
                    fs.Seek(headerOffset, SeekOrigin.Begin);

                    dspHeader = br.ReadBytes(0x60);

                    /* get adpcm data */
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

        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //MessageBox.Show(folderBrowserDialog1.SelectedPath);
                    Mmg_Extract_All();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int idx = listBox1.SelectedIndex;

            textBox4.Text = String.Format("{0:X8}", chdpEntries[idx].dHOffset);
            textBox3.Text = String.Format("{0:X8}", chdpEntries[idx].dSize);
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = listBox1.SelectedIndex;

            string dspFn = listBox1.GetItemText(listBox1.Items[i]) + ".dsp";
            saveFileDialog2.FileName = dspFn;


            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    //MessageBox.Show(folderBrowserDialog1.SelectedPath);
                    Mmg_Extract();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void exportPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] ImageData = new byte[16];
            byte[] Palette = new byte[16];

            Vgt2.Entry vgtEntry = vgtEntries[GetIndex(treeView2.SelectedNode)];
            Vgt2.Entry.EType eType = vgtEntries[GetIndex(treeView2.SelectedNode)].Diffuse.texImage0.texture_format;

            int x = (int)xBox.Value;
            int y = (int)yBox.Value;

            int type = (int)vgtEntry.Diffuse.texImage0.texture_format;

            vgtExportDialog.Filter = "PNG files|*.png|All files|*.*";
            // set C4/C8 to I4/I8 for now
            if (type == 9)
            {
                type = 1;
            } else if (type == 8)
            {
                type = 0;
            }

            if (//eType != Vgt2.Entry.EType.UNK ||
                eType == Vgt2.Entry.EType.C4 ||
                eType == Vgt2.Entry.EType.C8)
            {
                string fn = GetIndex(treeView2.SelectedNode) + "-" + vgtEntry.Diffuse.texImage0.texture_format + ".png";
                vgtExportDialog.FileName = fn;

                if (vgtExportDialog.ShowDialog() == DialogResult.OK)
                {
                    try { 
                    OLKImagePreview.Image.Save(vgtExportDialog.FileName);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }
                
            } else
            {
                // do stuff

                /* write vgt */
                string fn = GetIndex(treeView2.SelectedNode) + "-" + vgtEntry.Diffuse.texImage0.texture_format + ".png";

                vgtExportDialog.FileName = fn;

                if (vgtExportDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int size = (x * y) * 4;

                        byte[] dataBuffer = FileVGT.GetTextureData_C8(openFileDialog1.FileName, vgtEntry.dOffset, size);

                        //FileVGT.Export(openFileDialog1.FileName, vgtExportDialog.FileName, vgtEntry.dOffset, (int)xBox.Value, (int)yBox.Value, mipmapCheckBox.Checked, (int)mipmapNumBox.Value);
                        ImageData = Texture.Load(dataBuffer, 0, x, y, (Vgt.Type)type, Palette, Vgt.PaletteType.RGBA5A3);
                        Texture.SavePNG(vgtExportDialog.FileName, ImageData, x, y);

                        /* debug */
                        //string debugfn = vgtExportDialog.FileName + ".dds";
                        //FileVGT.WriteTextureData(debugfn, ImageData);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }

            }
        }
    }
}
