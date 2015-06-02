using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;
using System.Reflection;

namespace AET.Code
{
    public class HttpHandlerRoute : IRouteHandler
    {
        private String _VirtualPath = null;

        public HttpHandlerRoute(String virtualPath)
        {
            _VirtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler httpHandler = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));
            return httpHandler;
        }

    }
}