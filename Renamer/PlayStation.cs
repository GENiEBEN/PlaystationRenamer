using System.Collections.Generic;
using System.IO;
using GENiEBEN.FileHandlers;

namespace GENiEBEN.Renamer.PlayStation
{
    public class PlayStation1
    {
        public string GetCueFromArchive(string ArchiveFilePath)
        {
            string szResult = "";
            if (ArchiveFilePath.EndsWith(".zip"))
            {
                HandlerZIP handlerZIP = new HandlerZIP();
                List<string> szaBuffer = handlerZIP.ListZipContents(ArchiveFilePath);
                
                foreach (string entry in szaBuffer)
                {
                    if (entry.EndsWith(".cue"))
                    {
                        szResult = entry;
                        break;
                    }
                }    
            } else if(ArchiveFilePath.EndsWith(".7z")) {
                szResult = "7z archives are not supported yet.";
                //TODO: support for 7z archives
            }
            return szResult;
        }

        //Returns a list of all .BIN files attached to a .CUE file
        //This could be just one or multiple. TODO: There's no error handling for CUE files without FILE tags.
        public List<string> GetChildFiles(string CUEFilePath)
        {
            string[] szaBuffer = File.ReadAllLines(CUEFilePath);
            List<string> szaResult = new List<string>();
            foreach (string s in szaBuffer)
            {
                if (s.StartsWith("FILE"))
                {
                    szaResult.Add(s.Replace("FILE \"", "").Replace("\" BINARY", ""));
                }
            }
            return szaResult;
        }

        //Returns just the first .BIN file attached to a .CUE
        //This is the binary file where we are going to look for the disc ID
        public string GetFirstChild(string CUEFilePath)
        {
            List<string> szaBuffer = GetChildFiles(CUEFilePath);
            return szaBuffer[0];
        }

        //Parses the disc image file (.bin | .img) and retrieves the name of the executable that autoruns.
        //This executable is also the Disc ID, for example: SCES-00002
        public string GetAutorunTitle(string BINFilePath)
        {
            using (BinaryReader b = new BinaryReader(File.Open(BINFilePath, FileMode.Open)))
            {
                int intLength = (int)b.BaseStream.Length;
                int intPosition = 0;
                string szGameTitle = "Unknown Disc Title";
                string szMagic;

                b.BaseStream.Seek(intPosition, SeekOrigin.Begin);
                //TODO: This will fail if it's not a valid .bin file, add some error catching.
                while (intPosition < intLength) //parse the file byte by byte
                {
                    byte x = b.ReadByte();
                    if (x == 66) //look for 'B' as it might indicate the beggining of our Magic string
                    {
                        byte[] buff = b.ReadBytes(13); //read the next 13 bytes
                        szMagic = System.Text.Encoding.Default.GetString(buff); ;
                        if (szMagic == "OOT = cdrom:\\") //we found 'BOOT = cdrom:\'
                        {
                            byte[] buff2 = b.ReadBytes(12); //Read the next 12 bytes after the Magic
                            szGameTitle = System.Text.Encoding.Default.GetString(buff2); //We now have the name of the autorun executable on the disc.
                            break;
                        }
                    }
                    intPosition++; //keep searching
                }
                b.BaseStream.Position = 0;
                if (szGameTitle.EndsWith(";")) //We have a standard name (SSSS_NNN.NN;) so prettify it
                {
                    szGameTitle = szGameTitle.Replace("_", "-").Replace(".", "").Replace(";", "");
                }
                else
                {
                    szGameTitle = "Longer than expected title"; //Don't return a result at all if longer than expected, will have to see if such titles exist.
                }
                return szGameTitle;
            }
        }
    }
}
