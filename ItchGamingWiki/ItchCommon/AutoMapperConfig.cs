using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItchGamingWiki.ItchCommon
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<DBussiness, DBussinessDTO>();
                // map qua map lai giua cac model
            });
        }
    }
}