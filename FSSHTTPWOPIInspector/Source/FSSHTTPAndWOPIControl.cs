using System.Windows.Forms;
using Be.Windows.Forms;

namespace FSSHTTPandWOPIInspector
{
    public partial class FSSHTTPandWOPIControl : UserControl
    {
        public FSSHTTPandWOPIControl()
        {
            InitializeComponent();
        }

        public TreeView FSSHTTPandWOPITreeView
        {
           get
           {
                return this.TreeView;
           }
        }

        public HexBox FSSHTTPandWOPIHexBox
        {
            get
            {
                return this.HexBox;
            }
        }

        public RichTextBox FSSHTTPandWOPIRichTextBox
        {
            get
            {
                return this.RichTextBox;
            }
        }

        public SplitContainer FSSHTTPandWOPISplitContainer
        {
            get
            {
                return this.splitContainer;
            }
        }
    }
}
