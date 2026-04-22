using System.ComponentModel;

namespace Library.Root.LacalizeLabel
{
    public partial class LocalLabel
    {
        // Control overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose();
            }
        }

        // Required by the Control Designer
        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
    }
}
