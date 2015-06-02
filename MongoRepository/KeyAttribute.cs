using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace MongoRepository
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
        private string[] _cols;

        public KeyAttribute(params string[] columns)
        {
            this._cols = columns;
        }

        public virtual string[] Columns
        {
            get { return this._cols; }
        }
    }
}
