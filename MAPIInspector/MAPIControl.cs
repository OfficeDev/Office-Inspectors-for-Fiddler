using System.Windows.Forms;
using Be.Windows.Forms;

namespace MapiInspector
{
    public partial class MAPIControl : UserControl
    {
        public MAPIControl()
        {
            InitializeComponent();
        }

        public TreeView MAPITreeView
        {
           get
           {
                return this.mapiTreeView;
           }
        }

        public HexBox MAPIHexBox
        {
            get
            {
                return this.mapiHexBox;
            }
        }

    }
}
