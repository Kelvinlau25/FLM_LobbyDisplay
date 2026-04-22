using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root
{
    public class deletefield : ITemplate
    {
        public const string LabelHeaderID = "lbldel";
        public const string LiteralItemID = "ltritem";
        private ListItemType plittype;

        public deletefield(ListItemType type)
            : base()
        {
            this.plittype = type;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            Literal ltritem;
            Label lbdel;

            switch (this.plittype)
            {
                case ListItemType.Header:
                    lbdel = new Label();
                    lbdel.Text = "Del.";
                    lbdel.ID = "lbldel";
                    lbdel.EnableViewState = false;
                    container.Controls.Add(lbdel);
                    break;
                case ListItemType.Item:
                    ltritem = new Literal();
                    ltritem.ID = "ltritem";
                    ltritem.EnableViewState = false;
                    container.Controls.Add(ltritem);
                    break;
            }
        }
    }
}
