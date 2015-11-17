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

        #endregion

        private System.Windows.Forms.TreeView mapiTreeView;
        private System.Windows.Forms.RichTextBox mapiRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Be.Windows.Forms.HexBox mapiHexBox;
        private Be.Windows.Forms.HexBox cropsHexBox;
        private System.Windows.Forms.Splitter splitter;
    }
}
