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
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.mapiTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.mapiTreeView.Location = new System.Drawing.Point(0, 0);
            this.mapiTreeView.Name = "treeView1";
            this.mapiTreeView.Size = new System.Drawing.Size(424, 430);
            this.mapiTreeView.TabIndex = 0;
            // 
            // hexBox1
            // 
            this.mapiHexBox.BodyOffset = 0;
            this.mapiHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapiHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapiHexBox.HeaderColor = System.Drawing.Color.Maroon;
            this.mapiHexBox.Location = new System.Drawing.Point(424, 0);
            this.mapiHexBox.Name = "hexBox1";
            this.mapiHexBox.SelectionLength = ((long)(0));
            this.mapiHexBox.SelectionStart = ((long)(-1));
            this.mapiHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mapiHexBox.Size = new System.Drawing.Size(644, 430);
            this.mapiHexBox.TabIndex = 1;
            // 
            // MAPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapiHexBox);
            this.Controls.Add(this.mapiTreeView);
            this.Name = "MAPIControl";
            this.Size = new System.Drawing.Size(1068, 430);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mapiTreeView;
        private Be.Windows.Forms.HexBox mapiHexBox;

    }
}
