using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DevOne.Security.Cryptography.BCrypt;
namespace ItchGamingLibrary
{
    public class IGWCrypt
    {
        ///<summary>
        ///Ma hoa chuoi theo BCrypt
        /// </summary>
        /// <param name="str">Gia tri dau vao</param>
        /// <returns>string</returns>
        /// 
        public static string GenBcrypt(string str)
        {
            return BCryptHelper.HashPassword(str, BCryptHelper.GenerateSalt(12));
        }

        ///<summary>
        /// Kiem tra gia tri dau vao va gia tri Ma Hoa xem co hop le hay ko BCrypt
        /// </summary>
        /// <param name="str">Gia tri kiem tra</param>
        /// <param name="hashed">Gia tri da ma hoa</param>
        /// <returns>bool</returns>
        /// 
        public static bool verifyBcrypt(string str,string hashed)
        {
            try
            {
                return BCryptHelper.CheckPassword(str, hashed);
            }
            catch
            {
                return false;
            }
            //return BCrypt.Net.BCrypt.Verify(str, hashed);
        }

        public static string GenMd5(string str)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash,str);
            }
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            //Convert the input string to a byte array and compute the hash
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            //Create a new StringBuilder to collect the bytes and create a string
            StringBuilder sBuilder = new StringBuilder();
            //Loop through each byte of the hashed data and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            //Return the hexadecimal string
            return sBuilder.ToString();
        }
    }
}
