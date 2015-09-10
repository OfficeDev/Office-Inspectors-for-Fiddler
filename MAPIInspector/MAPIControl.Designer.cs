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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(424, 430);
            this.treeView1.TabIndex = 0;
            // 
            // hexBox1
            // 
            this.hexBox1.BodyOffset = 0;
            this.hexBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.HeaderColor = System.Drawing.Color.Maroon;
            this.hexBox1.Location = new System.Drawing.Point(424, 0);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.SelectionLength = ((long)(0));
            this.hexBox1.SelectionStart = ((long)(-1));
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(644, 430);
            this.hexBox1.TabIndex = 1;
            // 
            // MAPIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hexBox1);
            this.Controls.Add(this.treeView1);
            this.Name = "MAPIControl";
            this.Size = new System.Drawing.Size(1068, 430);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private Be.Windows.Forms.HexBox hexBox1;

    }
}
