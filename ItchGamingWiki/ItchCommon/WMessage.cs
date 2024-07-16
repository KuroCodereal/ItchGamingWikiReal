using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ItchGamingLibrary;

namespace ItchGamingWiki.ItchCommon
{
    public class BFEWMessage : BFMessage
    {
        public BFEWMessage() : base()
        {
            User us = SessionHelper.getSession();
            this.Header.AppName = ApplicationInfo.ApplicationName;
            this.Header.AppVersion = ApplicationInfo.ApplicationVersion;
            this.Header.SenderCode = us.UserName;
            this.Header.SenderName = us.FullName;
        }

        public BFEWMessage(decimal msgType) : base(msgType)
        {
            User us = SessionHelper.getSession();
            this.Header.AppName = ApplicationInfo.ApplicationName;
            this.Header.AppVersion = ApplicationInfo.ApplicationVersion;
            this.Header.SenderCode = us.UserName;
            this.Header.SenderName = us.FullName;
        }
    }

    // Cac loai thong diep request
    public class MessageType : BFMessageType { }

    //[Chi dung cho SYSTEMS] Message giao tiep giua server va client

    public class WMessage : BFMessage
    {
        public object Data { set; get; }
        public object SubData { set; get; }
        public object SubData2 { set; get; }
        public int PageIndex { set; get; }
        public int TotalRowData { set; get; }
        public string Description { set; get; }
        public string Description2 { set; get; }
        public WMessage() : base()
        {
            User us = SessionHelper.getSession();
            this.Header.AppName = ApplicationInfo.ApplicationName;
            this.Header.AppVersion = ApplicationInfo.ApplicationVersion;
            this.Header.SenderCode = us.UserName;
            this.Header.SenderName = us.FullName;
        }
        public WMessage(decimal msgType) : base(msgType)
        {
            User us = SessionHelper.getSession();
            this.Header.AppName = ApplicationInfo.ApplicationName;
            this.Header.AppVersion = ApplicationInfo.ApplicationVersion;
            this.Header.SenderCode = us.UserName;
            this.Header.SenderName = us.FullName;
        }
    }
}
