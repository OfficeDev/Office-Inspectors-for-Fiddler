using System.Drawing;
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
            this.HexBox = new Be.Windows.Forms.HexBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(691, 560);
            this.TreeView.TabIndex = 0;
            // 
            // RichTextBox
            // 
            this.RichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.Size = new System.Drawing.Size(691, 560);
            this.RichTextBox.TabIndex = 1;
            this.RichTextBox.Text = "";
            this.RichTextBox.Visible = false;
            // 
            // HexBox
            // 
            this.HexBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.HexBox.BodyOffset = 0;
            this.HexBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.HexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.HexBox.Location = new System.Drawing.Point(0, 0);
            this.HexBox.Name = "HexBox";
            this.HexBox.SelectionLength = ((long)(0));
            this.HexBox.SelectionStart = ((long)(-1));
            this.HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.HexBox.Size = new System.Drawing.Size(434, 560);
            this.HexBox.TabIndex = 2;
            this.HexBox.VScrollBarVisible = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.TreeView);
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.HexBox);
            this.splitContainer.Size = new System.Drawing.Size(1129, 560);
            this.splitContainer.SplitterDistance = 691;
            this.splitContainer.TabIndex = 4;
            // 
            // FSSHTTPandWOPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.RichTextBox); //This richTextBox must be add before the splitContainer so the richTextBox can be avaliable to display.
            this.Controls.Add(this.splitContainer);
            this.Name = "FSSHTTPandWOPIControl";
            this.Size = new System.Drawing.Size(1132, 563);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private Be.Windows.Forms.HexBox HexBox;
        private System.Windows.Forms.SplitContainer splitContainer;
    }
}
