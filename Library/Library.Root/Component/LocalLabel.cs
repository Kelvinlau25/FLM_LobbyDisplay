using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root.LacalizeLabel
{
    [ToolboxData("<{0}:LocalLabel ID=\"LocalLabel\" runat=\"server\"></{0}:LocalLabel>")]
    public partial class LocalLabel : WebControl
    {
        internal ResourceManager _rm;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Key
        {
            get
            {
                return (ViewState["LocalLabel_Text"] != null) ? (string)ViewState["LocalLabel_Text"] : "";
            }
            set
            {
                ViewState["LocalLabel_Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                if (_rm == null)
                {
                    _rm = new ResourceManager("resources.Language", Assembly.Load("App_GlobalResources"));
                }

                return (this.Key != "") ? _rm.GetString(this.Key, Thread.CurrentThread.CurrentCulture).Trim() : "";
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.WriteEncodedText(this.Text);
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
        }
    }
}
