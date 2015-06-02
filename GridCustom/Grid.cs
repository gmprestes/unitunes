using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Specialized;

namespace GridCustom
{
    [Themeable(true)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), ParseChildren(true), ToolboxData("<{0}:GridViewCustom runat=server></{0}:GridViewCustom>")]
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class Grid : System.Web.UI.WebControls.WebControl
    {
        public int TotalPages
        {
            get { return (int)(ViewState["vTotalPages"] ?? 1); }
            set { ViewState["vTotalPages"] = value; }
        }

        public int PageSize
        {
            get { return (int)(ViewState["PageSize"] ?? 1); }
            set { ViewState["PageSize"] = value; }
        }

        public int CurrentPage
        {
            get { return (int)(ViewState["CurrentPage"] ?? 1); }
            set { ViewState["CurrentPage"] = value; }
        }

        public string OrderBy
        {
            get { return (string)(ViewState["OrderBy"] ?? string.Empty); }
            set { ViewState["OrderBy"] = value; }
        }

        [Bindable(true),Category("Customizacoes"), DefaultValue(""), Description("Nomes das DataKeys separadas por ponto e virgula"), Localizable(true)]
        public string DataKeyNames
        {
            get { return (string)(ViewState["dataKeysNames"] ?? null); }
            set { ViewState["dataKeysNames"] = value; }
        }

        private string PageContext
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Url.AbsolutePath.Replace("http://", string.Empty).Replace("https://", string.Empty);
            }
        }

        public event EventHandler GridEvent;
        protected void OnGridEvent(EventArgs e)
        {
            if (GridEvent != null)
                GridEvent(this, e);
        }

       

        public IList DataSource
        {
            get
            {
                if (ViewState["___collection"] == null)
                    return null;
                return ViewState["___collection"] as IList;
            }
            set { ViewState["___collection"] = value; }
        }

        private void RegisterScript()
        {
            StringBuilder strScript = new StringBuilder();
            strScript.Append("var current = 1;");

            strScript.Append("function goToPage(value) { ");
            strScript.Append("current = value; ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Paginar_' + current);");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");


            strScript.Append("function goToLastPage() { ");
            strScript.AppendFormat("current = {0}; ", this.TotalPages);
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Paginar_' + current);");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" }");

            strScript.Append("function goToFirstPage() { ");
            strScript.Append("current = 1; ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Paginar_' + current);");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" }");

            strScript.Append("function novo() { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Inserir');");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            strScript.Append("function editar() { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Alterar');; ");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            strScript.Append("function excluir() { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Excluir'); ");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            strScript.Append("function buscar() { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('Buscar'); ");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            strScript.Append("function clicklinha() { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val('RowClick'); ");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            strScript.Append("function evento(ev) { ");
            strScript.Append("$('#contentCorpo_HiddenEvent').val(ev); ");
            strScript.Append("WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$contentCorpo$btEvent', '', true, '', '', false, true))");
            strScript.Append(" } ");

            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "PagingState", strScript.ToString(), true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.RegisterScript();

            StringBuilder css = new StringBuilder();
            css.Append("<style type=\"text/css\" rel=\"stylesheet\">");
            css.Append(".pagination { ");
            css.Append(" width:98%;");
            css.Append(" float:left;");
            css.Append(" padding:0; !important;");
            css.Append(" margin:10px 0 0 0 !important;");
            css.Append(" height:30px !important;");
            css.Append(" } ");
            css.Append(".pagination ul { ");
            css.Append(" padding:0; !important;");
            css.Append(" margin:0 !important;");
            css.Append(" height:30px !important;");
            css.Append(" float:left;");
            css.Append(" } ");
            css.Append(".table { ");
            css.Append(" margin:0 !important;");
            css.Append(" } ");
            css.Append("</style>");

            LiteralControl ltr = new LiteralControl();
            ltr.Text = css.ToString();

            this.Page.Header.Controls.Add(ltr);
        }
        
        


        public List<CustomKey> _selectedDataKeys;
        public List<CustomKey> SelectedDataKeys
        {
            get
            {
                var keys = new List<CustomKey>();

                var hidden = (HiddenField)this.FindControl("dataKeys");

                if (this.DataSource == null || this.DataSource.Count == 0)
                    return keys;
                try
                {
                    var properties = this.DataSource[0].GetType().GetProperties();

                    var stringKeysIndex = hidden.Value.TrimEnd(',');
                    var stringKeysNames = this.DataKeyNames.Split(',');


                    if (string.IsNullOrEmpty(stringKeysIndex) || string.IsNullOrEmpty(this.DataKeyNames) || this.DataSource == null)
                        return keys;

                    foreach (int index in stringKeysIndex.Replace(", ", string.Empty).Replace(" ,", string.Empty).Split(',').Select(q => Convert.ToInt32(q)))
                    {
                        object data = this.DataSource[index];
                        var key = new CustomKey();
                        foreach (var keyName in stringKeysNames)
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                if (property.Name.ToLower() == keyName.Trim().ToLower())
                                {
                                    var first = key.Values.Count == 0;
                                    if (first)
                                        key.Value = property.GetValue(data, null);

                                    key.Add(keyName, property.GetValue(data, null));
                                }
                            }
                        }

                        keys.Add(key);
                    }
                }
                catch
                {
                }

                this._selectedDataKeys = keys;

                return this._selectedDataKeys;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter writer = new HtmlTextWriter(sw);

                this.RenderControl(writer);

                var html = sb;

                this._selectedDataKeys = value;
            }
        }

        protected override void Render(HtmlTextWriter outPut)
        {
            try
            {
                //this.EnsureChildControls();

                if (!Visible)
                    return;

                //Atribui o css class grid ao Table do grid
                //this.Attributes.Add("class", "responsive table table-bordered");
                this.Attributes.Add("class", "table table-bordered");


                // Captura o html default do grid (é apenas um TABLE)
                StringWriter objWriter = new StringWriter();
                HtmlTextWriter objBuffer = new HtmlTextWriter(objWriter);


                base.Render(objBuffer);

                StringBuilder html = new StringBuilder();
                HtmlTextWriter writer = new HtmlTextWriter(new System.IO.StringWriter(html, System.Globalization.CultureInfo.InvariantCulture));


                if (this.DataSource != null && this.DataSource.Count > 0)
                {
                    html.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"table table-bordered\">");
                    html.Append("<thead>");
                    html.Append("<tr>");

                    var properties = this.DataSource[0].GetType().GetProperties();

                    for (int i = 0; i <= properties.Count(); i++)
                    {
                        

                        if (i == 0)
                            html.AppendFormat("<th style=\"width:15px\" ><input type=\"checkbox\" onclick=\"SetCheck(this);\" /></th>");
                        else
                        {
                            var property = properties[i - 1];

                            if (!property.Name.Equals("Id"))
                            {
                                string columnName = property.Name;
                                foreach (object attr in property.GetCustomAttributes(true))
                                {
                                    if (attr.GetType().Name.Contains("ColumnProperties"))
                                    {
                                        ColumnPropertiesAttribute attribute = (ColumnPropertiesAttribute)attr;
                                        attribute.CollumnEntityName = columnName;
                                        if (attribute.ColumnVisibility)
                                        {
                                            var order = attribute.CollumnEntityName == this.OrderBy.Replace(" desc", string.Empty).Replace(" asc", string.Empty);
                                            html.AppendFormat("<th style=\"width:{1}px\" {3}><a href=\"#\" onclick=\"evento('Ordenar_{2}'); return false;\">{0}</a></th>", attribute.Alias, attribute.Width, (this.OrderBy.Split(' ')[1].Contains("desc") ? attribute.CollumnEntityName + " asc" : attribute.CollumnEntityName + " desc"), order ? "style=\"color:#000 !important; font-weight:bold !important;\"" : "");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    

                    html.Append("</tr>");
                    html.Append("</thead>");


                    html.Append("<tbody>");

                    var index = 0;
                    foreach (object data in this.DataSource)
                    {
                        html.Append("<tr ondblclick=\"$('table.responsive tr input:checkbox').attr('checked', false); $(this).find('input:checkbox').attr('checked', true); SelectedKeys(); clicklinha();\">");

                        for (int i = 0; i <= properties.Count(); i++)
                        {
                            if (i == 0)
                            {
                                html.AppendFormat("<td><input type=\"checkbox\" arrayindex=\"{0}\" /></td>", index);
                            }
                            else
                            {
                                var property = properties[i - 1];

                                if (!property.Name.Equals("Id"))
                                {
                                    object objValue = property.GetValue(data, null);

                                    string columnName = property.Name;
                                    foreach (object attr in property.GetCustomAttributes(true))
                                    {
                                        if (attr.GetType().Name.Contains("ColumnProperties"))
                                        {
                                            ColumnPropertiesAttribute attribute = (ColumnPropertiesAttribute)attr;
                                            if (attribute.ColumnVisibility)
                                            {
                                                if (objValue != null)
                                                {
                                                    switch (objValue.GetType().Name)
                                                    {
                                                        case "Boolean":
                                                            html.AppendFormat("<td>{0}</td>", (bool)objValue ? "sim" : "não");
                                                            break;
                                                        default:
                                                            html.AppendFormat("<td>{0}</td>", string.Format(string.IsNullOrEmpty(attribute.Format) ? "{0}" : attribute.Format, objValue));
                                                            break;
                                                    }
                                                }
                                                else
                                                    html.Append("<td></td>");
                                            }
                                        }
                                    }
                                }
                            }                            
                        }

                        html.Append("</tr>");
                        index++;
                    }


                    html.Append("</tbody>");
                    html.Append("</table>");
                }
                else
                {
                    html.Append("<h3>Nenhuma Informação Cadastrada</h3>");
                }

                #region Pager
                html.Append(this.Pager());
                #endregion


                string id = this.UniqueID;

                #region Scripts
                html.Append("<script type=\"text/javascript\">");
                html.Append("function SetCheck(chk) { ");
                html.Append(" var checked = chk.checked; ");
                html.Append(" $(\"table tbody tr td input:checkbox\").prop('checked', checked); ");
                html.Append(" SelectedKeys(); ");
                html.Append(" } ");

                html.Append("function SelectedKeys() { ");
                html.Append(" var keys = ''; ");
                html.Append(" $('#dataKeys').val(''); ");
                html.Append(" $(\"table tbody tr td input:checked\").each(function () { keys += ($(this).attr('arrayindex') + ','); });");
                html.Append(" $('#dataKeys').val(keys); ");
                html.Append(" } ");

                html.Append(" $(\"table tbody tr td input:checkbox\").click(function () { SelectedKeys(); } );");
                html.Append("</script>");
                #endregion

                outPut.Write(html.ToString());

            }
            catch
            {
                StringBuilder sb = new StringBuilder();
                HtmlTextWriter htw = new HtmlTextWriter(new System.IO.StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture));
                outPut.Write(sb);
            }
        }

        public string Pager()
        {
            StringBuilder html = new StringBuilder();

            if (this.TotalPages > 1)
            {
                if (this.TotalPages > 40)
                    this.TotalPages = 40;

                html.Append("<div class=\"pagination\">");
                html.Append("<ul>");
                html.Append("<li><a href=\"#\" onclick=\"goToFirstPage()\"><span class=\"icon12 icomoon-icon-arrow-left\"></span></a></li>");

                for (int i = 1; i <= this.TotalPages; i++)
                {
                    html.AppendFormat("<li {1}><a href=\"#\" onclick=\"goToPage({0});\">{0}</a></li>", i, i == this.CurrentPage ? "class=\"active\"" : string.Empty);
                }

                html.Append("<li><a href=\"#\" onclick=\"goToLastPage()\"><span class=\"icon12 icomoon-icon-arrow-right\"></span></a></li>");

                html.Append("</ul>");
                html.Append("</div>");
            }


            return html.ToString();
        }
    }

    public class CustomKey
    {
        private List<KeyValue> _values;
        private object _value;

        public CustomKey()
        {
            this._values = new List<KeyValue>();
        }

        public object this[int index] { get { return this._values.ToList()[index].Value; } }

        public object this[string name]
        {
            get
            {
                return this._values.FirstOrDefault(q => q.Name == name).Value;
            }
        }

        public List<KeyValue> Values
        {
            get
            {
                return this._values;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public void Add(string name, object value)
        {
            this._values.Add(new KeyValue() { Name = name, Value = value }); 
        }
    }

    public class KeyValue
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

}
