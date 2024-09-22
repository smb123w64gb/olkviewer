using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace olkviewer
{
    class FileMOT
    {
        public static void GetEntries(FileStream fs, BinaryReader br, List<Mot.Entry> motEntries, ListView listView1, Mot.Header motHeader, long fOffset, int Count)
        {
            motEntries.Clear();
            listView1.Items.Clear();

            listView1.BeginUpdate();



            for (int i = 0; i < Count; i++)
            {
                Mot.Entry motEntry = new Mot.Entry(br);

                motEntry.dOffset = (uint)(motEntry.DataPtr + motHeader.DataOffset + fOffset);

                string motIdxString = String.Format("{0:X4}", i);
                string motIdString = String.Format("{0:X4}", motEntry.MotId);
                string motOffsetString = String.Format("{0:X8}", motEntry.dOffset);

                ListViewItem item = new ListViewItem();
                item.Text = motIdxString;
                item.SubItems.Add(motIdString);
                item.SubItems.Add(motOffsetString);

                motEntries.Add(motEntry);
                listView1.Items.Add(item);

            }

            listView1.EndUpdate();
        }

        public static void Export(string OlkFileName, string MotFileName, long Offset)
        {
            //byte[] motData;
            byte[] mmData;
            byte[] subData = new byte[3];

            List<string> bData = new List<string>();

            /* Read Mot data */
            using (FileStream fs = new FileStream(OlkFileName, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                string bString = "";
                //long fOffset = (long)motEntries[motIndex].dOffset;

                //int size = 1;

                //int eof = 0;

                fs.Seek(Offset, SeekOrigin.Begin);

                for (int i = 0; i < 0x400; i++)
                {
                    long pos = fs.Position;

                    byte bCode = br.ReadByte();
                    int b = (int)bCode & 0x3F;

                    /* make switch statement */

                    switch (b)
                    {
                        case 1:
                            subData = br.ReadBytes(2);
                            bString = String.Format("{0:X2} {1:X2}{2:X2}", bCode, subData[0], subData[1]);
                            break;

                        case 9:
                        case 0xA:
                        case 0xB:
                        case 0x19:
                        case 0x28:
                        case 0x29:
                        case 0x2A:
                            subData = br.ReadBytes(2);
                            bString = String.Format("{0:X2} {1:X2}{2:X2}", bCode, subData[0], subData[1]);
                            break;

                        case 0x25:
                            subData = br.ReadBytes(2);
                            bString = String.Format("{0:X2} {1:X2} {2:X2}", bCode, subData[0], subData[1]);
                            break;

                        default:
                            bString = String.Format("{0:X2}", bCode);
                            break;
                    }

                    string bidxString = String.Format("{0:X4}: ", pos - Offset);

                    if ((b == 0x28) || (b == 0x2A))
                    {
                        bString += "\t\t // jump\n";
                    }
                    else if (b == 8)
                    {
                        bString += "\t\t // sub end?\n";
                    }

                    bString = bidxString + bString;

                    if (b == 0x25)
                    {
                        /* messy */
                        /*
                        if (((subData[0] == 0xD) || (subData[0] == 0x7)) && 
                            (subData[1] == 0x1))
                        {
                            byte nb = br.ReadByte();

                            if (nb == 2)
                            {
                                //bData.Add("02\n");
                                bString += " 02";
                                bData.Add(bString);
                                MessageBox.Show("End reached");
                                break;
                            }
                            else
                            {
                                fs.Seek(fs.Position - 1, SeekOrigin.Begin);
                            }
                        }
                        */
                    }

                    if (b == 2)
                    {
                        bString += "\t\t // end\n";
                        bData.Add(bString);
                        MessageBox.Show("End reached");
                        break;
                    }
                    //else
                    //{
                    //    fs.Seek(fs.Position - 1, SeekOrigin.Begin);
                    //}

                    bData.Add(bString);

                }

                /* get vgt data */
                //motData = br.ReadBytes(size);

                //Vgt_Byte_Swap(vgtData, size, x, y);
                //Vgt_Data_Read(fs, br, fOffset);

                mmData = new byte[16];
            }

            /* write mot data */
            using (StreamWriter sw = new StreamWriter(MotFileName))
            {

                foreach (string s in bData)
                {
                    sw.WriteLine(s);
                }
            }
        }
    }
}
