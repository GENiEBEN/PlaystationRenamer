using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using GENiEBEN.Renamer.PlayStation;
using GENiEBEN.FileHandlers;


//FLOW (wip)
//[todo] Get recursive file list from root directory
//[todo] If file decide if archived or disc image file
//  [done] If archived, open and get name of CUE file (first found!)
//  [todo] Extract CUE and read track names
//[done] read CUE and get track names, including first in series
//[done] Detect primary disc image file (i.e first .bin file if multiple)
//[done] Seek for autorun executable filename, use that as disc id
//

namespace GENiEBEN.Renamer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PlayStation1 playStation1 = new PlayStation1();
            HandlerZIP handlerZIP = new HandlerZIP();

            //Console.WriteLine(playStation1.GetFirstChild(@"F:\zips\[SLPM-86700] Simple 1500 Series Vol.051 - The Jigsaw Puzzle NTSC (J)\[SLPM-86700] Simple 1500 Series Vol.051 - The Jigsaw Puzzle NTSC (J).cue"));

            //Console.WriteLine(playStation1.GetAutorunTitle(@"F:\zips\[SLPM-86700] Simple 1500 Series Vol.051 - The Jigsaw Puzzle NTSC (J)\[SLPM-86700] Simple 1500 Series Vol.051 - The Jigsaw Puzzle NTSC (J).bin"));

            //List<string> szaBuffer = handlerZIP.ListZipContents(@"F:\zips\[SCES-00007] Air Combat  PAL (E).zip");

            Console.WriteLine(playStation1.GetCueFromArchive(@"F:\zips\[SCES-00007] Air Combat  PAL (E).zip"));

        }
    }


}
