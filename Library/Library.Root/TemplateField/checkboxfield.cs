using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library.Root
{
    public class checkboxfield : ITemplate
    {
        public const string CheckboxHeaderID = "chkall";
        public const string CheckboxItemID = "ckitem";
        private ListItemType plittype;

        public checkboxfield(ListItemType type)
            : base()
        {
            this.plittype = type;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            CheckBox ckitem;
            CheckBox ckall;

            switch (this.plittype)
            {
                case ListItemType.Header:
                    ckall = new CheckBox();
                    ckall.ID = "chkall";
                    ckall.AutoPostBack = true;
                    container.Controls.Add(ckall);
                    break;
                case ListItemType.Footer:
                    return;
                case ListItemType.Item:
                    ckitem = new CheckBox();
                    ckitem.ID = "ckitem";
                    ckitem.AutoPostBack = true;
                    container.Controls.Add(ckitem);
                    break;
            }
        }
    }
}
