using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItchGamingWiki.ItchCommon
{
    public class KEYWORD_PAGE
    {
        #region KEY GEMBOX
        public const string LicenseGemBox = "";
        public const string LicenseDocumentGemBox = "";
        #endregion KEY GEMBOX

        public const string ZEFExtensionSQLServerName = "";
        public const string ZEFExtensionSQLServerKey = "";

        public const string ZBulkOperationSQLServerName = "";
        public const string ZBulkOperationSQLServerKey = "";

        ///<summary>
        ///Number of row in page = 100;
        /// </summary>
        /// 
        public static int valuePage = 100;

        ///<summary>
        ///Có 2 ký tự IN HOA liền nhau, có kí tự đặc biệt [!@#$%^&*], có 2 chữ số liền nhau, có 3 chữ in thường liền nhau, phải có từ 8 đến 100 kí tự
        /// </summary>
        /// 
        public static string regexPasswordStrong = "^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,100}$";

        ///<summary>
        ///Có kí tự IN HOA, có kí tự đặc biệt [!@#$%^&*], có số, có chữ in thường, phải có từ 8 đến 100 kí tự.
        /// </summary>
        /// 
        public static string regexPasswordNormal = "^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,100}$";

        ///<summary>
        ///Thông báo khi mật khẩu k đủ mạnh
        /// </summary>
        /// 
        public static string regexPasswordError = "Mật khẩu phải có chữ cái in hoa, có chữ cái in thường và có chữ số, có tối thiểu 8 ký tự. Vui lòng kiểm tra lại!";

        //Thông báo khi mật khẩu đang để mặc định
        public static string defaultPassDescription = "Mật khẩu của bạn đang để mặc định. Để đảm bảo an toàn thông tin, vui lòng đổi mật khẩu để tiếp tục sử dụng chương trình. Xin cảm ơn";

        public static string regexFloatNumber = @"^[-+]?[0-9]*(?:\.[0-9]*)?$";
        //Lấy chuỗi ngẫu nhiên có độ dài = lenPassword ký tự

        public static string getRandomPassword(int lenPassword)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$&*";
            //string getChar = new string(Enumerable.Repeat(chars,lenPassword).Select(x => x[random.Next(x.Length)]).ToArray());
            var listChars = Enumerable.Repeat(0,lenPassword).Select(x => chars[random.Next(chars.Length)]);
            return string.Join("", listChars);
        }

        public const int Default = -1;
        public const int Yes = 1;
        public const int No = 0;
        public const int IsDefault = -1;
        public const int IsYes = 1;
        public const int IsNo = 2;
        //".ASPXAUTH"
        public static string TokenRememberName = System.Web.Security.FormsAuthentication.FormsCookieName;

    }
}