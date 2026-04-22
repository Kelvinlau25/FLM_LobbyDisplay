using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root
{
    public class radiobuttonfield : ITemplate
    {
        public const string RadioButtonItemID = "rbitem";
        private ListItemType plittype;

        public radiobuttonfield(ListItemType type)
            : base()
        {
            this.plittype = type;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            RadioButton rbitem;

            switch (this.plittype)
            {
                case ListItemType.Header:
                    return;
                case ListItemType.Footer:
                    return;
                case ListItemType.Item:
                    rbitem = new RadioButton();
                    rbitem.ID = "rbitem";
                    rbitem.AutoPostBack = true;
                    container.Controls.Add(rbitem);
                    break;
            }
        }
    }
}
