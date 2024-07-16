using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    public static class IGWException
    {
        public static string GetMessage(this Exception ex)
        {
            string result = string.Empty;
            if (ex == null) return result;
            result = ex.Message;
            if (ex.InnerException != null)
            {
                result += ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    result += ex.InnerException.InnerException.Message;
                }
            }
            return result;
        }

        public static string GetMessage(this DbEntityValidationException dbEx)
        {
            string msg = string.Empty;
            foreach(var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach(var validationError in validationErrors.ValidationErrors)
                {
                    string PropertyName = validationError.PropertyName;
                    string ErrorMessage = validationError.ErrorMessage;
                    msg = (msg == string.Empty ? $"[{PropertyName}-{ErrorMessage}]" : msg + "[{PropertyName}-{ErrorMessage}]");
                }
            }
            return dbEx.Message + ";" + msg;
        }
    }
}
