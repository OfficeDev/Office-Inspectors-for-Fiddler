using System;
using System.Text;
using System.Windows.Forms;

namespace MapiInspector
{
    public partial class MAPIControl
    {
        private void InitializeContextMenus()
        {
            // TreeView context menu
            ContextMenu mapiTreeViewContextMenu = new ContextMenu();
            this.mapiTreeView.ContextMenu = mapiTreeViewContextMenu;
            MenuItem mapiTreeViewMenuItem1 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy selected text");
            MenuItem mapiTreeViewMenuItem2 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy tree");
            MenuItem mapiTreeViewMenuItem3 = this.mapiTreeView.ContextMenu.MenuItems.Add("Expand");
            MenuItem mapiTreeViewMenuItem4 = this.mapiTreeView.ContextMenu.MenuItems.Add("Collapse");
            MenuItem mapiTreeViewMenuItem5 = this.mapiTreeView.ContextMenu.MenuItems.Add("Toggle Debug");
            mapiTreeViewMenuItem1.Click += new EventHandler(MapiTreeViewMenuItem1_Click);
            mapiTreeViewMenuItem2.Click += new EventHandler(MapiTreeViewMenuItem2_Click);
            mapiTreeViewMenuItem3.Click += new EventHandler(MapiTreeViewMenuItem3_Click);
            mapiTreeViewMenuItem4.Click += new EventHandler(MapiTreeViewMenuItem4_Click);
            mapiTreeViewMenuItem5.Click += new EventHandler(MapiTreeViewMenuItem5_Click);

            // MAPI HexBox context menu
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

            // CROPS HexBox context menu
            ContextMenu cm_crops = new ContextMenu();
            this.cropsHexBox.ContextMenu = cm_crops;
            MenuItem item_crops = this.cropsHexBox.ContextMenu.MenuItems.Add("Copy");
            item_crops.Click += new EventHandler(CROPS_Copy);
        }

        private void CopyMethod(object sender, EventArgs e, Be.Windows.Forms.HexBox hexBox)
        {
            byte[] targetBytes = new byte[hexBox.SelectionLength];
            Array.Copy(hexBox.GetAllBytes(), hexBox.SelectionStart, targetBytes, 0, hexBox.SelectionLength);
            string hex = BitConverter.ToString(targetBytes).Replace("-", string.Empty);
            if (!string.IsNullOrEmpty(hex))
            {
                Clipboard.SetText(hex);
            }
        }

        private void MAPI_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.mapiHexBox);
        }

        private void CROPS_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.CROPSHexBox);
        }

        private string CleanString(string text)
        {
            return text.TrimEnd('\0').Replace("\0", "\\0");
        }

        private string GetNodeText(TreeNode node)
        {
            if (node == null) return string.Empty;
            if (node.Tag is global::MAPIInspector.Parsers.BaseStructure.Position position && position.SourceBlock != null)
            {
                return CleanString(position.SourceBlock.Text);
            }
            return CleanString(node.Text);
        }

        private void MapiTreeViewMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetNodeText(this.mapiTreeView.SelectedNode));
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

        private void MapiTreeViewMenuItem5_Click(object sender, EventArgs e)
        {
            Inspector.ToggleDebug();
        }

        private void GetNodeTreeText(StringBuilder sb, TreeNode node, int count)
        {
            var indents = ++count;
            for (int i = 0; i < indents; i++)
                sb.Append("   ");
            sb.AppendLine(GetNodeText(node));
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Tag is string tag && tag == "ignore") continue;
                GetNodeTreeText(sb, tn, indents);
            }
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
                    sb.AppendFormat("${0:X8} | ", (counter / 16) * 16);
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

            sb.Append(" };");
            Clipboard.SetText(sb.ToString());
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SearchTextBox_GotFocus(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Search (Ctrl+F)")
            {
                searchTextBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Search (Ctrl+F)";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                searchTextBox.Focus();
                searchTextBox.SelectAll();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PerformSearch()
        {
            string searchText = searchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText) || searchText == "Search (Ctrl+F)")
            {
                return;
            }
            var foundNode = FindNode(mapiTreeView.Nodes, searchText);
            if (foundNode != null)
            {
                mapiTreeView.SelectedNode = foundNode;
                mapiTreeView.Focus();
                foundNode.EnsureVisible();
            }
        }

        // Recursively searches for a TreeNode whose text contains the searchText (case-insensitive substring match).
        private TreeNode FindNode(TreeNodeCollection nodes, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return null;
            foreach (TreeNode node in nodes)
            {
                if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    return node;
                var found = FindNode(node.Nodes, searchText);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
