using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    //Message giao tiếp giữa các function
    public class IGWMessage
    {
        public bool isSuccess = true;
        public Object Data;
        public Object SubData;
        public string Description;
        public IGWMessage()
        {
            isSuccess = true;
        }

        public IGWMessage(bool _IsSuccess)
        {
            isSuccess = _IsSuccess;
        }

        public IGWMessage(bool _IsSuccess, Object _Data, string _Msg = "")
        {
            isSuccess= _IsSuccess;
            Data = _Data;
            Description = _Msg;
        }
    }
}
