using System.Reflection;
using System;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using GmpsMvcController.Controller;
using GmpsMvcController.Utils;
using System.Collections.Specialized;
using System.Dynamic;
using System.Web.SessionState;

//RouteTable.Routes.Add(new Route("request/Ajax/{controller}/{action}/{*index}", new GmpsMvcRouteHanler()));

namespace GmpsMvcController.Handler
{
    internal class GmpsMvcHttpHandler : IHttpHandler, IDisposable, IRequiresSessionState
    {
        private delegate void AssyncCallbackMethod(HttpContext context);

        private readonly Assembly _callingAssembly;
        public GmpsMvcHttpHandler(Assembly callingAssembly)
        {
            this._callingAssembly = callingAssembly;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.Process(context);
        }

        private void Process(HttpContext context)
        {
            try
            {
                var controller = (context.Request.RequestContext.RouteData.Values["controller"] ?? "") as String;
                var action = (context.Request.RequestContext.RouteData.Values["action"] ?? "") as String;

                Type typeOfClass = null;
                foreach (var type in _callingAssembly.GetExportedTypes())
                    if (type.Name.ToLower() == "controller" + controller.ToLower())
                    {
                        typeOfClass = type;
                        break;
                    }

                if (typeOfClass != null && typeOfClass.IsSubclassOf(typeof(GmpsController)))
                {
                    var requestMethod = context.Request.HttpMethod;

                    var parametersArray = new List<object>();

                    MethodInfo methodInfo = null;
                    try
                    {
                        #region One occurrence of the method
                        methodInfo = typeOfClass.GetMethod(action);
                        var parameters = methodInfo.GetParameters();

                        if (CheckRequestType(methodInfo.GetCustomAttributesData(), requestMethod))
                        {

                            if (parameters.Length != 0) // if method have parameters
                            {
                                #region GET or DELETE | Don't have content in body of request
                                if (requestMethod == "GET" || requestMethod == "DELETE")
                                {
                                    var id = context.Request.RequestContext.RouteData.Values["index"] ?? "";
                                    if (!string.IsNullOrEmpty(id as String))
                                    {
                                        if (parameters.Length == 1)
                                            parametersArray.Add(ConvertToType(id, parameters[0].ParameterType));
                                        else
                                        {
                                            var query = new NameValueCollection(context.Request.QueryString);
                                            query.Add("index", id as String);
                                            parametersArray = TryGetParametersValueFromQueryString(parameters, query);
                                        }
                                    }
                                    else
                                        parametersArray = TryGetParametersValueFromQueryString(parameters, context.Request.QueryString);

                                }

                                #endregion

                                #region POST or PUT
                                else
                                {

                                    var id = context.Request.RequestContext.RouteData.Values["index"] ?? "";

                                    System.IO.Stream streamBody = context.Request.InputStream;
                                    System.Text.Encoding encoding = context.Request.ContentEncoding;
                                    System.IO.StreamReader reader = new System.IO.StreamReader(streamBody, encoding);
                                    string inputJson = reader.ReadToEnd();
                                    DynamicJsonObject dic = inputJson.FromJson();

                                    foreach (var para in parameters)
                                    {
                                        if ((!string.IsNullOrEmpty(id as String)) && (para.Name.ToLower() == "index"))
                                            parametersArray.Add(ConvertToType(id, para.ParameterType));
                                        else
                                        {
                                            object aux = null;
                                            dic.TryGetMember(para.Name.ToLower(), out aux);
                                            if (aux != null)
                                                parametersArray.Add(ConvertToType(aux, para.ParameterType));
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                            methodInfo = null;

                        #endregion
                    }
                    catch
                    {

                    }

                    if (methodInfo != null)
                    {
                        object result = null;

                        GmpsController classInstance = (GmpsController)Activator.CreateInstance(typeOfClass, null);
                        classInstance._clientIP = context.Request.UserHostAddress;
                        classInstance._clientHostName = context.Request.UserHostName;
                        if (context.Request.Url != null)
                        {
                            classInstance._requestUrl = context.Request.Url.PathAndQuery;
                            classInstance._requestRawUrl = context.Request.Url.AbsoluteUri;
                            classInstance._requestQuery = context.Request.Url.Query;
                        }
                        if (context.Request.Browser != null)
                            classInstance._requestBrowser = context.Request.Browser.Browser;

                        if (context.Request.ContentEncoding != null)
                            classInstance._requestEncoding = context.Request.ContentEncoding.EncodingName;

                        classInstance._requestLength = context.Request.ContentLength;
                        classInstance._requestCookies = context.Request.Cookies;
                        classInstance._requestHeaders = context.Request.Headers;
                        classInstance._requestHttpMethod = context.Request.HttpMethod;

                        if (context.Request.UrlReferrer != null)
                            classInstance._requestUrlReferrer = context.Request.UrlReferrer.AbsoluteUri;

                        classInstance._requestUserLanguages = context.Request.UserLanguages;
                        classInstance._requestIsAuthenticated = context.Request.IsAuthenticated;
                        classInstance._requestIsSecureConnection = context.Request.IsSecureConnection;
                        classInstance._requestLogonUserIdentity = context.Request.LogonUserIdentity;
                        classInstance._requestClientCertificate = context.Request.ClientCertificate;
                        classInstance._requestUserAgent = context.Request.UserAgent;
                        classInstance._requestInputStream = context.Request.InputStream;
                        classInstance._requestForm = context.Request.Form;
                        classInstance._response = context.Response;

                        try
                        {
                            result = methodInfo.Invoke(classInstance, parametersArray.ToArray());
                            if (methodInfo.ReturnType != typeof(void))
                            {
                                if (methodInfo.ReturnType.IsPrimitive || methodInfo.ReturnType.Name.ToLower() == "string")
                                    context.Response.Write(result);
                                else
                                    context.Response.Write(result.ToJson());
                            }
                        }
                        catch //  Error to call controller
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound; // not found
                        }
                    }
                    else // Method not found
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound; // not found
                    }
                }
                else // Controler Not found
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound; // not found
                }
            }
            catch // erro não especificado
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // internal server error
            }

            context.Response.Flush();
            context.Response.End();
        }

        private bool CheckRequestType(IList<CustomAttributeData> atributes, string requestMethod)
        {
            bool ok = false;
            foreach (CustomAttributeData item in atributes)
            {
                if (item.ToString() != "[GmpsMvcController.Controller.HttpNotAccessibleMethod()]")
                    if (item.ToString() == "[GmpsMvcController.Controller.HttpGetPostMethod()]"
                        || (item.ToString() == "[GmpsMvcController.Controller.HttpGetMethod()]" && requestMethod == "GET")
                        || item.ToString() == "[GmpsMvcController.Controller.HttpPostMethod()]" && (requestMethod == "POST" || requestMethod == "PUT"))
                        ok = true;
            }

            return ok;
        }

        private MethodInfo GetBestMethodMatch(MethodInfo[] methods)
        {
            return null;
        }

        private MethodInfo GetMethodWithMoreParameters(MethodInfo[] methods)
        {
            return null;
        }

        private object ConvertToType(object target, Type targetType)
        {
            try
            {
                return Convert.ChangeType(target, targetType);
            }
            catch
            {
                try
                {
                    return target.FromJson(targetType);
                }
                catch
                {
                    return null;
                }
            }
        }

        private List<object> TryGetParametersValueFromQueryString(ParameterInfo[] parameters, NameValueCollection query)
        {
            var list = new List<object>();

            if (parameters.Length == 1 && query.Count == 1)
                list.Add(this.ConvertToType(query[0], parameters[0].ParameterType));
            else
                foreach (var para in parameters)
                    if (!string.IsNullOrEmpty(query[para.Name.ToLower()]))
                        list.Add(this.ConvertToType(query[para.Name.ToLower()], para.ParameterType));

            return list;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}