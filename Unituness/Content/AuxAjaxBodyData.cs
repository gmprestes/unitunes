using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SysAdmin.API
{
    public class AjaxBodyData
    {
        public string token { get; set; }
        public string arg { get; set; }
        public string arg2 { get; set; }
        public string arg3 { get; set; }
        public string arg4 { get; set; }
        public string arg5 { get; set; }

        public AjaxBodyData()
        {
            this.token = string.Empty;
            this.arg = string.Empty;
            this.arg2 = string.Empty;
            this.arg3 = string.Empty;
            this.arg4 = string.Empty;
            this.arg5 = string.Empty;
        }
    }
}
