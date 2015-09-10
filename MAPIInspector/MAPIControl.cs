using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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

        public TreeView TreeView1
        {
           get
           {
                return this.treeView1;
           }
        }

        public HexBox HexBox1
        {
            get
            {
                return this.hexBox1;
            }
        }

    }
}
