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
            this.mapiHexBox = new Be.Windows.Forms.HexBox();
            this.mapiRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // mapiTreeView
            // 
            this.mapiTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.mapiTreeView.Location = new System.Drawing.Point(0, 0);
            this.mapiTreeView.Name = "mapiTreeView";
            this.mapiTreeView.Size = new System.Drawing.Size(424, 430);
            this.mapiTreeView.TabIndex = 0;
            // 
            // mapiHexBox
            // 
            this.mapiHexBox.BodyOffset = 0;
            this.mapiHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapiHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapiHexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.mapiHexBox.Location = new System.Drawing.Point(424, 0);
            this.mapiHexBox.Name = "mapiHexBox";
            this.mapiHexBox.SelectionLength = ((long)(0));
            this.mapiHexBox.SelectionStart = ((long)(-1));
            this.mapiHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mapiHexBox.Size = new System.Drawing.Size(644, 430);
            this.mapiHexBox.TabIndex = 1;
            // 
            // mapiRichTextBox
            // 
            this.mapiRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.mapiRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mapiRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.mapiRichTextBox.Name = "mapiRichTextBox";
            this.mapiRichTextBox.Size = new System.Drawing.Size(421, 430);
            this.mapiRichTextBox.TabIndex = 2;
            this.mapiRichTextBox.Text = "";
            this.mapiRichTextBox.Visible = false;
            // 
            // MAPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapiRichTextBox);
            this.Controls.Add(this.mapiHexBox);
            this.Controls.Add(this.mapiTreeView);
            this.Name = "MAPIControl";
            this.Size = new System.Drawing.Size(1068, 430);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mapiTreeView;
        private Be.Windows.Forms.HexBox mapiHexBox;
        private System.Windows.Forms.RichTextBox mapiRichTextBox;

    }
}
