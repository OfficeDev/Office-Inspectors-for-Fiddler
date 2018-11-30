using Be.Windows.Forms;
using System.Windows.Forms;

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
            set
            {
                this.TreeView = value;
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

        public ContainerControl FSSHTTPandWOPIContainer
        {
            get
            {
                return this.Container;
            }
        }
    }
}
