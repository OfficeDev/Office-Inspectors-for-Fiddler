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

        public RichTextBox MAPIRichTextBox
        {
            get
            {
                return this.mapiRichTextBox;
            }
        }

        public HexBox CROPSHexBox
        {
            get
            {
                return this.cropsHexBox;
            }
        }

        public SplitContainer SplitContainer
        {
            get
            {
                return this.splitContainer;
            }
        }
    }
}
