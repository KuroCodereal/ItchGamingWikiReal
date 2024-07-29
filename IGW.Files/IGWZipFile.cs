using ItchGamingLibrary;
using System;
using System.IO.Compression;
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
                ZipFilename = "IGWZipFile" + IGWCommon.DateToStringYyMmDdHhMmSs(DateTime.Now) + ".zip";
                string ZipPath = ToZipPath + ZipFilename;
                //System.IO.Compression.ZipFile.CreateFromDirectory(FromPath, ZipPath);
                return new IGWMessage(true, null, "Zip file success!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Giai nen file .zip
        //ZipPath: Duong dan thu muc file .zip can giai nen
        //ExtractPath: Thu muc chua file sau khi giai nen

        public IGWMessage ExtractZipFile(string ZipPath, string ExtractPath)
        {
            try
            {
                if(string.IsNullOrEmpty(ZipPath) || string.IsNullOrEmpty(ExtractPath))
                {
                    return new IGWMessage(false, null, "ZipPath and ExtractPath are invalid.");
                }
                //System.IO.Compression.ZipFile.ExtractToDirectory(ZipPath, ExtractPath);
                return new IGWMessage(true, null, "Extract file success!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
