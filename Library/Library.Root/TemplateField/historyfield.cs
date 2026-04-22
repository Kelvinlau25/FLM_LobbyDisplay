using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root
{
    public class historyfield : ITemplate
    {
        public const string LabelHeaderID = "lblhis";
        public const string LiteralItemID = "ltrhisitem";
        private ListItemType plittype;

        public historyfield(ListItemType type)
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
                    lbdel.Text = "Audit Trail";
                    lbdel.ID = "lblhis";
                    lbdel.EnableViewState = false;
                    container.Controls.Add(lbdel);
                    break;
                case ListItemType.Item:
                    ltritem = new Literal();
                    ltritem.ID = "ltrhisitem";
                    ltritem.EnableViewState = false;
                    container.Controls.Add(ltritem);
                    break;
            }
        }
    }
}
