using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root
{
    public class viewfield : ITemplate
    {
        public const string LabelHeaderID = "lblview";
        public const string LiteralItemID = "ltrviewitem";
        private ListItemType plittype;

        public viewfield(ListItemType type)
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
                    lbdel.Text = "View";
                    lbdel.ID = "lblview";
                    lbdel.EnableViewState = false;
                    container.Controls.Add(lbdel);
                    break;
                case ListItemType.Item:
                    ltritem = new Literal();
                    ltritem.ID = "ltrviewitem";
                    ltritem.EnableViewState = false;
                    container.Controls.Add(ltritem);
                    break;
            }
        }
    }
}
