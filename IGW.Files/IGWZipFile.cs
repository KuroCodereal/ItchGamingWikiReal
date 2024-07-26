using ItchGamingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGW.Files
{
    public class IGWZipFile
    {
        //Nen cac file thanh .zip
        //FormPath: Thu muc chua cac file can nen
        //ToZipPath: thu muc chua file sau khi nen
        //ZipFileName: ten file sau khi nen
        public IGWMessage AddToZipFile(string FromPath, string ToZipPath, string ZipFilename = "")
        {
            try
            {
                if (string.IsNullOrEmpty(FromPath) || string.IsNullOrEmpty(ToZipPath))
                {
                    return new IGWMessage(false, null, "FromPath and ToZipPath are invalid");
                }
                //ZipFilename = "IGWZipFile" + IGWCommom
            }
        }
    }
}
