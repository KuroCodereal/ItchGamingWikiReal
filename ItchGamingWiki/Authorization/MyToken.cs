using ItchGamingWiki.ItchCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace ItchGamingWiki.Authorization
{
    public class MyToken
    {
        // thoi gian ton tai cua Token (don vi: hour)

        private int timeOfToken { set; get; }
        public MyToken()
        {
            // 24h
            this.timeOfToken = 24;
        }

        public MyToken(int numberOfDay)
        {
            // token ton tai trong bn ngay
            this.timeOfToken = numberOfDay;
        }

        public string GenerateToken(string reason, User user)
        {
            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _key = Guid.Parse(user.SecurityStamp).ToByteArray();
            byte[] _Id = Encoding.UTF8.GetBytes(user.UserId.ToString());
            byte[] _reason = Encoding.UTF8.GetBytes(reason);
            byte[] data = new byte[_time.Length + _key.Length + _reason.Length + _Id.Length];

            System.Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            System.Buffer.BlockCopy(_key, 0, data, _time.Length, _key.Length);
            System.Buffer.BlockCopy(_reason, 0, data, _time.Length + _key.Length, _reason.Length);
            System.Buffer.BlockCopy(_Id, 0, data, _time.Length + _key.Length + _reason.Length, _Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        public TokenValidation ValidationToken(string reason, User user, string token)
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] _key = data.Skip(8).Take(16).ToArray();
            byte[] _reason = data.Skip(24).Take(4).ToArray();
            byte[] _Id = data.Skip(28).ToArray();

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));
            if (when < DateTime.UtcNow.AddHours(this.timeOfToken * -1))
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }
            Guid gKey = new Guid(_key);
            if (gKey.ToString() != user.SecurityStamp)
            {
                result.Errors.Add(TokenValidationStatus.WrongGuid);
            }
            if (reason != Encoding.UTF8.GetString(_reason))
            {
                result.Errors.Add(TokenValidationStatus.WrongPurpose);
            }
            if (user.UserId.ToString() != Encoding.UTF8.GetString(_Id))
            {
                result.Errors.Add(TokenValidationStatus.WrongUser);
            }
            return result;
        }

    }

    public class TokenValidation
    {
        public bool Validated { get { return Errors.Count == 0; } }
        public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
    }

    public enum TokenValidationStatus
    {
        Expired,
        WrongUser,
        WrongPurpose,
        WrongGuid
    }

    public static class ReasonToken
    {
        public static string ResetPassword = "RES_PASSWORD";
        public static string RememberAccount = "REM_ACCOUNT";
        public static string othersReason = "XXX";
    }

    //=========================================================

    public class MyToken1
    {
        // Thoi gian live cua token (don vi: hour)
        public int timeOfToken { get; set; }
        public MyToken1()
        {
            timeOfToken = 24;
        }

        public MyToken1(int numberOfDay)
        {
            this.timeOfToken = numberOfDay;
        }

        public string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public bool ValidationToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(this.timeOfToken * -1))
            {
                return false;
            }
            return true;
        }
    }
}