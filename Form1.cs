using System;
using System.Windows.Forms;
using System.IO;

namespace PlaystationRenamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine(GetPS1ExeTitleFromBINCUE(@"F:\zips\[SCES-00002] Battle Arena Toshinden PAL (E)\Battle Arena Toshinden (Europe) (Track 01).bin"));
        }


        public string GetPS1ExeTitleFromBINCUE(string FilePath)
        {
            using (BinaryReader b = new BinaryReader(File.Open(FilePath, FileMode.Open)))
            {
                int length = (int)b.BaseStream.Length;
                int pos = 0;
                string szGameTitle = "Unknown Disc Title";

                b.BaseStream.Seek(pos, SeekOrigin.Begin);
                //TODO: This will fail if it's not a valid .bin file, add some error catching.
                while (pos < length - 10) //parse the file byte by byte
                {
                    byte x = b.ReadByte();
                    if (x == 66) //look for 'B' and ':\' as indicator for string 'BOOT = cdrom:\'
                    {
                        b.BaseStream.Seek(11, SeekOrigin.Current); byte y = b.ReadByte(); //Try to see if 11th character is ':'
                        b.BaseStream.Seek(0, SeekOrigin.Current); byte z = b.ReadByte(); //Try to see if 12th character is '\'
                        if (y == 58 && z == 92) //Found ':\'. TODO: Chances of false positives are slim, but it's something to consider.
                        {
                            byte[] buff = b.ReadBytes(12); //Read the next 12bytes which should be the name of the autorun executable on the disc.
                            szGameTitle = System.Text.Encoding.Default.GetString(buff); ;
                            break;
                        }
                    }
                    pos++; //keep searching
                }
                b.BaseStream.Position = 0;
                if (szGameTitle.EndsWith(";")) //We have a standard name (SSSS_NNN.NN;) so prettify it
                {
                    szGameTitle=szGameTitle.Replace("_", "-").Replace(".", "").Replace(";", "");
                }
                else
                {
                    szGameTitle = "Longer than expected title";
                }
                return szGameTitle;
            }
        }
    }
}