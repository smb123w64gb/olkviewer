
namespace olkviewer
{
    partial class OlkViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OlkViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.fileSelectMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeTextBox = new System.Windows.Forms.TextBox();
            this.indexBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.gfxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportPNGItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xBox = new System.Windows.Forms.NumericUpDown();
            this.yBox = new System.Windows.Forms.NumericUpDown();
            this.mipmapCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.vgtExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.alphaCheckBox = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.vgtImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.vgtPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textureOffTextBox = new System.Windows.Forms.TextBox();
            this.textureHeaderOffTextBox = new System.Windows.Forms.TextBox();
            this.mipmapNumBox = new System.Windows.Forms.NumericUpDown();
            this.vmgPage = new System.Windows.Forms.TabPage();
            this.motPage = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.motIndexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.motAnimHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.motOffsetHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.motMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mmgPage = new System.Windows.Forms.TabPage();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.mmgMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.motExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.index2Box = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.replaceFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.OLKImagePreview = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.fileSelectMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.indexBox)).BeginInit();
            this.gfxMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.vgtPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mipmapNumBox)).BeginInit();
            this.motPage.SuspendLayout();
            this.motMenuStrip.SuspendLayout();
            this.mmgPage.SuspendLayout();
            this.mmgMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.index2Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OLKImagePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.helpButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1069, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            this.exitMenuItem});
            this.FileButton.Image = ((System.Drawing.Image)(resources.GetObject("FileButton.Image")));
            this.FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileButton.Name = "FileButton";
            this.FileButton.ShowDropDownArrow = false;
            this.FileButton.Size = new System.Drawing.Size(29, 22);
            this.FileButton.Text = "File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Enabled = false;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveMenuItem.Text = "Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // helpButton
            // 
            this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.helpButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpButton.Image = ((System.Drawing.Image)(resources.GetObject("helpButton.Image")));
            this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpButton.Name = "helpButton";
            this.helpButton.ShowDropDownArrow = false;
            this.helpButton.Size = new System.Drawing.Size(36, 22);
            this.helpButton.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.fileSelectMenuStrip;
            this.treeView1.Enabled = false;
            this.treeView1.Location = new System.Drawing.Point(12, 44);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(211, 480);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // fileSelectMenuStrip
            // 
            this.fileSelectMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractMenuItem,
            this.replaceMenuItem});
            this.fileSelectMenuStrip.Name = "fileSelectMenuStrip";
            this.fileSelectMenuStrip.Size = new System.Drawing.Size(125, 48);
            // 
            // extractMenuItem
            // 
            this.extractMenuItem.Name = "extractMenuItem";
            this.extractMenuItem.Size = new System.Drawing.Size(124, 22);
            this.extractMenuItem.Text = "Extract...";
            this.extractMenuItem.Click += new System.EventHandler(this.extractMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Enabled = false;
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.Size = new System.Drawing.Size(124, 22);
            this.replaceMenuItem.Text = "Replace...";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(248, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(246, 20);
            this.textBox1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "OLK files|*.olk|All files|*.*";
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Enabled = false;
            this.offsetTextBox.Location = new System.Drawing.Point(311, 114);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.Size = new System.Drawing.Size(130, 20);
            this.offsetTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(258, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Offset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Size";
            // 
            // sizeTextBox
            // 
            this.sizeTextBox.Enabled = false;
            this.sizeTextBox.Location = new System.Drawing.Point(311, 148);
            this.sizeTextBox.Name = "sizeTextBox";
            this.sizeTextBox.Size = new System.Drawing.Size(130, 20);
            this.sizeTextBox.TabIndex = 6;
            // 
            // indexBox
            // 
            this.indexBox.Enabled = false;
            this.indexBox.Location = new System.Drawing.Point(311, 73);
            this.indexBox.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.indexBox.Name = "indexBox";
            this.indexBox.Size = new System.Drawing.Size(56, 20);
            this.indexBox.TabIndex = 7;
            this.indexBox.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(261, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Index";
            this.label3.Visible = false;
            // 
            // treeView2
            // 
            this.treeView2.ContextMenuStrip = this.gfxMenuStrip;
            this.treeView2.Enabled = false;
            this.treeView2.Location = new System.Drawing.Point(12, 121);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(246, 200);
            this.treeView2.TabIndex = 9;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // gfxMenuStrip
            // 
            this.gfxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem1,
            this.importToolStripMenuItem1,
            this.toolStripSeparator1,
            this.exportPNGItem});
            this.gfxMenuStrip.Name = "gfxMenuStrip";
            this.gfxMenuStrip.Size = new System.Drawing.Size(143, 76);
            // 
            // exportToolStripMenuItem1
            // 
            this.exportToolStripMenuItem1.Name = "exportToolStripMenuItem1";
            this.exportToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.exportToolStripMenuItem1.Text = "Export...";
            this.exportToolStripMenuItem1.Click += new System.EventHandler(this.exportToolStripMenuItem1_Click);
            // 
            // importToolStripMenuItem1
            // 
            this.importToolStripMenuItem1.Name = "importToolStripMenuItem1";
            this.importToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.importToolStripMenuItem1.Text = "Import...";
            this.importToolStripMenuItem1.Click += new System.EventHandler(this.importToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // exportPNGItem
            // 
            this.exportPNGItem.Enabled = false;
            this.exportPNGItem.Name = "exportPNGItem";
            this.exportPNGItem.Size = new System.Drawing.Size(142, 22);
            this.exportPNGItem.Text = "Export (PNG)";
            this.exportPNGItem.Click += new System.EventHandler(this.exportPNGToolStripMenuItem_Click);
            // 
            // xBox
            // 
            this.xBox.Location = new System.Drawing.Point(110, 37);
            this.xBox.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.xBox.Name = "xBox";
            this.xBox.Size = new System.Drawing.Size(56, 20);
            this.xBox.TabIndex = 10;
            // 
            // yBox
            // 
            this.yBox.Location = new System.Drawing.Point(195, 38);
            this.yBox.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.yBox.Name = "yBox";
            this.yBox.Size = new System.Drawing.Size(61, 20);
            this.yBox.TabIndex = 11;
            // 
            // mipmapCheckBox
            // 
            this.mipmapCheckBox.AutoSize = true;
            this.mipmapCheckBox.Location = new System.Drawing.Point(110, 14);
            this.mipmapCheckBox.Name = "mipmapCheckBox";
            this.mipmapCheckBox.Size = new System.Drawing.Size(63, 17);
            this.mipmapCheckBox.TabIndex = 12;
            this.mipmapCheckBox.Text = "Mipmap";
            this.mipmapCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y";
            // 
            // vgtExportDialog
            // 
            this.vgtExportDialog.Filter = "DDS files|*.dds|All files|*.*";
            // 
            // alphaCheckBox
            // 
            this.alphaCheckBox.AutoSize = true;
            this.alphaCheckBox.Location = new System.Drawing.Point(179, 15);
            this.alphaCheckBox.Name = "alphaCheckBox";
            this.alphaCheckBox.Size = new System.Drawing.Size(77, 17);
            this.alphaCheckBox.TabIndex = 15;
            this.alphaCheckBox.Text = "Alpha Map";
            this.alphaCheckBox.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(21, 15);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(32, 17);
            this.checkBox3.TabIndex = 16;
            this.checkBox3.Text = "?";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Visible = false;
            // 
            // vgtImportDialog
            // 
            this.vgtImportDialog.FileName = "openFileDialog2";
            this.vgtImportDialog.Filter = "DDS files|*.dds|All files|*.*";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.vgtPage);
            this.tabControl1.Controls.Add(this.vmgPage);
            this.tabControl1.Controls.Add(this.motPage);
            this.tabControl1.Controls.Add(this.mmgPage);
            this.tabControl1.Location = new System.Drawing.Point(248, 174);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(281, 350);
            this.tabControl1.TabIndex = 17;
            // 
            // vgtPage
            // 
            this.vgtPage.Controls.Add(this.label9);
            this.vgtPage.Controls.Add(this.label8);
            this.vgtPage.Controls.Add(this.textureOffTextBox);
            this.vgtPage.Controls.Add(this.textureHeaderOffTextBox);
            this.vgtPage.Controls.Add(this.mipmapNumBox);
            this.vgtPage.Controls.Add(this.alphaCheckBox);
            this.vgtPage.Controls.Add(this.mipmapCheckBox);
            this.vgtPage.Controls.Add(this.checkBox3);
            this.vgtPage.Controls.Add(this.treeView2);
            this.vgtPage.Controls.Add(this.label5);
            this.vgtPage.Controls.Add(this.xBox);
            this.vgtPage.Controls.Add(this.label4);
            this.vgtPage.Controls.Add(this.yBox);
            this.vgtPage.Location = new System.Drawing.Point(4, 22);
            this.vgtPage.Name = "vgtPage";
            this.vgtPage.Padding = new System.Windows.Forms.Padding(3);
            this.vgtPage.Size = new System.Drawing.Size(273, 324);
            this.vgtPage.TabIndex = 0;
            this.vgtPage.Text = "VGT";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Texture Offset";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Header Offset";
            // 
            // textureOffTextBox
            // 
            this.textureOffTextBox.Location = new System.Drawing.Point(110, 91);
            this.textureOffTextBox.Name = "textureOffTextBox";
            this.textureOffTextBox.Size = new System.Drawing.Size(100, 20);
            this.textureOffTextBox.TabIndex = 19;
            // 
            // textureHeaderOffTextBox
            // 
            this.textureHeaderOffTextBox.Location = new System.Drawing.Point(110, 65);
            this.textureHeaderOffTextBox.Name = "textureHeaderOffTextBox";
            this.textureHeaderOffTextBox.Size = new System.Drawing.Size(100, 20);
            this.textureHeaderOffTextBox.TabIndex = 18;
            // 
            // mipmapNumBox
            // 
            this.mipmapNumBox.Location = new System.Drawing.Point(59, 11);
            this.mipmapNumBox.Name = "mipmapNumBox";
            this.mipmapNumBox.Size = new System.Drawing.Size(45, 20);
            this.mipmapNumBox.TabIndex = 17;
            // 
            // vmgPage
            // 
            this.vmgPage.Location = new System.Drawing.Point(4, 22);
            this.vmgPage.Name = "vmgPage";
            this.vmgPage.Padding = new System.Windows.Forms.Padding(3);
            this.vmgPage.Size = new System.Drawing.Size(273, 293);
            this.vmgPage.TabIndex = 1;
            this.vmgPage.Text = "VMG";
            // 
            // motPage
            // 
            this.motPage.Controls.Add(this.listView1);
            this.motPage.Location = new System.Drawing.Point(4, 22);
            this.motPage.Name = "motPage";
            this.motPage.Size = new System.Drawing.Size(273, 293);
            this.motPage.TabIndex = 2;
            this.motPage.Text = "MOT";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.motIndexHeader,
            this.motAnimHeader,
            this.motOffsetHeader});
            this.listView1.ContextMenuStrip = this.motMenuStrip;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 13);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(249, 222);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // motIndexHeader
            // 
            this.motIndexHeader.Text = "Index";
            this.motIndexHeader.Width = 49;
            // 
            // motAnimHeader
            // 
            this.motAnimHeader.Text = "Anim. ID";
            // 
            // motOffsetHeader
            // 
            this.motOffsetHeader.Text = "Offset";
            this.motOffsetHeader.Width = 116;
            // 
            // motMenuStrip
            // 
            this.motMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.motMenuStrip.Name = "motMenuStrip";
            this.motMenuStrip.Size = new System.Drawing.Size(117, 26);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // mmgPage
            // 
            this.mmgPage.BackColor = System.Drawing.SystemColors.Control;
            this.mmgPage.Controls.Add(this.textBox4);
            this.mmgPage.Controls.Add(this.label7);
            this.mmgPage.Controls.Add(this.label6);
            this.mmgPage.Controls.Add(this.textBox3);
            this.mmgPage.Controls.Add(this.listBox1);
            this.mmgPage.Location = new System.Drawing.Point(4, 22);
            this.mmgPage.Name = "mmgPage";
            this.mmgPage.Padding = new System.Windows.Forms.Padding(3);
            this.mmgPage.Size = new System.Drawing.Size(273, 293);
            this.mmgPage.TabIndex = 3;
            this.mmgPage.Text = "MMG";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(54, 32);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(61, 20);
            this.textBox4.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Offset";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Size";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(172, 32);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(70, 20);
            this.textBox3.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.mmgMenuStrip;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "test"});
            this.listBox1.Location = new System.Drawing.Point(12, 85);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(255, 147);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // mmgMenuStrip
            // 
            this.mmgMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.extractAllToolStripMenuItem});
            this.mmgMenuStrip.Name = "mmgMenuStrip";
            this.mmgMenuStrip.Size = new System.Drawing.Size(127, 48);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.extractAllToolStripMenuItem.Text = "Extract All";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // motExportDialog
            // 
            this.motExportDialog.DefaultExt = "txt";
            this.motExportDialog.Filter = "Txt files|*.txt|All files|*.*";
            // 
            // index2Box
            // 
            this.index2Box.Location = new System.Drawing.Point(385, 73);
            this.index2Box.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.index2Box.Name = "index2Box";
            this.index2Box.Size = new System.Drawing.Size(56, 20);
            this.index2Box.TabIndex = 18;
            this.index2Box.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(461, 114);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(68, 20);
            this.textBox2.TabIndex = 19;
            this.textBox2.Visible = false;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // OLKImagePreview
            // 
            this.OLKImagePreview.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.OLKImagePreview.Location = new System.Drawing.Point(544, 12);
            this.OLKImagePreview.Name = "OLKImagePreview";
            this.OLKImagePreview.Size = new System.Drawing.Size(512, 512);
            this.OLKImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OLKImagePreview.TabIndex = 20;
            this.OLKImagePreview.TabStop = false;
            // 
            // OlkViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 535);
            this.Controls.Add(this.OLKImagePreview);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.index2Box);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.indexBox);
            this.Controls.Add(this.sizeTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.offsetTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "OlkViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OLK Viewer";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.fileSelectMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.indexBox)).EndInit();
            this.gfxMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.vgtPage.ResumeLayout(false);
            this.vgtPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mipmapNumBox)).EndInit();
            this.motPage.ResumeLayout(false);
            this.motMenuStrip.ResumeLayout(false);
            this.mmgPage.ResumeLayout(false);
            this.mmgPage.PerformLayout();
            this.mmgMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.index2Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OLKImagePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox offsetTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sizeTextBox;
        private System.Windows.Forms.NumericUpDown indexBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip fileSelectMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem extractMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ContextMenuStrip gfxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem1;
        private System.Windows.Forms.NumericUpDown xBox;
        private System.Windows.Forms.NumericUpDown yBox;
        private System.Windows.Forms.CheckBox mipmapCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SaveFileDialog vgtExportDialog;
        private System.Windows.Forms.CheckBox alphaCheckBox;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.OpenFileDialog vgtImportDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage vgtPage;
        private System.Windows.Forms.TabPage vmgPage;
        private System.Windows.Forms.TabPage motPage;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader motIndexHeader;
        private System.Windows.Forms.ColumnHeader motAnimHeader;
        private System.Windows.Forms.ColumnHeader motOffsetHeader;
        private System.Windows.Forms.ContextMenuStrip motMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog motExportDialog;
        private System.Windows.Forms.NumericUpDown index2Box;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.OpenFileDialog replaceFileDialog;
        private System.Windows.Forms.ToolStripDropDownButton helpButton;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabPage mmgPage;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ContextMenuStrip mmgMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textureOffTextBox;
        private System.Windows.Forms.TextBox textureHeaderOffTextBox;
        private System.Windows.Forms.NumericUpDown mipmapNumBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportPNGItem;
        private System.Windows.Forms.PictureBox OLKImagePreview;
    }
}

