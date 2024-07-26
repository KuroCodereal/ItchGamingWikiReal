using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItchGamingLibrary;
using System.IO;
using System.Web;

namespace IGW.Files
{
    public class IGWFile
    {
        // Do dai file toi da cho phep
        public int LimitSizeFile = 1; //MB
        public long MaxLengthFileAccept = 1 * 1024 * 1024; //1MB

        //Thong tin tai khoan dang nhap
        public IGWFile()
        {
            LimitSizeFile = 4;
            MaxLengthFileAccept = LimitSizeFile * 1024 * 1024;
        }

        //Thong tin tai khoan dang nhap
        //Dung luong file toi da cho phep (MB)

        public IGWFile(int _LimitSizeFile)
        {
            LimitSizeFile = _LimitSizeFile;
            this.MaxLengthFileAccept = LimitSizeFile * 1024 * 1024;
        }

        //Kiem tra dung luong file cho phep
        ///<param name="FileLength">
        /// ContentLength cua file can kiem tra
        /// </param>
        ///<return>
        ///True/False
        /// </return>
        /// 
        public bool IsValidFileLength(long FileLength = 0)
        {
            FileLength = (FileLength < 0 ? 0 : FileLength);
            if(FileLength > 0 && FileLength <= MaxLengthFileAccept)
            {
                return true;
            }

            return false;
        }

        //Kiem tra dinh dang file co hop le ko ("pdf","docx","doc","xls","xlsx","ppt","pptx","txt","png","jpg","jpeg","icon")
        ///<param name="FileName">
        /// Ten file can kiem tra
        /// </param>
        /// <return>
        /// True/False
        /// </return>
        /// 
        public bool IsValidFileAccept(string FileName)
        {
            try
            {
                string extFile = System.IO.Path.GetExtension(FileName).Substring(1).ToLower();
                List<string> LstExtensionFileAccept = new List<string>()
                {
                    "pdf","docx","doc","xls","xlsx","ppt","pptx","txt","png","jpg","jpeg","icon" //,"zip","rar"
                };
                return LstExtensionFileAccept.Contains(extFile);
            }
            catch
            {
                return false;
            }
        }

        //Kiem tra co phai la file "zip","rar" khong
        ///<param name="FileName">
        /// Ten file can kiem tra
        /// </param>
        ///<return>
        ///True/False
        /// </return>
        /// 
        public bool IsValidFilesAcceptZipRar(string FileName)
        {
            try
            {
                string extFile = System.IO.Path.GetExtension(FileName).Substring(1).ToLower();
                if(extFile == "zip" || extFile == "rar")
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //Kiem tra file co phai file excel khong
        public bool IsValidationExcelFile(string FileName, string FileContentType)
        {
            string extFile = System.IO.Path.GetExtension(FileName).Substring(1).ToLower();
            List<string> lstCorrectExcelContentType = new List<string>() { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            if ((extFile == "xls" || extFile == "xlsx" || extFile == "csv") && lstCorrectExcelContentType.Contains(FileContentType))
            {
                return true;
            }
            return false;
        }

        //Kiem tra xem file co ton tai khong
        public bool IsExistsFile(string UrlFile)
        {
            try
            {
                if (string.IsNullOrEmpty(UrlFile)) return false;
                return File.Exists(UrlFile);
            }
            catch
            {
                return false;
            }
        }

        //Kiem tra xem duong dan(url) folder co ton tai khong?
        public bool IsExistsFolder(string UrlFolder)
        {
            if (string.IsNullOrEmpty(UrlFolder)) return false;
            return File.Exists(UrlFolder);
        }

        //Tao mot folder moi neu chua ton tai
        public bool CreateFolder(string UrlFolder)
        {
            try
            {
                if(string.IsNullOrEmpty(UrlFolder)) return false;
                if (!Directory.Exists(UrlFolder))
                {
                    Directory.CreateDirectory(UrlFolder);
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Xoa file
        public bool DeleteFile(string urlFile)
        {
            try
            {
                if (string.IsNullOrEmpty(urlFile))
                {
                    throw new Exception("Vui long nhap UrlFile.");
                }
                if (File.Exists(urlFile))
                {
                    File.Delete(urlFile);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // Xoa file ben trong thu muc
        public bool DeleteAllFilesinFolder(string UrlFolder)
        {
            try
            {
                if (!Directory.Exists(UrlFolder))
                {
                    throw new Exception("UrlFolder khong hop le");
                }
                string[] lstFileName = Directory.GetFiles(UrlFolder);
                foreach(string item in lstFileName)
                {
                    DeleteFile(item);
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Chuyen file tu thu muc FromFolder den ToFolder
        public bool MoveFile(string FileName, string FromFolder, string ToFolder)
        {
            try
            {
                if(string.IsNullOrEmpty(FileName))
                {
                    throw new Exception("Vui long nhap FileName");
                }
                if (string.IsNullOrEmpty(FromFolder))
                {
                    throw new Exception("Vui long nhap FromFolder");
                }
                if (string.IsNullOrEmpty(ToFolder))
                {
                    throw new Exception("Vui long nhap ToFolder");
                }
                string from = FromFolder + FileName;
                string to = ToFolder + FileName;
                if(File.Exists(from))
                {
                    File.Move(from,to);
                }
                return true;
            }
            catch(Exception ex )
            {
                throw ex;
            }
        }

        #region SAVE FILE TO SERVER
        private string DateToStringYyMmDdHhMmSs(DateTime d)
        {
            if (d == null) return string.Empty;
            return d.Year.ToString().Substring(2, 2) + d.Month.ToString().PadLeft(2, '0') + d.Day.ToString().PadLeft(2, '0') + d.Hour.ToString().PadLeft(2, '0') + d.Minute.ToString().PadLeft(2, '0') + d.Second.ToString().PadLeft(2, '0');
        }

        ///<summary>
        ///Luu file
        /// </summary>
        /// <param name="FileBase">File dau vao - HttpPostedFileBase</param>
        /// <param name="ControllerServer">Bien Server o controller</param>
        /// <param name="Folder">Thu muc chua file can luu</param>
        /// <param name="FileName">Ten file tra ve sau khi da doi</param>
        /// <return>IGWMessage</return>
        /// 

        public IGWMessage Save(HttpPostedFileBase FileBase, HttpServerUtilityBase ControllerServer, string Folder, ref string FileName)
        {
            string FolderFileUpdate = (string.IsNullOrEmpty(Folder) ? "/FileUpload/" : Folder);
            if (!Directory.Exists(ControllerServer.MapPath(FolderFileUpdate)))
            {
                Directory.CreateDirectory(ControllerServer.MapPath(FolderFileUpdate));
            }
            if (FileBase == null)
            {
                return new IGWMessage(false, null, "FileBase is null.");
            }
            if (FileBase.ContentLength <= 0)
            {
                return new IGWMessage(false, null, "FileBase.ContentLength is null.");
            }
            //if(this.IsExistsFile(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName)))
            //{
            //    return "File ban tai len da co. Vui long kiem tra lai.";
            //}
            FileName = FileBase.FileName;
            string pathLocal = Path.Combine(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName));
            FileBase.SaveAs(pathLocal);
            return new IGWMessage(true,FolderFileUpdate + FileBase.FileName, "Save file success!");
        }

        ///<summary>
        ///luu file va kiem tra file da co hay chua
        /// </summary>
        /// <param name="FileBase">File dau vao - HttpPostedFileBase</param>
        /// <param name="ControllerServer">Bien Server o controller</param>
        /// <param name="Folder">Thu muc chua file can luu</param>
        /// <param name="FileName">Ten file tra ve sau khi da doi</param>
        /// <return>IGWMessage</return>
        /// 
        public IGWMessage SaveAndCheckExists(HttpPostedFileBase FileBase, HttpServerUtilityBase ControllerServer, string Folder, ref string FileName)
        {
            string FolderFileUpdate = (string.IsNullOrEmpty(Folder) ? "/FileUpload/" : Folder);
            if (!Directory.Exists(ControllerServer.MapPath(FolderFileUpdate)))
            {
                Directory.CreateDirectory(ControllerServer.MapPath(FolderFileUpdate));
            }
            if (FileBase == null)
            {
                return new IGWMessage(false, null, "FileBase is null.");
            }
            if (FileBase.ContentLength <= 0)
            {
                return new IGWMessage(false, null, "FileBase.ContentLength is null.");
            }
            if (this.IsExistsFile(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName)))
            {
                return new IGWMessage(false, null, "File ban tai len da co. Vui long kiem tra lai.");
            }
            FileName = FileBase.FileName;
            string pathLocal = Path.Combine(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName));
            FileBase.SaveAs(pathLocal);
            return new IGWMessage(true, FolderFileUpdate + FileBase.FileName, "Save file success!");
        }

        ///<summary>
        ///Luu file va doi ten file va kiem tra file da co hay chua
        /// </summary>
        /// <param name="FileBase">File dau vao - HttpPostedFileBase</param>
        /// <param name="ControllerServer">Bien Server o controller</param>
        /// <param name="Folder">Thu muc chua file can luu</param>
        /// <param name="FileName">Ten file tra ve sau khi da doi</param>
        /// <return>IGWMessage</return>
        /// 
        public IGWMessage SaveAndChangeFile(HttpPostedFileBase FileBase, HttpServerUtilityBase ControllerServer, string Folder, ref string FileName)
        {
            string FolderFileUpdate = (string.IsNullOrEmpty(Folder) ? "/FileUpload/" : Folder);
            if (!Directory.Exists(ControllerServer.MapPath(FolderFileUpdate)))
            {
                Directory.CreateDirectory(ControllerServer.MapPath(FolderFileUpdate));
            }
            if (FileBase == null)
            {
                return new IGWMessage(false, null, "FileBase is null.");
            }
            if (FileBase.ContentLength <= 0)
            {
                return new IGWMessage(false, null, "FileBase.ContentLength is null.");
            }
            if (this.IsExistsFile(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName)))
            {
                return new IGWMessage(false, null, "File ban tai len da co. Vui long kiem tra lai.");
            }
            //string pathFile = Path.GetFileName(File.FileName);
            FileName = FileBase.FileName;
            string pathLocal = Path.Combine(ControllerServer.MapPath("~" + FolderFileUpdate + FileBase.FileName));
            //urlSaveToDatabase = "/FileUpload/" + file.FileName;
            FileBase.SaveAs(pathLocal);
            //return FolderFileUpdate + file.FileName;
            // Neu muon doi ten file upload de tranh bi trung lap
            string extensionFile = this.DateToStringYyMmDdHhMmSs(DateTime.Now);
            string newFileName = Path.GetFileNameWithoutExtension(FileBase.FileName) + extensionFile + Path.GetExtension(FileBase.FileName);
            string pathLocalNew = Path.Combine(ControllerServer.MapPath("~" + FolderFileUpdate), newFileName);
            File.Move(pathLocal, pathLocalNew);
            FileName = newFileName;
            string urlSaveToDatabase = FolderFileUpdate + newFileName;
            return new IGWMessage(true,urlSaveToDatabase,"Save and change file success!");
        }
        #endregion

        // Ma hoa FireBase sang Base64
        ///<param name="FileBase">HttpPostedFileBase</param>
        ///<param name="fileType">FileType tra ve</param>
        ///
        public string EncodeFileToBase64Binary(HttpPostedFileBase FileBase, ref string FileType)
        {
            string encodeFile = null;
            try
            {
                if(FileBase == null)
                {
                    return encodeFile;
                }
                if (!this.IsValidFileLength(FileBase.ContentLength))
                {
                    return encodeFile;
                }
                string ExtFile = System.IO.Path.GetExtension(FileBase.FileName).Substring(1).ToUpper();
                switch (ExtFile)
                {
                    case "PDF":
                        FileType = "application/pdf";
                        break;
                    case "DOC":
                        FileType = "application/msword";
                        break;
                    case "DOCX":
                        FileType = "application/msword";
                        break;
                    case "XLS":
                        FileType = "application/vnd.ms-excel";
                        break;
                    case "XLSX":
                        FileType = "application/vnd.ms-excel";
                        break;
                    case "TXT":
                        FileType = "text/plain";
                        break;
                    case "TIF":
                        FileType = "image/tiff";
                        break;
                    case "GIF":
                        FileType = "image/gif";
                        break;
                    case "PNG":
                        FileType = "image/png";
                        break;
                    case "JPG":
                        FileType = "image/jpg";
                        break;
                    case "JPEG":
                        FileType = "image/jpeg";
                        break;
                    default:
                        FileType = null;
                        //TYPE_FILE = image/svg+xml; //Cac type anh khac
                        break;
                }
                if (string.IsNullOrEmpty(FileType))
                {
                    return null;
                }
                //byte[] fileType = null;
                //BinaryReader rdr = new BinaryReader(file.InputStream);
                //fileType = rdr.ReadBytes((int)file.ContentLength);
                //encodeFile = Convert.ToBase64String(fileType);
                //string theFileName = Path.GetFileName(file.FileName);
                //byte[] thepictureAsBytes = new byte[file.ContentLength];
                //using(BinaryReader theReader = new BinaryReader(file.InputStream))
                //{
                //    thepictureAsBytes = theReader.ReadBytes(file.ContentLength);
                //}
                //encodeFile = Convert.ToBase64String(thepictureAsBytes);

                byte[] binaryData;
                binaryData = new byte[FileBase.InputStream.Length];
                long bytesRead = FileBase.InputStream.Read(binaryData, 0, (int)FileBase.InputStream.Length);
                FileBase.InputStream.Close();
                encodeFile = System.Convert.ToBase64String(binaryData,0,binaryData.Length);
            }
            catch
            {
                encodeFile = null;
            }
            return encodeFile;
        }
    }
}
