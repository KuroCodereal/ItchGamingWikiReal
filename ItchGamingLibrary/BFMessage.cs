using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    /// <summary>
    /// Message giao tiep giua BackEnd va FrontEnd
    /// </summary>
    public class BFMessage
    {
        public BFHeader Header = new BFHeader();
        public BFBody Body = new BFBody();
        public BFAuthorize Authorize = new BFAuthorize();
        public BFMessage()
        {
            Body.TotalRowData = 0;
        }

        public BFMessage(decimal msgType)
        {
            Header = new BFHeader(msgType);
            Body.TotalRowData = 0;
        }

        public string EncodeWMessage()
        {
            return JsonConvert.SerializeObject(this);
        }

        public BFMessage DecodeWMessage(string jsonRequest)
        {
            return JsonConvert.DeserializeObject<BFMessage>(jsonRequest);
        }
    }

    public class BFHeader
    {
        ///<summary>
        /// Ten ung dung gui thong tin
        /// </summary>
        /// 
        public string AppName { get; set; }
        /// <summary>
        /// Version cua ung dung gui thong tin
        /// </summary>
        public string AppVersion { get; set; }
        ///<summary>
        /// Ma cua nguoi gui thong tin: class messageCode
        /// </summary>
        /// 
        public string SenderCode { get; set; }
        ///<summary>
        ///Ten cua nguoi gui thong tin
        /// </summary>
        /// 
        public string SenderName { get; set; }
        ///<summary>
        ///Ngay tao giao dich
        /// </summary>
        /// 
        public DateTime SendDate { get; set; }
        ///<summary>
        ///Loai thong diep gui: Request[0 - Default, 1- Lay thong tin, 2-Them moi, 3-Edit, 4-Warning, 5- Delete]
        ///Loai thong diep tra ve: Response messageType[100-info,200-success,300-primary,400-warning,500-danger]
        /// </summary>
        /// 
        public decimal MsgType { get; set; }
        ///<summary>
        ///So tham chieu cua gia tri hoi - request
        /// </summary>
        /// 
        public string RequestID { get; set; }
        ///<summary>
        ///So tham chieu cua gia tri tra loi - response
        /// </summary>
        /// 
        public string ResponseID { get; set; }
        public BFHeader()
        {
            //this.AppName = ApplicationInfo.ApplicationName;
            //this.AppVersion = ApplicationInfo.ApplicationVersion;
            //this.SenderCode = us.UserName;
            //this.SenderName = us.FullName;
            this.MsgType = BFMessageType.UnknownError;
            this.ResponseID = Guid.NewGuid().ToString();
            this.SendDate = new DateTime();
        }

        public BFHeader(decimal Type)
        {
            this.MsgType = Type;
            this.ResponseID = Guid.NewGuid().ToString();
            this.SendDate = new DateTime();
        }
    }
        public class BFBody
        {
            public object Data {  get;set; }
            public object SubData {  get;set; }
            public object SubData2 {  get;set; }
            public int PageIndex {  get;set; }
            public int TotalRowData {  get;set; }
            public string Description {  get;set; }
            public string Description2 {  get;set; }
        }

        public class BFAuthorize
        {
            public string Token;
            public string Language = "VI";
            public string ClientIP = "LOCAL";
            public BFAuthorize()
            {
                Token = Guid.NewGuid().ToString();
            }
        }
        public class BFMessageType
        {
            ///<summary>
            ///Thong tin
            /// </summary>
            /// 
            public const decimal Informational = 100;

            ///<summary>
            ///Thanh cong
            /// </summary>
            /// 
            public const decimal Success = 200;

            ///<summary>
            ///Dieu huong
            /// </summary>
            /// 
            public const decimal Redirection = 300;

            ///<summary>
            ///Yeu cau phia client khong hop le, bi loi
            /// </summary>
            /// 
            public const decimal ClientError = 400;

            ///<summary>
            ///Xu ly server bi loi
            /// </summary>
            /// 
            public const decimal ServerError = 500;

            ///<summary>
            ///Loi ko xac dinh
            /// </summary>
            /// 
            public const decimal UnknownError = 0;
        }

    }
