using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace GmpsMvcController.Controller
{
    public class GmpsController : IGmpsController
    {
        #region atributos

        internal string _clientIP { get; set; }
        internal string _clientHostName { get; set; }
        internal string _requestRawUrl { get; set; }
        internal string _requestUrl { get; set; }
        internal string _requestUrlReferrer { get; set; }
        internal string _requestQuery { get; set; }
        internal string _requestEncoding { get; set; }
        internal int _requestLength { get; set; }
        internal HttpCookieCollection _requestCookies { get; set; }
        internal NameValueCollection _requestHeaders { get; set; }
        internal string _requestHttpMethod { get; set; }
        internal string[] _requestUserLanguages { get; set; }
        internal bool _requestIsAuthenticated { get; set; }
        internal bool _requestIsSecureConnection { get; set; }
        internal string _requestBrowser { get; set; }
        internal WindowsIdentity _requestLogonUserIdentity { get; set; }
        internal HttpClientCertificate _requestClientCertificate { get; set; }
        internal string _requestUserAgent { get; set; }
        internal Stream _requestInputStream { get; set; }
        internal NameValueCollection _requestForm { get; set; }
        internal HttpResponse _response { get; set; }


        #endregion

        // construtor
        public GmpsController()
        {
        }

        #region interface methods
        public string ClientIP()
        {
            return _clientIP;
        }

        public string ClientHostName
        {
            get { return _clientHostName; }
        }

        public string RequestUrl
        {
            get
            {
                return _requestUrl;
            }
        }

        public string RequestRawUrl
        {
            get
            {
                return _requestRawUrl;
            }
        }

        public string RequestUrlReferrer
        {
            get
            {
                return _requestUrlReferrer;
            }
        }

        public string RequestQuery
        {
            get
            {
                return _requestQuery;
            }
        }

        public string RequestEncoding
        {
            get
            {
                return _requestEncoding;
            }
        }

        /// <summary>
        /// Represents the length, in byte, of content sent by the client
        /// </summary>
        public int RequestLength
        {
            get
            {
                return _requestLength;
            }
        }

        public string RequestBrowser
        {
            get
            {
                return _requestBrowser;
            }
        }

        public HttpCookieCollection RequestCookie
        {
            get
            {
                return _requestCookies;
            }
        }

        public NameValueCollection RequestHeaders
        {
            get
            {
                return _requestHeaders;
            }
        }

        public string RequestHttpMethod
        {
            get
            {
                return _requestHttpMethod;
            }
        }

        public string[] RequestUserLanguages
        {
            get
            {
                return _requestUserLanguages;
            }
        }

        public bool RequestIsAuthenticated
        {
            get
            {
                return _requestIsAuthenticated;
            }
        }

        public bool RequestIsSecureConnection
        {
            get
            {
                return _requestIsSecureConnection;
            }
        }

        public WindowsIdentity RequestLogonUserIdentity
        {
            get
            {
                return _requestLogonUserIdentity;
            }
        }

        public HttpClientCertificate RequestClientCertificate
        {
            get
            {
                return _requestClientCertificate;
            }
        }

        public string RequestUserAgent
        {
            get
            {
                return _requestUserAgent;
            }
        }

        public Stream RequestInputStream
        {
            get
            {
                return _requestInputStream;
            }
        }

        public NameValueCollection RequestForm
        {
            get
            {
                return _requestForm;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return _response;
            }
        }

        #endregion
    }
}
