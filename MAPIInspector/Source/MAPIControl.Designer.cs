using System.Windows.Forms;
using System;
using System.Text;
using BlockParser;

namespace MapiInspector
{
    public partial class MAPIControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.mapiTreeView = new System.Windows.Forms.TreeView();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.mapiHexBox = new Be.Windows.Forms.HexBox();
			this.cropsHexBox = new Be.Windows.Forms.HexBox();
			this.splitter = new System.Windows.Forms.Splitter();
			this.searchTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.searchTextBox = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.searchTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapiTreeView
			// 
			this.mapiTreeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.mapiTreeView.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapiTreeView.Location = new System.Drawing.Point(0, 36);
			this.mapiTreeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.mapiTreeView.Name = "mapiTreeView";
			this.mapiTreeView.Size = new System.Drawing.Size(634, 451);
			this.mapiTreeView.TabIndex = 0;
			// 
			// splitContainer
			// 
			this.splitContainer.Cursor = System.Windows.Forms.Cursors.Default;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(634, 36);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.splitContainer.Size = new System.Drawing.Size(434, 451);
			this.splitContainer.SplitterDistance = 214;
			this.splitContainer.SplitterWidth = 6;
			this.splitContainer.TabIndex = 4;
			// 
			// mapiHexBox
			// 
			this.mapiHexBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
			this.mapiHexBox.BodyOffset = 0;
			this.mapiHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapiHexBox.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.mapiHexBox.HeaderColor = System.Drawing.Color.Maroon;
			this.mapiHexBox.Location = new System.Drawing.Point(0, 0);
			this.mapiHexBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.mapiHexBox.Name = "mapiHexBox";
			this.mapiHexBox.ReadOnly = true;
			this.mapiHexBox.SelectionLength = ((long)(0));
			this.mapiHexBox.SelectionStart = ((long)(-1));
			this.mapiHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
			this.mapiHexBox.Size = new System.Drawing.Size(434, 451);
			this.mapiHexBox.TabIndex = 2;
			this.mapiHexBox.VScrollBarVisible = true;
			// 
			// cropsHexBox
			// 
			this.cropsHexBox.BodyOffset = 1;
			this.cropsHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cropsHexBox.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cropsHexBox.HeaderColor = System.Drawing.Color.Maroon;
			this.cropsHexBox.Location = new System.Drawing.Point(0, 0);
			this.cropsHexBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cropsHexBox.Name = "cropsHexBox";
			this.cropsHexBox.ReadOnly = true;
			this.cropsHexBox.SelectionLength = ((long)(0));
			this.cropsHexBox.SelectionStart = ((long)(-1));
			this.cropsHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
			this.cropsHexBox.Size = new System.Drawing.Size(150, 46);
			this.cropsHexBox.TabIndex = 4;
			this.cropsHexBox.VScrollBarVisible = true;
			// 
			// splitter
			// 
			this.splitter.Location = new System.Drawing.Point(634, 36);
			this.splitter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(6, 451);
			this.splitter.TabIndex = 5;
			this.splitter.TabStop = false;
			// 
			// searchTableLayoutPanel
			// 
			this.searchTableLayoutPanel.ColumnCount = 2;
			this.searchTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.searchTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
			this.searchTableLayoutPanel.Controls.Add(this.searchTextBox, 0, 0);
			this.searchTableLayoutPanel.Controls.Add(this.searchButton, 1, 0);
			this.searchTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.searchTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.searchTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.searchTableLayoutPanel.Name = "searchTableLayoutPanel";
			this.searchTableLayoutPanel.RowCount = 1;
			this.searchTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.searchTableLayoutPanel.Size = new System.Drawing.Size(1068, 36);
			this.searchTableLayoutPanel.TabIndex = 0;
			// 
			// searchTextBox
			// 
			this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.searchTextBox.Location = new System.Drawing.Point(0, 0);
			this.searchTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this.searchTextBox.Name = "searchTextBox";
			this.searchTextBox.Size = new System.Drawing.Size(1028, 34);
			this.searchTextBox.TabIndex = 0;
			this.searchTextBox.Text = "Search (Ctrl+F)";
			this.searchTextBox.GotFocus += new System.EventHandler(this.SearchTextBox_GotFocus);
			this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
			this.searchTextBox.LostFocus += new System.EventHandler(this.SearchTextBox_LostFocus);
			// 
			// searchButton
			// 
			this.searchButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.searchButton.Image = global::MAPIInspector.Properties.Resources.Search;
			this.searchButton.Location = new System.Drawing.Point(1032, 0);
			this.searchButton.Margin = new System.Windows.Forms.Padding(0);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(36, 36);
			this.searchButton.TabIndex = 1;
			this.toolTip1.SetToolTip(this.searchButton, "Search");
			this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// MAPIControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.mapiTreeView);
			this.Controls.Add(this.searchTableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MAPIControl";
			this.Size = new System.Drawing.Size(1068, 487);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.searchTableLayoutPanel.ResumeLayout(false);
			this.searchTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        private System.Windows.Forms.TreeView mapiTreeView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Be.Windows.Forms.HexBox mapiHexBox;
        private Be.Windows.Forms.HexBox cropsHexBox;
        private System.Windows.Forms.Splitter splitter;
        private TableLayoutPanel searchTableLayoutPanel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}