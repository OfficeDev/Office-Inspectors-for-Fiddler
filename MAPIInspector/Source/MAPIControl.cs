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
        public MAPIControl()
        {
            this.InitializeComponent();
            this.InitializeContextMenus();
        }

        public MAPIInspector Inspector { get; set; }

        /// <summary>
        /// Gets mapiTreeView
        /// </summary>
        public TreeView MAPITreeView => this.mapiTreeView;

        /// <summary>
        /// Gets mapiHexBox
        /// </summary>
        public HexBox MAPIHexBox => this.mapiHexBox;

        /// <summary>
        /// Gets cropsHexBox
        /// </summary>
        public HexBox CROPSHexBox => this.cropsHexBox;

        /// <summary>
        /// Gets splitContainer
        /// </summary>
        public SplitContainer SplitContainer => this.splitContainer;
    }
}