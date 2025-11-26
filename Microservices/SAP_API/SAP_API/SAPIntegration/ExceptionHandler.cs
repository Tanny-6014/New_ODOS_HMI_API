using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SAP_API
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        ILog logger = log4net.LogManager.GetLogger("ApiLog");
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
                       
            var _exceptiontype = actionExecutedContext.Exception.GetType().Name;
            var _statuscode = GetResponseCode(_exceptiontype);

            if (_statuscode == HttpStatusCode.InternalServerError)
            {
                logger.Error("Exception : " + actionExecutedContext.Exception.Message + ", Inner Exception - " + actionExecutedContext.Exception.InnerException.Message + "Request Uri - "+actionExecutedContext.Request.RequestUri+ "Request Content - " + actionExecutedContext.Request.Content + ", Trace - " + actionExecutedContext.Exception.StackTrace);
                
                //Need To Add - log this exception message to database and Send mail alert to Support Team. 

                var response = new HttpResponseMessage(_statuscode)
                {
                    Content = new StringContent("An unhandled exception was thrown by service."),
                    ReasonPhrase = "Internal Server Error.Please Contact your Administrator.",
                    StatusCode=HttpStatusCode.InternalServerError
                };
                actionExecutedContext.Response = response;
            }
            else
            {
                logger.Info("Exception : " + actionExecutedContext.Exception.Message + ", occured in method : " + actionExecutedContext.ActionContext.ActionDescriptor.ActionName);

                var response = new HttpResponseMessage(_statuscode)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = actionExecutedContext.Exception.Message,
                    StatusCode=_statuscode
                };
                actionExecutedContext.Response = response;

            }
            

        }


        private HttpStatusCode GetResponseCode(string exceptiontype)
        {
            try
            {
                if (string.IsNullOrEmpty(exceptiontype))
                    return HttpStatusCode.InternalServerError;
                else
                {
                    switch(exceptiontype)
                    {
                        case "ApplicationException":
                            return HttpStatusCode.BadRequest;
                        case "ArgumentNullException":
                            return HttpStatusCode.BadRequest;
                        case "InvalidCastException":
                            return HttpStatusCode.BadRequest;
                        default:  return HttpStatusCode.InternalServerError;

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}