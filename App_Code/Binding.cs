using System.Collections.Generic;
using System.Web;
using Library.Root.Object;

namespace Control
{
    /// <summary>
    /// Component Binding 
    /// ----------------------------------------------
    /// C.C.Yeon    16 April 2012   initial version
    /// </summary>
    public class Binding
    {
        public static void BindDropDownListResource(System.Web.UI.WebControls.DropDownList DDL, string ResourceName, string Text = "", string Value = "")
        {
            DDL.DataSource = Library.Root.Control.Convertion<Binder>.Deserializer((string)HttpContext.GetGlobalResourceObject("SearchSource", ResourceName));
            DDL.DataTextField = "Text";
            DDL.DataValueField = "Value";
            DDL.DataBind();
            AddList(DDL, Text, Value);
        }

        public static void BindDropDownList(System.Web.UI.WebControls.DropDownList DDL, List<Binder> list, string Text = "", string Value = "")
        {
            if (list.Count > 0)
            {
                DDL.DataSource = list;
                DDL.DataTextField = "Text";
                DDL.DataValueField = "Value";
                DDL.DataBind();
            }
            AddList(DDL, Text, Value);
        }

        private static void AddList(System.Web.UI.WebControls.DropDownList ddl, string Text, string Value)
        {
            if (Value != string.Empty)
            {
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Text, Value));
            }
        }
    }
}
