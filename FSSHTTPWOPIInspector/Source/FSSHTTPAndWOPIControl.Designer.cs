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

        #endregion

        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.ContainerControl Container;
        private Be.Windows.Forms.HexBox HexBox;
        private System.Windows.Forms.Splitter splitter;
    }
}
