using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace GridCustom
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    [Serializable]
    public class ColumnPropertiesAttribute : Attribute
    {
        private string aling;
        private string alias;
        private bool columnvisibility;
        private bool pivotvisibility;
        private bool dontexport;
        private string collumnentityname;
        private string wherecondition;
        private int width;
        private string format;
        private int maxLength;
        private string colors;
        private string color;

        public ColumnPropertiesAttribute(string aling, string alias)
        {
            this.aling = aling;
            this.alias = alias;
        }

        public ColumnPropertiesAttribute(string aling, string alias, string[] dados)
        {
            this.aling = aling;
            this.alias = alias;
            this.Dados = dados;
        }

        public virtual string Aling
        {
            get { return aling; }
        }

        public virtual string Alias
        {
            get { return alias; }
        }

        public virtual bool ColumnVisibility
        {
            get { return columnvisibility; }
            set { columnvisibility = value; }
        }

        public virtual bool PivotVisibility
        {
            get { return pivotvisibility; }
            set { pivotvisibility = value; }
        }

        public virtual bool DontExport
        {
            get { return dontexport; }
            set { dontexport = value; }
        }

        public virtual string CollumnEntityName
        {
            get { return collumnentityname; }
            set { collumnentityname = value; }
        }

        public virtual int Width
        {
            get { return width; }
            set { width = value; }
        }

        public virtual string WhereCondition
        {
            get { return wherecondition; }
            set { wherecondition = value; }
        }

        public virtual string Format
        {
            get { return this.format; }
            set { this.format = value; }
        }

        public virtual int MaxLength
        {
            get { return this.maxLength; }
            set { this.maxLength = value; }
        }

        public virtual string Colors
        {
            get { return this.colors; }
            set { this.colors = value; }
        }

        public virtual string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public string[] Dados { get; private set; }
    }
}
