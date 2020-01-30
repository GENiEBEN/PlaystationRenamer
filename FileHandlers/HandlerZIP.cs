using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace GENiEBEN.FileHandlers
{
    public class HandlerZIP
    {
        public List<string> ListZipContents(string ZIPFilePath)
        {
            List<string> szaResult = new List<string>();
            ZipArchive ZIPFile = ZipFile.OpenRead(ZIPFilePath);
            foreach (ZipArchiveEntry ZIPEntry in ZIPFile.Entries)
            {
                szaResult.Add(ZIPEntry.FullName);
            }
            return szaResult;
        }
    }
}
