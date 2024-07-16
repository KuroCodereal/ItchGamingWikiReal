using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Web.Util;

namespace ItchGamingWiki.Authorization
{
    public class ValidateJsonXSS : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext?.Request;
            string requestContentType = request.ContentType.ToLower();
            //if (request != null && "application/json".Equals(request.ContentType, StringComparison.OrdinalIgnoreCase)) ;
            if(request != null && requestContentType.Contains("application/json"))
            {
                if(request.ContentLength > 0 && request.Form.Count == 0)
                {
                    if(request.InputStream.Position > 0)
                        request.InputStream.Position = 0; //InputStram has already been read once from "ProcessRequest"
                    using(var reader = new StreamReader(request.InputStream))
                    {
                        string postedContent = reader.ReadToEnd(); //Get posted JSON content
                        int failureIndex = 0;
                        bool isValid = RequestValidator.Current.InvokeIsValidRequestString(HttpContext.Current, postedContent,
                            RequestValidationSource.Form, "postedJson", out failureIndex); //Invoke XSS validation
                        //string jsEncode = AntiXss.JavascriptEncode(postedContent);
                        //string htmlEncode = AntiXss.JavascriptEncode(postedContent);
                        //string urlEncode = AntiXss.JavascriptEncode(postedContent);
                        if (!isValid) //Not valid, so throw request validation exception
                        {
                            throw new HttpRequestValidationException("Potentially unsafe input detected");
                        }
                    }
                }
            }
        }
    }
}