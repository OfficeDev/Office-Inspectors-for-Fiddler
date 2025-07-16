using System.Windows.Forms;
using Be.Windows.Forms;

namespace MapiInspector
{
    /// <summary>
    /// MAPIControl class used for display
    /// </summary>
    public partial class MAPIControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the MAPIControl class
        /// </summary>
        public MAPIControl(MAPIInspector inspector)
        {
            Inspector = inspector;
            this.InitializeComponent();
        }

        private MAPIInspector Inspector { get; set; }

        /// <summary>
        /// Gets mapiTreeView
        /// </summary>
        public TreeView MAPITreeView
        {
           get
           {
                return this.mapiTreeView;
           }
        }

        /// <summary>
        /// Gets mapiHexBox
        /// </summary>
        public HexBox MAPIHexBox
        {
            get
            {
                return this.mapiHexBox;
            }
        }

        /// <summary>
        /// Gets mapiRichTextBox
        /// </summary>
        public RichTextBox MAPIRichTextBox
        {
            get
            {
                return this.mapiRichTextBox;
            }
        }

        /// <summary>
        /// Gets cropsHexBox
        /// </summary>
        public HexBox CROPSHexBox
        {
            get
            {
                return this.cropsHexBox;
            }
        }

        /// <summary>
        /// Gets splitContainer
        /// </summary>
        public SplitContainer SplitContainer
        {
            get
            {
                return this.splitContainer;
            }
        }
    }
}
