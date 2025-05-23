﻿using ItchGamingWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItchGamingWiki.ItchCommon
{
    // Lay va hien thi cac thong tin co ban hien thi tren website
    public class ApplicationInfo
    {
        //ten du an 
        public static string ApplicationNameDefault = "";

        //Lay ten application luc dang nhap
        public static string ApplicationName = "";

        //Version du an
        public static string ApplicationVersion = "1.0";

        /*
         public static SYSCustomer
         public static SYSSoftware
         */
        public static SYSUser SYSUser {  get; set; }

        static ApplicationInfo()
        {
            using (var db = new ItchGamingWikiEntities())
            {
                SYSUser = db.SYSUsers.FirstOrDefault();

            }
        }
    }
}