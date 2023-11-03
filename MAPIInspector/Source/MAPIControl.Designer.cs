using System.Windows.Forms;
using System;
using System.Text;

namespace MapiInspector
{
    public partial class MAPIControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mapiTreeView = new System.Windows.Forms.TreeView();
            this.mapiRichTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.mapiHexBox = new Be.Windows.Forms.HexBox();
            this.cropsHexBox = new Be.Windows.Forms.HexBox();
            this.splitter = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapiTreeView
            // 
            this.mapiTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.mapiTreeView.Location = new System.Drawing.Point(0, 0);
            this.mapiTreeView.Name = "mapiTreeView";
            this.mapiTreeView.Size = new System.Drawing.Size(424, 472);
            this.mapiTreeView.TabIndex = 0;
            ContextMenu mapiTreeViewContextMenu = new ContextMenu();
            this.mapiTreeView.ContextMenu = mapiTreeViewContextMenu;
            MenuItem mapiTreeViewMenuItem1 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy selected text");
            MenuItem mapiTreeViewMenuItem2 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy tree");
            MenuItem mapiTreeViewMenuItem3 = this.mapiTreeView.ContextMenu.MenuItems.Add("Expand");
            MenuItem mapiTreeViewMenuItem4 = this.mapiTreeView.ContextMenu.MenuItems.Add("Collapse");
            mapiTreeViewMenuItem1.Click += new EventHandler(MapiTreeViewMenuItem1_Click);
            mapiTreeViewMenuItem2.Click += new EventHandler(MapiTreeViewMenuItem2_Click);
            mapiTreeViewMenuItem3.Click += new EventHandler(MapiTreeViewMenuItem3_Click);
            mapiTreeViewMenuItem4.Click += new EventHandler(MapiTreeViewMenuItem4_Click);
            // 
            // mapiRichTextBox
            // 
            this.mapiRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.mapiRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mapiRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.mapiRichTextBox.Name = "mapiRichTextBox";
            this.mapiRichTextBox.Size = new System.Drawing.Size(424, 472);
            this.mapiRichTextBox.TabIndex = 2;
            this.mapiRichTextBox.Text = "";
            this.mapiRichTextBox.Visible = false;
            // 
            // splitContainer
            // 
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(424, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.Controls.Add(this.mapiHexBox);
            this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.cropsHexBox);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Size = new System.Drawing.Size(644, 472);
            this.splitContainer.SplitterDistance = 214;
            this.splitContainer.TabIndex = 4;
            // 
            // mapiHexBox
            // 
            this.mapiHexBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.mapiHexBox.BodyOffset = 0;
            this.mapiHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapiHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapiHexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.mapiHexBox.Location = new System.Drawing.Point(0, 0);
            this.mapiHexBox.Name = "mapiHexBox";
            this.mapiHexBox.SelectionLength = ((long)(0));
            this.mapiHexBox.SelectionStart = ((long)(-1));
            this.mapiHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mapiHexBox.Size = new System.Drawing.Size(644, 472);
            this.mapiHexBox.TabIndex = 2;
            this.mapiHexBox.VScrollBarVisible = true;
            this.mapiHexBox.CanCopy();

            ContextMenu cm = new ContextMenu();
            this.mapiHexBox.ContextMenu = cm;
            MenuItem item = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy (no spaces)");
            item.Click += new EventHandler(MAPI_Copy);
            MenuItem item4 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy (with spaces)");
            item4.Click += new EventHandler(MAPI_CopyWithSpaces);
            MenuItem item2 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks");
            item2.Click += new EventHandler(MAPI_CopyAsByteBlocks);
            MenuItem item3 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks (with prefix)");
            item3.Click += new EventHandler(MAPI_CopyAsByteBlocksWithPrefix);
            MenuItem item5 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 0x00 code block");
            item5.Click += new EventHandler(MAPI_CopyAsCodeBlock);


            // 
            // cropsHexBox
            // 
            this.cropsHexBox.BodyOffset = 1;
            this.cropsHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cropsHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cropsHexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.cropsHexBox.Location = new System.Drawing.Point(0, 0);
            this.cropsHexBox.Name = "cropsHexBox";
            this.cropsHexBox.SelectionLength = ((long)(0));
            this.cropsHexBox.SelectionStart = ((long)(-1));
            this.cropsHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.cropsHexBox.Size = new System.Drawing.Size(150, 46);
            this.cropsHexBox.TabIndex = 4;
            this.cropsHexBox.VScrollBarVisible = true;

            ContextMenu cm_crops = new ContextMenu();
            this.cropsHexBox.ContextMenu = cm_crops;
            MenuItem item_crops = this.cropsHexBox.ContextMenu.MenuItems.Add("Copy");
            item_crops.Click += new EventHandler(CROPS_Copy);


            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(424, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 472);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            // 
            // MAPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mapiRichTextBox);
            this.Controls.Add(this.mapiTreeView);
            this.Name = "MAPIControl";
            this.Size = new System.Drawing.Size(1068, 472);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void CopyMethod(object sender, EventArgs e, Be.Windows.Forms.HexBox hexBox)
        {
            byte[] targetBytes = new byte[hexBox.SelectionLength];
            Array.Copy(hexBox.GetAllBytes(), hexBox.SelectionStart, targetBytes, 0, hexBox.SelectionLength);
            string hex = BitConverter.ToString(targetBytes).Replace("-", string.Empty);
            if (!string.IsNullOrEmpty(hex))
            {
                Clipboard.SetText(hex);
            }
        }

        void MAPI_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.mapiHexBox);
        }

        void CROPS_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.CROPSHexBox);
        }

        /// <summary>
        /// Cleans a string by removing trailing null characters and replacing internal null characters with "\\0".
        /// </summary>
        /// <param name="text">The input string to be cleaned.</param>
        /// <returns>A cleaned string with null characters treated as specified.</returns>
        private string CleanString(string text)
        {
            // Remove any trailing null characters
            // Replace internal null characters with "\\0"
            return text.TrimEnd('\0').Replace("\0", "\\0");
        }

        private void MapiTreeViewMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.mapiTreeView.SelectedNode != null)
            {
                Clipboard.SetText(CleanString(this.mapiTreeView.SelectedNode.Text));
            }
        }

        private void MapiTreeViewMenuItem2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            GetNodeTreeText(sb, mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0], -1);
            Clipboard.SetText(sb.ToString());
        }

        private void MapiTreeViewMenuItem3_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.ExpandAll();
        }

        private void MapiTreeViewMenuItem4_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.Collapse();
        }

        private void GetNodeTreeText(StringBuilder sb, TreeNode node, int count)
        {
            var indents = ++count;
            for (int i = 0; i < indents; i++)
                sb.Append("   ");
            sb.AppendLine(CleanString(node.Text));
            foreach (var n in node.Nodes)
                GetNodeTreeText(sb, n as TreeNode, indents);
        }

        private void MAPI_CopyWithSpaces(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void MAPI_CopyAsByteBlocks(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if ((counter % 16) == 0 && counter != 0)
                    sb.AppendLine();
                else if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void MAPI_CopyAsByteBlocksWithPrefix(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("POSITION | 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
            sb.AppendLine("----------------------------------------------------------");

            int counter = 0;
            foreach (var c in targetBytes)
            {
                if ((counter % 16) == 0)
                {
                    if (counter != 0)
                        sb.AppendLine();
                    sb.Append("${(counter / 16) * 16:X8} | ");
                }
                else if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void MAPI_CopyAsCodeBlock(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder("byte[] arrOutput = { ");
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if (counter != 0)
                    sb.Append(", ");
                counter++;
                sb.Append("0x" + c.ToString("x2"));
            }

            sb.Append("};");
            Clipboard.SetText(sb.ToString());
        }

        #endregion

        private System.Windows.Forms.TreeView mapiTreeView;
        private System.Windows.Forms.RichTextBox mapiRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Be.Windows.Forms.HexBox mapiHexBox;
        private Be.Windows.Forms.HexBox cropsHexBox;
        private System.Windows.Forms.Splitter splitter;
    }
}
