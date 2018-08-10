using System.Windows.Forms;
using System.Drawing;
using System;
using System.Text;

namespace FSSHTTPandWOPIInspector
{
    public partial class FSSHTTPandWOPIControl
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
            this.TreeView = new System.Windows.Forms.TreeView();
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Container = new System.Windows.Forms.ContainerControl();
            this.HexBox = new Be.Windows.Forms.HexBox();
            this.splitter = new System.Windows.Forms.Splitter();
            this.Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            //this.TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            //this.TreeView.Size = new System.Drawing.Size(879, 475);
            this.TreeView.TabIndex = 0;

            this.TreeView.Size = new System.Drawing.Size(1700, 475);
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            // 
            // RichTextBox
            // 
            this.RichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBox.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.Size = new System.Drawing.Size(1700, 472);
            this.RichTextBox.TabIndex = 2;
            this.RichTextBox.Text = "";
            this.RichTextBox.Visible = false;
            // 
            // Container
            // 
            this.Container.AutoScroll = true;
            this.Container.Controls.Add(this.HexBox);
            this.Container.Cursor = System.Windows.Forms.Cursors.Default;
            //this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container.Location = new System.Drawing.Point(879, 0);
            this.Container.Name = "Container";
            //this.Container.Size = new System.Drawing.Size(93, 475);
            this.Container.TabIndex = 6;

            this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container.Size = new System.Drawing.Size(2, 475);
            // 
            // HexBox
            // 
            this.HexBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.HexBox.BodyOffset = 0;
            this.HexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.HexBox.Location = new System.Drawing.Point(0, 0);
            this.HexBox.Name = "HexBox";
            this.HexBox.SelectionLength = ((long)(0));
            this.HexBox.SelectionStart = ((long)(-1));
            this.HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.HexBox.Size = new System.Drawing.Size(93, 475);
            this.HexBox.TabIndex = 2;
            this.HexBox.VScrollBarVisible = true;


            ContextMenu cm = new ContextMenu();
            this.HexBox.ContextMenu = cm;
            MenuItem item = this.HexBox.ContextMenu.MenuItems.Add("Copy (no spaces)");
            item.Click += new EventHandler(FSSWOPI_Copy);
            MenuItem item4 = this.HexBox.ContextMenu.MenuItems.Add("Copy (with spaces)");
            item4.Click += new EventHandler(FSSWOPI_CopyWithSpaces);
            MenuItem item2 = this.HexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks");
            item2.Click += new EventHandler(FSSWOPI_CopyAsByteBlocks);
            MenuItem item3 = this.HexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks (with prefix)");
            item3.Click += new EventHandler(FSSWOPI_CopyAsByteBlocksWithPrefix);
            MenuItem item5 = this.HexBox.ContextMenu.MenuItems.Add("Copy as 0x00 code block");
            item5.Click += new EventHandler(FSSWOPI_CopyAsCodeBlock);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(879, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 475);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            // 
            // FSSHTTPandWOPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.Container);
            this.Controls.Add(this.RichTextBox);
            this.Controls.Add(this.TreeView);
            this.Name = "FSSHTTPandWOPIControl";
            this.Size = new System.Drawing.Size(972, 475);
            this.Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void CopyMethod(object sender, EventArgs e, Be.Windows.Forms.HexBox hexBox)
        {
            byte[] targetBytes = new byte[hexBox.SelectionLength];
            Array.Copy(hexBox.GetAllBytes(), hexBox.SelectionStart, targetBytes, 0, hexBox.SelectionLength);
            string hex = BitConverter.ToString(targetBytes).Replace("-", string.Empty);
            Clipboard.SetText(hex);
        }

        void FSSWOPI_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.HexBox);
        }

        //void CROPS_Copy(object sender, EventArgs e)
        //{
        //    CopyMethod(sender, e, this.CROPSHexBox);
        //}

        private void MapiTreeViewMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.TreeView.SelectedNode != null)
            {
                Clipboard.SetText(this.TreeView.SelectedNode.Text);
            }
        }

        private void MapiTreeViewMenuItem2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            GetNodeTreeText(sb, TreeView.SelectedNode ?? TreeView.Nodes[0], -1);
            Clipboard.SetText(sb.ToString());
        }

        private void MapiTreeViewMenuItem3_Click(object sender, EventArgs e)
        {
            var node = TreeView.SelectedNode ?? TreeView.Nodes[0];
            node.ExpandAll();
        }

        private void MapiTreeViewMenuItem4_Click(object sender, EventArgs e)
        {
            var node = TreeView.SelectedNode ?? TreeView.Nodes[0];
            node.Collapse();
        }

        private void GetNodeTreeText(StringBuilder sb, TreeNode node, int count)
        {
            var indents = ++count;
            for (int i = 0; i < indents; i++)
                sb.Append("   ");
            sb.AppendLine(node.Text.Replace("\0", "\\0"));
            foreach (var n in node.Nodes)
                GetNodeTreeText(sb, n as TreeNode, indents);
        }

        private void FSSWOPI_CopyWithSpaces(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[HexBox.SelectionLength];
            Array.Copy(HexBox.GetAllBytes(), HexBox.SelectionStart, targetBytes, 0, HexBox.SelectionLength);

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

        private void FSSWOPI_CopyAsByteBlocks(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[HexBox.SelectionLength];
            Array.Copy(HexBox.GetAllBytes(), HexBox.SelectionStart, targetBytes, 0, HexBox.SelectionLength);

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

        private void FSSWOPI_CopyAsByteBlocksWithPrefix(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[HexBox.SelectionLength];
            Array.Copy(HexBox.GetAllBytes(), HexBox.SelectionStart, targetBytes, 0, HexBox.SelectionLength);

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
                    sb.Append($"{(counter / 16) * 16:X8} | ");
                }
                else if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void FSSWOPI_CopyAsCodeBlock(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[HexBox.SelectionLength];
            Array.Copy(HexBox.GetAllBytes(), HexBox.SelectionStart, targetBytes, 0, HexBox.SelectionLength);

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

        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.ContainerControl Container;
        private Be.Windows.Forms.HexBox HexBox;
        private System.Windows.Forms.Splitter splitter;
    }
}
