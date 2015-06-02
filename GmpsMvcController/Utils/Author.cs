using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmpsMvcController.Utils
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct | System.AttributeTargets.Method)]
    internal class Author : System.Attribute
    {
        private string name;
        public double version;

        public Author(string name)
        {
            this.name = name;
            version = 1.0;
        }

        public Author(string name, double version)
        {
            this.name = name;
            this.version = version;
        }
    }




}
