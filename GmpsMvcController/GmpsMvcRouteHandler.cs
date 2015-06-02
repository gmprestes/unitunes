using GmpsMvcController.Handler;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace GmpsMvcController
{
    public sealed class GmpsMvcRouteHandler : IRouteHandler
    {
        private readonly IHttpHandler _instancia;

        public GmpsMvcRouteHandler()
        {
            this._instancia = (IHttpHandler)Activator.CreateInstance(typeof(GmpsMvcHttpHandler), new object[] { Assembly.GetCallingAssembly() }, null);
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return _instancia;
        }

    }
}
